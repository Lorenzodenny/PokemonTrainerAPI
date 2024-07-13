using PokemonTrainerAPI.Abstract;
using PokemonTrainerAPI.Controllers;

namespace PokemonTrainerAPI.Model
{
    public class PokemonFactory : IPokemonFactory
    {
        public Pokemon FactoryPokemon(string name, string species, string type, int trainerId)
        {
            return new Pokemon
            {
                Name = name,
                Species = species,
                Type = Enum.Parse<PokemonController.PokemonType>(type, true),
                TrainerId = trainerId
            };
        }
    }
}
