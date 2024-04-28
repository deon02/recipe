using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using recipe.context;
using recipe.Models;
using recipe.Enums;

public class ActivityService
{
    private readonly appDbContext _dbContext;

    public ActivityService(appDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LikeRecipe(int recipeId, int userId)
    {
        var existingActivity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.RecipeId.Id == recipeId && a.UserId.Id == userId);

        if (existingActivity == null)
        {
            // Add new activity
            _dbContext.Activities.Add(new ActivityModel
            {
                UserId = await _dbContext.Users.FindAsync(userId),
                RecipeId = await _dbContext.Recipes.FindAsync(recipeId),
                Status = ReviewStatus.Liked,
            });
        }
        else
        {
            existingActivity.Status = ReviewStatus.Indifferent;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DislikeRecipe(int recipeId, int userId)
    {
        var existingActivity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.RecipeId.Id == recipeId && a.UserId.Id == userId);

        if (existingActivity == null)
        {
            // Add new activity
            _dbContext.Activities.Add(new ActivityModel
            {
                UserId = await _dbContext.Users.FindAsync(userId),
                RecipeId = await _dbContext.Recipes.FindAsync(recipeId),
                Status = ReviewStatus.Disliked,
            });
        }
        else
        {
            existingActivity.Status = ReviewStatus.Indifferent;
        }

        await _dbContext.SaveChangesAsync();
    }
}
