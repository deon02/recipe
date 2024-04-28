namespace recipe.Models
{
    public class CommentModel
    {


        
            public int Id { get; set; }
            public UserModel UserId { get; set; }
            public RecipeModel RecipeId { get; set; }
            public string Content { get; set; } = "";
            public DateTime CreatedAt { get; set; }
            // Add other properties as needed
        
    }
}
