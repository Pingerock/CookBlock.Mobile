namespace CookBlock.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int User_Id { get; set; }

        public override bool Equals(object obj)
        {
            Favourite fav = obj as Favourite;
            return this.Id == fav.Id;
        }
    }
}
