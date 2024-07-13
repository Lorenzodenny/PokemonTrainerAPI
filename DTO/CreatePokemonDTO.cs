namespace PokemonTrainerAPI.DTO
{
    public class CreatePokemonDTO
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public int TrainerId { get; set; }
    }
}
