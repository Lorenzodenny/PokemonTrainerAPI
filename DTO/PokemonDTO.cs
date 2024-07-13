namespace PokemonTrainerAPI.DTO
{
    public record PokemonDTO
    {
        public int PokemonId { get; init; }
        public string Name { get; init; }
        public string Species { get; init; }
        public string Type { get; init; }
        public int TrainerId { get; init; }
        public string TrainerName { get; init; }
        public string TrainerSurname { get; init; }
    }

}
