using recipe.Models; // Add this using directive

namespace recipe.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeModel>> GetRecipesAsync();
        Task<RecipeModel> GetRecipeByIdAsync(int recipeId);
        Task<bool> UploadRecipeAsync(RecipeModel recipe);
        // Add other methods as needed
    }
}
