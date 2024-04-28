using recipe.Enums;

namespace recipe.Models


{
    public class RecipeModel
    {

        
            public int Id { get; set; }
            public string RecipeName { get; set; } = "";
            public string Ingredients { get; set; } = "";
            public string Instructions { get; set; } = "";
            public string PhotoPath { get; set; } = "";
            public RecipeCategory Category { get; set; } // Add Category property of enum type
        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<ActivityModel> Activities { get; set; }

        // Add other properties as needed

    }
}
