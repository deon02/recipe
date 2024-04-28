using Microsoft.AspNetCore.Mvc;
using recipe.context;
using recipe.Models;
using Microsoft.EntityFrameworkCore;
using recipe.Enums; // Import the Enums namespace if it's not already imported

namespace recipe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeManagementController : ControllerBase
    {
        private readonly appDbContext _dbContext;

        public RecipeManagementController(appDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeModel>>> GetAllRecipes()
        {
            try
            {
                // Fetch all recipes from the database asynchronously
                var recipes = await _dbContext.Recipes.ToListAsync();
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeModel>> GetRecipeById(int id)
        {
            try
            {
                // Fetch a recipe by its ID from the database asynchronously
                var recipe = await _dbContext.Recipes.FindAsync(id);
                if (recipe == null)
                {
                    return NotFound("Recipe not found");
                }
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(RecipeDto recipeDto)
        {
            if (recipeDto == null)
            {
                return BadRequest("Recipe data is null");
            }

            // Convert RecipeDto to RecipeModel
            var recipeModel = new RecipeModel
            {
                RecipeName = recipeDto.RecipeName,
                Ingredients = recipeDto.Ingredients,
                Instructions = recipeDto.Instructions,
                PhotoPath = recipeDto.PhotoPath,
                Category = recipeDto.Category // Map the Category property from RecipeDto
            };

            try
            {
                // Add the recipe to the database
                _dbContext.Recipes.Add(recipeModel);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<RecipeModel>>> GetRecipesByCategory(string category)
        {
            try
            {
                // Parse the category string to RecipeCategory enum
                if (!Enum.TryParse(category, true, out RecipeCategory categoryEnum))
                {
                    return BadRequest($"Invalid category: '{category}'");
                }

                // Filter recipes by category from the database asynchronously
                var recipes = await _dbContext.Recipes
                                    .Where(r => r.Category == categoryEnum)
                                    .ToListAsync();

                if (recipes == null || recipes.Count == 0)
                {
                    return NotFound($"No recipes found for category '{category}'");
                }

                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }







    }
}
