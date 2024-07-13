using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonTrainerAPI.Abstract;
using PokemonTrainerAPI.Data;
using PokemonTrainerAPI.DTO;
using PokemonTrainerAPI.Model;
using PokemonTrainerAPI.Services;

namespace PokemonTrainerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : BaseController
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;

        public enum PokemonType
        {
            Normal,
            Fire,
            Water,
            Electric,
            Grass,
            Ice,
            Fighting,
            Poison,
            Ground,
            Flying,
            Psychic,
            Bug,
            Rock,
            Ghost,
            Dragon,
            Dark,
            Steel,
            Fairy
        }

        public PokemonController(AppDbContext context, IPokemonService pokemonService, ILogger<PokemonController> logger) : base(context)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PokemonDTO>))]
        public async Task<IActionResult> GetPokemons([FromQuery] PokemonType? type = null, [FromQuery] string name = null)
        {
            _logger.LogInformation("Getting pokemons with type: {type} and name: {name}", type, name);
            try
            {
                var pokemons = await _pokemonService.GetPokemons(type, name);
                if (pokemons == null || !pokemons.Any())
                {
                    return NotFound();
                }

                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting pokemons");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPokemon(int id)
        {
            _logger.LogInformation("Getting pokemon with ID: {id}", id);
            try
            {
                var pokemon = await _pokemonService.GetPokemon(id);
                if (pokemon == null)
                {
                    return NotFound();
                }
                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting pokemon with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPokemon(int id, CreatePokemonDTO pokemonDto)
        {
            if (id != pokemonDto.PokemonId)
            {
                return BadRequest();
            }

            try
            {
                var result = await _pokemonService.UpdatePokemon(id, pokemonDto);
                if (!result)
                {
                    return NotFound();
                }

                // Log the state of the entities being tracked
                var trackedEntries = db.ChangeTracker.Entries()
                                          .Where(e => e.State != EntityState.Unchanged)
                                          .ToList();

                foreach (var entry in trackedEntries)
                {
                    Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating pokemon with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PokemonDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostPokemon(CreatePokemonDTO pokemonDto)
        {
            try
            {
                var createdPokemon = await _pokemonService.CreatePokemon(pokemonDto);
                if (createdPokemon == null)
                {
                    return BadRequest("Trainer not found.");
                }

                // Log the state of the entities being tracked
                var trackedEntries = db.ChangeTracker.Entries()
                                          .Where(e => e.State != EntityState.Unchanged)
                                          .ToList();

                foreach (var entry in trackedEntries)
                {
                    Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                }

                return CreatedAtAction(nameof(GetPokemon), new { id = createdPokemon.PokemonId }, createdPokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating pokemon");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            try
            {
                var result = await _pokemonService.DeletePokemon(id);
                if (!result)
                {
                    return NotFound();
                }

                // Log the state of the entities being tracked
                var trackedEntries = db.ChangeTracker.Entries()
                                          .Where(e => e.State != EntityState.Unchanged)
                                          .ToList();

                foreach (var entry in trackedEntries)
                {
                    Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting pokemon with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        private bool PokemonExists(int id)
        {
            return db.Pokemons.Any(e => e.PokemonId == id);
        }
    }
}
