using PokemonTrainerAPI.DTO;

namespace PokemonTrainerAPI.Abstract
{
    public interface ISchoolService
    {
        Task<IEnumerable<StudentDTO>> GetStudents();
        Task<IEnumerable<CourseDTO>> GetCourses();
        Task<StudentDTO> CreateStudent(StudentDTO studentDto);
        Task<CourseDTO> CreateCourse(CourseDTO courseDto);
        Task<CourseDTO> GetCourse(int id);
        Task<bool> UpdateCourse(int id, CourseDTO courseDto);
        Task<bool> DeleteCourse(int id);
        Task<bool> EnrollStudentInCourse(StudentCourseDTO studentCourseDto);
        Task<bool> RemoveStudentFromCourse(StudentCourseDTO studentCourseDto);
    }

}
