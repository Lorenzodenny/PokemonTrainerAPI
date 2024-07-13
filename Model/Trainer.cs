using System.ComponentModel.DataAnnotations;

namespace PokemonTrainerAPI.Model
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        // Proprietà di navigazione
        public List<Pokemon> Pokemons { get; set; }
    }
}
