using static PokemonTrainerAPI.Controllers.PokemonController;

namespace PokemonTrainerAPI.Model
{
    public class Pokemon
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public PokemonType Type { get; set; } 
        public int TrainerId { get; set; }

        // Proprietà di navigazione
        public Trainer Trainer { get; set; }
    }

}
