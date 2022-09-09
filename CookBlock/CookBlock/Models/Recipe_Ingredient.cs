namespace CookBlock.Models
{
    public class Recipe_Ingredient
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public override bool Equals(object obj)
        {
            Recipe_Ingredient ingredient = obj as Recipe_Ingredient;
            return this.Id == ingredient.Id;
        }
    }
}
