using PokemonTrainerAPI.DTO;

namespace PokemonTrainerAPI.Services
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerDTO>> GetTrainers(string name, string gender);
        Task<TrainerDTO> GetTrainer(int id);
        Task<bool> UpdateTrainer(int id, TrainerDTO trainerDto);
        Task<TrainerDTO> CreateTrainer(TrainerDTO trainerDto);
        Task<bool> DeleteTrainer(int id);
    }
}
