using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("t_e_animal_ani")]
    public class Animal
    {
        [Key]
        [Column("ani_id")]
        public int Id { get; set; }
        
        [Column("ani_name")]
        public string Name { get; set; }
        [Column("ani_species")]
        public string Species { get; set; }
        [Column("ani_poids")]
        public int Poids { get; set; }
        [Column("ani_taille")]
        public int Taille { get; set; }
    }
}
