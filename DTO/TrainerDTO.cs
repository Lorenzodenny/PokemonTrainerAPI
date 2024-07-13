namespace PokemonTrainerAPI.DTO
{
    public record TrainerDTO(int TrainerId, string Name, string Surname, int Age, string Gender, int NumberOfPokemons);
}
