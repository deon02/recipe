// IActivityService.cs
using recipe.Models;
using System.Threading.Tasks;

public interface IActivityService
{
    Task<bool> LikeRecipe(int recipeId, int userId);
    Task<bool> DislikeRecipe(int recipeId, int userId);
}


