using AutoMapper;
using PokemonTrainerAPI.Model;
using PokemonTrainerAPI.DTO;
using static PokemonTrainerAPI.Controllers.PokemonController;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mappature tra entità e DTO
        CreateMap<Pokemon, PokemonDTO>()
            .ForMember(dto => dto.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<CreatePokemonDTO, Pokemon>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<PokemonType>(src.Type)));

        // Configura il mapping per TrainerDTO
        CreateMap<Trainer, TrainerDTO>()
            .ForCtorParam("TrainerId", opt => opt.MapFrom(src => src.TrainerId))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Surname", opt => opt.MapFrom(src => src.Surname))
            .ForCtorParam("Age", opt => opt.MapFrom(src => src.Age))
            .ForCtorParam("Gender", opt => opt.MapFrom(src => src.Gender))
            .ForCtorParam("NumberOfPokemons", opt => opt.MapFrom(src => src.Pokemons.Count));

        // Aggiungi questa riga per configurare la mappatura inversa
        CreateMap<TrainerDTO, Trainer>()
            .ForMember(dest => dest.Pokemons, opt => opt.Ignore()); // Ignora la lista di Pokemons perché non viene gestita direttamente dal DTO

        // setta student ie corsi

        CreateMap<Student, StudentDTO>().ReverseMap();
        CreateMap<Course, CourseDTO>().ReverseMap();
        CreateMap<StudentCourse, StudentCourseDTO>().ReverseMap();
    }
}
