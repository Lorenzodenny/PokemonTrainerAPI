using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonTrainerAPI.Abstract;
using PokemonTrainerAPI.Data;
using PokemonTrainerAPI.DTO;
using PokemonTrainerAPI.Model;

namespace PokemonTrainerAPI.Service
{
    public class SchoolService : BaseController, ISchoolService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SchoolService> _logger;

        public SchoolService(AppDbContext context, IMapper mapper, ILogger<SchoolService> logger) : base(context)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<StudentDTO>> GetStudents()
        {
            var students = await db.Students.ToListAsync();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<IEnumerable<CourseDTO>> GetCourses()
        {
            var courses = await db.Courses.ToListAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<StudentDTO> CreateStudent(StudentDTO studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<CourseDTO> CreateCourse(CourseDTO courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);
            db.Courses.Add(course);
            await db.SaveChangesAsync();
            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<bool> EnrollStudentInCourse(StudentCourseDTO studentCourseDto)
        {
            var studentCourse = _mapper.Map<StudentCourse>(studentCourseDto);
            db.StudentCourses.Add(studentCourse);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<CourseDTO> GetCourse(int id)
        {
            var course = await db.Courses.FindAsync(id);
            return course != null ? _mapper.Map<CourseDTO>(course) : null;
        }

        public async Task<bool> UpdateCourse(int id, CourseDTO courseDto)
        {
            var course = await db.Courses.FindAsync(id);
            if (course == null) return false;

            _mapper.Map(courseDto, course);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await db.Courses.FindAsync(id);
            if (course == null) return false;

            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return true;
        }


        public async Task<bool> RemoveStudentFromCourse(StudentCourseDTO studentCourseDto)
        {
            var studentCourse = await db.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentCourseDto.StudentId && sc.CourseId == studentCourseDto.CourseId);

            if (studentCourse == null)
            {
                return false;
            }

            db.StudentCourses.Remove(studentCourse);
            await db.SaveChangesAsync();
            return true;
        }
    }

}
