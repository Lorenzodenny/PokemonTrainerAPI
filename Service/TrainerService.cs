using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonTrainerAPI.Data;
using PokemonTrainerAPI.DTO;
using PokemonTrainerAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static PokemonTrainerAPI.Controllers.PokemonController;

namespace PokemonTrainerAPI.Services
{
    public class TrainerService : BaseController, ITrainerService
    {
        private readonly TrainerBuilder _trainerBuilder;
        private readonly IMapper _mapper;
        private readonly ILogger<TrainerService> _logger;

        public TrainerService(AppDbContext context, TrainerBuilder trainerBuilder, IMapper mapper, ILogger<TrainerService> logger) : base(context)
        {
            _trainerBuilder = trainerBuilder;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TrainerDTO>> GetTrainers(string name, string gender)
        {
            _logger.LogInformation("Fetching trainers with filters - Name: {name}, Gender: {gender}", name, gender);
            try
            {
                IQueryable<Trainer> query = db.Trainers.Include(t => t.Pokemons);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(t => t.Name.ToLower().Contains(name.ToLower()));
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    query = query.Where(t => t.Gender.ToLower().Contains(gender.ToLower()));
                }

                var trainers = await query.ToListAsync();

                return _mapper.Map<IEnumerable<TrainerDTO>>(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trainers");
                throw;
            }
        }

        public async Task<TrainerDTO> GetTrainer(int id)
        {
            _logger.LogInformation("Fetching trainer with ID: {id}", id);
            try
            {
                var trainer = await db.Trainers
                    .Include(t => t.Pokemons)
                    .FirstOrDefaultAsync(t => t.TrainerId == id);

                return _mapper.Map<TrainerDTO>(trainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trainer with ID: {id}", id);
                throw;
            }
        }

        public async Task<bool> UpdateTrainer(int id, TrainerDTO trainerDto)
        {
            if (id != trainerDto.TrainerId)
            {
                _logger.LogWarning("Trainer ID mismatch: {id} != {trainerId}", id, trainerDto.TrainerId);
                return false;
            }

            try
            {
                var trainer = await db.Trainers.FindAsync(id);
                if (trainer == null)
                {
                    _logger.LogWarning("Trainer with ID: {id} not found", id);
                    return false;
                }

                _mapper.Map(trainerDto, trainer);

                db.Entry(trainer).State = EntityState.Modified;
                await db.SaveChangesAsync();

                _logger.LogInformation("Updated trainer with ID: {id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating trainer with ID: {id}", id);
                throw;
            }
        }

        //// Esempio di update che aggiunge, modifica e cancella un oggetto di un entità child 

        //public async Task<bool> UpdateTrainer(int id, TrainerDTO trainerDto)
        //{
        //    try
        //    {
        //        if (id != trainerDto.TrainerId)
        //        {
        //            _logger.LogWarning("Trainer ID mismatch: {id} != {trainerId}", id, trainerDto.TrainerId);
        //            return false;
        //        }

        //        var trainer = await db.Trainers.Include(t => t.Pokemons).FirstOrDefaultAsync(t => t.TrainerId == id);
        //        if (trainer == null)
        //        {
        //            _logger.LogWarning("Trainer with ID: {id} not found", id);
        //            return false;
        //        }

        //        // Modifica delle proprietà dell'entità Trainer
        //        trainer.Name = trainerDto.Name;
        //        trainer.Surname = trainerDto.Surname;
        //        trainer.Age = trainerDto.Age;
        //        trainer.Gender = trainerDto.Gender;

        //        // Aggiunta di un nuovo Pokémon
        //        trainer.Pokemons.Add(new Pokemon
        //        {
        //            Name = "Charmander",
        //            Species = "Fire",
        //            Type = PokemonType.Fire,
        //            TrainerId = trainer.TrainerId
        //        });

        //        int IdPokemonEsistente = 10;
        //        // Modifica di un Pokémon esistente
        //        var pokemonToUpdate = trainer.Pokemons.FirstOrDefault(p => p.PokemonId == IdPokemonEsistente);
        //        if (pokemonToUpdate != null)
        //        {
        //            pokemonToUpdate.Name = "Updated Name";
        //        }

        //        int altroIdPokemon = 7;
        //        // Rimozione di un Pokémon esistente
        //        var pokemonToRemove = trainer.Pokemons.FirstOrDefault(p => p.PokemonId == altroIdPokemon);
        //        if (pokemonToRemove != null)
        //        {
        //            db.Pokemons.Remove(pokemonToRemove);
        //        }

        //        db.Entry(trainer).State = EntityState.Modified;
        //        await db.SaveChangesAsync();

        //        _logger.LogInformation("Updated trainer with ID: {id}", id);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating trainer with ID: {id}", id);
        //        return false;
        //    }
        //}

        public async Task<TrainerDTO> CreateTrainer(TrainerDTO trainerDto)
        {
            _logger.LogInformation("Creating a new trainer");
            try
            {
                var trainer = _trainerBuilder
                    .WithName(trainerDto.Name)
                    .WithSurname(trainerDto.Surname)
                    .WithAge(trainerDto.Age)
                    .WithGender(trainerDto.Gender)
                    .Build();

                db.Trainers.Add(trainer);
                await db.SaveChangesAsync();

                return _mapper.Map<TrainerDTO>(trainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a trainer");
                throw;
            }
        }

        public async Task<bool> DeleteTrainer(int id)
        {
            _logger.LogInformation("Deleting trainer with ID: {id}", id);
            try
            {
                var trainer = await db.Trainers.FindAsync(id);
                if (trainer == null)
                {
                    _logger.LogWarning("Trainer with ID: {id} not found for deletion", id);
                    return false;
                }

                db.Trainers.Remove(trainer);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting trainer with ID: {id}", id);
                throw;
            }
        }
    }
}
