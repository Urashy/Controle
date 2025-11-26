namespace Front.Models
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public int Poids { get; set; }
        public int Taille { get; set; }
    }
}
