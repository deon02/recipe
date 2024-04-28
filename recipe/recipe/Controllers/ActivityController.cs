using Microsoft.AspNetCore.Mvc;
using recipe.Models;
using System.Threading.Tasks;
using recipe.Enums;
using recipe.context;
using Microsoft.EntityFrameworkCore;

namespace recipe.Controllers
{
    [ApiController]
    [Route("api/Activity")]
    public class ActivityController : ControllerBase
    {
        private readonly appDbContext _dbContext;

        public ActivityController(appDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikeRecipe([FromBody] LikeRequest request)
        {
            int recipeId = request.RecipeId;
            int userId = request.UserId;

            var existingActivity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.RecipeId.Id == recipeId && a.UserId.Id == userId);

            if (existingActivity != null)
            {
                // If the user has already liked the recipe, remove the like
                if (existingActivity.Status == ReviewStatus.Liked)
                {
                    _dbContext.Activities.Remove(existingActivity);
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }

                // If the user has disliked the recipe, change the status to liked
                existingActivity.Status = ReviewStatus.Liked;
            }
            else
            {
                // Add new activity
                _dbContext.Activities.Add(new ActivityModel
                {
                    UserId = await _dbContext.Users.FindAsync(userId),
                    RecipeId = await _dbContext.Recipes.FindAsync(recipeId),
                    Status = ReviewStatus.Liked,
                });
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{recipeId}/dislike")]
        public async Task<IActionResult> DislikeRecipe(int recipeId, int userId)
        {
            var existingActivity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.RecipeId.Id == recipeId && a.UserId.Id == userId);

            if (existingActivity != null)
            {
                // If the user has already disliked the recipe, remove the dislike
                if (existingActivity.Status == ReviewStatus.Disliked)
                {
                    _dbContext.Activities.Remove(existingActivity);
                    await _dbContext.SaveChangesAsync();
                    return Ok("Dislike removed successfully");
                }

                // If the user has liked the recipe, change the status to disliked
                existingActivity.Status = ReviewStatus.Disliked;
            }
            else
            {
                // Add new activity
                _dbContext.Activities.Add(new ActivityModel
                {
                    UserId = await _dbContext.Users.FindAsync(userId),
                    RecipeId = await _dbContext.Recipes.FindAsync(recipeId),
                    Status = ReviewStatus.Disliked,
                });
            }

            await _dbContext.SaveChangesAsync();
            return Ok("Recipe disliked successfully");
        }


        [HttpGet("{recipeId}/likeCount")]
        public async Task<IActionResult> GetLikeCount(int recipeId)
        {
            // Query the database to count the number of likes for the recipe
            var likeCount = await _dbContext.Activities
                .Where(a => a.RecipeId.Id == recipeId && a.Status == ReviewStatus.Liked)
                .CountAsync();

            return Ok(likeCount);
        }

        [HttpGet("sorted-by-likes")]
        public async Task<ActionResult<IEnumerable<RecipeModel>>> GetRecipesSortedByLikes()
        {
            try
            {
                // Query to fetch recipes along with their like counts
                var recipesWithLikeCounts = await _dbContext.Recipes
                    .Select(r => new
                    {
                        Recipe = r,
                        LikeCount = _dbContext.Activities.Count(a => a.RecipeId.Id == r.Id && a.Status == ReviewStatus.Liked)

                    })
                    .OrderByDescending(r => r.LikeCount) // Sort recipes by like counts in descending order
                    .ToListAsync();

                // Extract recipes from the result
                var sortedRecipes = recipesWithLikeCounts.Select(r => r.Recipe);

                return Ok(sortedRecipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("user-liked-recipes/{userId}")]
        public async Task<IActionResult> GetUserLikedRecipes(int userId)
        {
            try
            {
                // Query to fetch the recipes liked by the user
                var likedRecipes = await _dbContext.Activities
                    .Where(a => a.UserId.Id == userId && a.Status == ReviewStatus.Liked)
                    .Select(a => a.RecipeId)
                    .ToListAsync();

                return Ok(likedRecipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public class LikeRequest
        {
            public int RecipeId { get; set; }
            public int UserId { get; set; }
        }
    }
}
