using recipe.Models;
using recipe.context;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using recipe.Services;

namespace Recipe.Services // Adjust namespace as needed
{
    public class RecipeService : IRecipeService
    {
        private readonly appDbContext _dbContext;

        public RecipeService(appDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RecipeModel>> GetRecipesAsync()
        {
            // Implement logic to fetch recipes from the database asynchronously
            // For demonstration purposes, let's assume we fetch all recipes from the database
            return await _dbContext.Recipes.ToListAsync();
        }

        public async Task<RecipeModel> GetRecipeByIdAsync(int recipeId)
        {
            // Implement logic to fetch a recipe by its ID asynchronously
            // For demonstration purposes, let's assume we fetch the recipe from the database by ID
            return await _dbContext.Recipes.FindAsync(recipeId);
        }

        public async Task<bool> UploadRecipeAsync(RecipeModel recipe)
        {
            try
            {
                // Add logic here to save the recipe to the database or perform any necessary operations
                _dbContext.Recipes.Add(recipe);
                await _dbContext.SaveChangesAsync();

                return true; // Return true if the upload was successful
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during recipe upload: " + ex.Message);
                return false; // Return false if an error occurred during the upload
            }
        }
    }
}
