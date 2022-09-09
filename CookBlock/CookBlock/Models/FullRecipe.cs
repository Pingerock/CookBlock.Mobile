using System.Collections.Generic;

namespace CookBlock.Models
{
    public class FullRecipe
    {
        public Recipe recipe;
        public List<Recipe_Ingredient> ingredients;
        public List<Recipe_Instruction> instructions;
        public List<Comment> comments;
        public List<Recipe_Rating> ratings;
        public Food_Type food_type;

        public FullRecipe(Recipe recipe, List<Recipe_Ingredient> ingredients, List<Recipe_Instruction> instructions,
            List<Comment> comments, List<Recipe_Rating> ratings, Food_Type food_type)
        {
            this.recipe = recipe;
            this.ingredients = ingredients;
            this.instructions = instructions;
            this.comments = comments;
            this.ratings = ratings;
            this.food_type = food_type;
        }
    }
}
