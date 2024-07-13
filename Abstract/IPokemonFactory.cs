using PokemonTrainerAPI.Model;

namespace PokemonTrainerAPI.Abstract
{
    public interface IPokemonFactory
    {
        Pokemon FactoryPokemon(string name, string species, string type, int trainerId);
    }

}
