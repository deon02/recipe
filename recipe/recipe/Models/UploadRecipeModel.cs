using recipe.Enums;

namespace recipe.Models
{
    public class RecipeDto
    {
        public string RecipeName { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string PhotoPath { get; set; }

        public RecipeCategory Category { get; set; } // Add Category property



    }
}
