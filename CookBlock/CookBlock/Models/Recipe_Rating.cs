using System;
using System.Collections.Generic;
using System.Text;

namespace CookBlock.Models
{
    public class Recipe_Rating
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int User_Id { get; set; }
        public int Rating_Score { get; set; }

        public override bool Equals(object obj)
        {
            Recipe_Rating rat = obj as Recipe_Rating;
            return this.Id == rat.Id;
        }
    }
}
