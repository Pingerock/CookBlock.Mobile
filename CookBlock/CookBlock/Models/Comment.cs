using System;
using System.Collections.Generic;
using System.Text;

namespace CookBlock.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int User_Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public override bool Equals(object obj)
        {
            Comment com = obj as Comment;
            return this.Id == com.Id;
        }
    }
}
