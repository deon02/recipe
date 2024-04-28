namespace recipe.Models
{
    public class CommentDto
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
