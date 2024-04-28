using recipe.Enums;
using System.ComponentModel.DataAnnotations;

namespace recipe.Models
{
    public class ActivityModel
    {

        public int Id { get; set; }
        public UserModel UserId { get; set; }
        public RecipeModel RecipeId { get; set; }

        public ReviewStatus Status { get; set; } = ReviewStatus.Indifferent;
    
        // Add other properties as needed
    }
}
