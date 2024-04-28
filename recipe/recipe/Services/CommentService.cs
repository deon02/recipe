//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using recipe.context;
//using recipe.Models;

//public class CommentService
//{
//    private readonly appDbContext dbContext;

//    public CommentService(appDbContext dbContext)
//    {
//        this.dbContext = dbContext;
//    }

//    public async Task<bool> CommentRecipe(RecipeModel recipe, UserModel user, string comment)
//    {
//        if (recipe == null || user == null)
//        {
//            return false; // Recipe or user doesn't exist
//        }

//        var newComment = new CommentModel
//        {
//            RecipeId = recipe,
//            UserId = user,
//            Content = comment,
//            CreatedAt = DateTime.UtcNow, // Use UtcNow instead of DateTime.Now

//        };

//        await this.dbContext.Comments.AddAsync(newComment);
//        await this.dbContext.SaveChangesAsync();

//        return true; // Successfully added comment
//    }

//    public async Task<CommentModel[]> GetComments(int recipeId)
//    {
//        var recipeExists = await this.dbContext.Recipes.FindAsync(recipeId);
//        if (recipeExists == null)
//        {
//            throw new InvalidOperationException("Recipe not found"); // Throw an exception if the recipe doesn't exist
//        }

//        var comments = await this.dbContext.Comments
//            .Where(c => c.RecipeId.Id == recipeId) // Assuming RecipeId is a RecipeModel object
//            .OrderByDescending(c => c.CreatedAt)
//            .ToArrayAsync();

//        return comments;
//    }


//}
