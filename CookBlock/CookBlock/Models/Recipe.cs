using System;
using System.Collections.Generic;
using System.Text;

namespace CookBlock.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string? Picture_base64 { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Food_Type_Id { get; set; }

        public override bool Equals(object obj)
        {
            Recipe recipe = obj as Recipe;
            return this.Id == recipe.Id;
        }
    }
}
