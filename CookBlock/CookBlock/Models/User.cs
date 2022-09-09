using System;
using System.Collections.Generic;
using System.Text;

namespace CookBlock.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime Date { get; set; }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            return this.Id == user.Id;
        }
    }
}
