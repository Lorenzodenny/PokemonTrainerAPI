using PokemonTrainerAPI.Controllers;
using PokemonTrainerAPI.DTO;

namespace PokemonTrainerAPI.Services
{
    public interface IPokemonService
    {
        Task<IEnumerable<PokemonDTO>> GetPokemons(PokemonController.PokemonType? type, string name);
        Task<PokemonDTO> GetPokemon(int id);
        Task<bool> UpdatePokemon(int id, CreatePokemonDTO pokemonDto);
        Task<PokemonDTO> CreatePokemon(CreatePokemonDTO pokemonDto);
        Task<bool> DeletePokemon(int id);
    }
}
