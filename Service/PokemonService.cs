using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonTrainerAPI.Abstract;
using PokemonTrainerAPI.Controllers;
using PokemonTrainerAPI.Data;
using PokemonTrainerAPI.DTO;
using PokemonTrainerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PokemonTrainerAPI.Services
{
    public class PokemonService : BaseController, IPokemonService
    {
        private readonly IPokemonFactory _pokemonFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(AppDbContext context, IPokemonFactory pokemonFactory, IMapper mapper, ILogger<PokemonService> logger) : base(context)
        {
            _pokemonFactory = pokemonFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PokemonDTO>> GetPokemons(PokemonController.PokemonType? type, string name)
        {
            _logger.LogInformation("Fetching pokemons with filters - Type: {type}, Name: {name}", type, name);
            try
            {
                IQueryable<Pokemon> query = db.Pokemons.Include(p => p.Trainer);

                if (type.HasValue)
                {
                    query = query.Where(p => p.Type == type.Value);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Species.ToLower().Contains(name.ToLower()));
                }

                var pokemons = await query.ToListAsync();

                return _mapper.Map<IEnumerable<PokemonDTO>>(pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching pokemons");
                throw;
            }
        }

        public async Task<PokemonDTO> GetPokemon(int id)
        {
            _logger.LogInformation("Fetching pokemon with ID: {id}", id);
            try
            {
                var pokemon = await db.Pokemons
                    .Include(p => p.Trainer)
                    .FirstOrDefaultAsync(p => p.PokemonId == id);

                return _mapper.Map<PokemonDTO>(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching pokemon with ID: {id}", id);
                throw;
            }
        }

        public async Task<bool> UpdatePokemon(int id, CreatePokemonDTO pokemonDto)
        {
            if (id != pokemonDto.PokemonId)
            {
                return false;
            }

            try
            {
                var pokemon = await db.Pokemons.Include(p => p.Trainer).FirstOrDefaultAsync(p => p.PokemonId == id);
                if (pokemon == null)
                {
                    return false;
                }

                var trainer = await db.Trainers.FindAsync(pokemonDto.TrainerId);
                if (trainer == null)
                {
                    return false;
                }

                // Utilizza AutoMapper per mappare i campi da DTO a entità
                _mapper.Map(pokemonDto, pokemon);

                pokemon.Trainer = trainer;

                db.Entry(pokemon).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating pokemon with ID: {id}", id);
                throw;
            }
        }

        public async Task<PokemonDTO> CreatePokemon(CreatePokemonDTO pokemonDto)
        {
            try
            {
                var trainer = await db.Trainers.FindAsync(pokemonDto.TrainerId);
                if (trainer == null)
                {
                    return null;
                }

                var pokemon = _pokemonFactory.FactoryPokemon(pokemonDto.Name, pokemonDto.Species, pokemonDto.Type, pokemonDto.TrainerId);
                _mapper.Map(pokemonDto, pokemon);
                pokemon.Trainer = trainer;

                db.Pokemons.Add(pokemon);
                await db.SaveChangesAsync();

                return _mapper.Map<PokemonDTO>(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating pokemon");
                throw;
            }
        }

        public async Task<bool> DeletePokemon(int id)
        {
            try
            {
                var pokemon = await db.Pokemons.FindAsync(id);
                if (pokemon == null)
                {
                    return false;
                }

                db.Pokemons.Remove(pokemon);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting pokemon with ID: {id}", id);
                throw;
            }
        }
    }
}
