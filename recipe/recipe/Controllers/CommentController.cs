using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe.context;
using recipe.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace recipe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly appDbContext _dbContext;

        public CommentController(appDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CommentRecipe(string com,int recipeId, int userId )
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return BadRequest("Recipe not found");

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var newComment = new CommentModel
            {
                RecipeId = recipe,
                UserId = user,
                Content = com,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Comments.Add(newComment);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetComments(int recipeId)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return NotFound("Recipe not found");

            var comments = await _dbContext.Comments
                .Where(c => c.RecipeId.Id == recipeId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    UserId = c.UserId.Id,
                    Username = c.UserId.Username, // Assuming Username is the property for the username in the Users table
                    RecipeId = c.RecipeId.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            if (comments.Any())
                return Ok(comments);
            else
                return NotFound("No comments found for the recipe");
        }

    }
}
