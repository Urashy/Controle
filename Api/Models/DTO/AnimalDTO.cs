using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.DTO
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Poids { get; set; }
        public int Taille { get; set; }
    }
}
