using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonTrainerAPI.Data;
using PokemonTrainerAPI.Model;
using PokemonTrainerAPI.DTO;
using PokemonTrainerAPI.Services;

namespace PokemonTrainerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : BaseController
    {
        private readonly ITrainerService _trainerService;
        private readonly ILogger<TrainerController> _logger;

        public TrainerController(AppDbContext context, ITrainerService trainerService, ILogger<TrainerController> logger) : base(context)
        {
            _trainerService = trainerService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrainerDTO>))]
        public async Task<IActionResult> GetTrainers([FromQuery] string name = null, [FromQuery] string gender = null)
        {
            _logger.LogInformation("Getting trainers with name: {name} and gender: {gender}", name, gender);
            try
            {
                var trainers = await _trainerService.GetTrainers(name, gender);
                return Ok(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting trainers");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrainerDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainer(int id)
        {
            _logger.LogInformation("Getting trainer with ID: {id}", id);
            try
            {
                var trainer = await _trainerService.GetTrainer(id);

                if (trainer == null)
                {
                    return NotFound();
                }

                return Ok(trainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting trainer with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTrainer(int id, TrainerDTO trainerDto)
        {
            if (id != trainerDto.TrainerId)
            {
                return BadRequest();
            }

            try
            {
                bool updateResult = await _trainerService.UpdateTrainer(id, trainerDto);
                if (!updateResult)
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
                _logger.LogError(ex, "An error occurred while updating trainer with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

      

        [HttpPost]
        public async Task<IActionResult> PostTrainer(TrainerDTO trainerDto)
        {
            try
            {
                TrainerDTO createdTrainer = await _trainerService.CreateTrainer(trainerDto);

                // Log the state of the entities being tracked
                var trackedEntries = db.ChangeTracker.Entries()
                                          .Where(e => e.State != EntityState.Unchanged)
                                          .ToList();

                foreach (var entry in trackedEntries)
                {
                    Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                }

                return CreatedAtAction(nameof(GetTrainer), new { id = createdTrainer.TrainerId }, createdTrainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating trainer");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            try
            {
                bool deleteResult = await _trainerService.DeleteTrainer(id);
                if (!deleteResult)
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
                _logger.LogError(ex, "An error occurred while deleting trainer with ID: {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        private bool TrainerExists(int id)
        {
            return db.Trainers.Any(e => e.TrainerId == id);
        }
    }
}
