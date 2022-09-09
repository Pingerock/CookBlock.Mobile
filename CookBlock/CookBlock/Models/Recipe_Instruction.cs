namespace CookBlock.Models
{
    public class Recipe_Instruction
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int Position { get; set; }
        public string Picture_base64 { get; set; }
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            Recipe_Instruction instruction = obj as Recipe_Instruction;
            return this.Id == instruction.Id;
        }
    }
}
