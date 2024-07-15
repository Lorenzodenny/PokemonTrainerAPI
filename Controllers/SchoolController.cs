using Microsoft.AspNetCore.Mvc;
using PokemonTrainerAPI.Abstract;
using PokemonTrainerAPI.DTO;

namespace PokemonTrainerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(ISchoolService schoolService, ILogger<SchoolController> logger)
        {
            _schoolService = schoolService;
            _logger = logger;
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _schoolService.GetStudents();
            return Ok(students);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _schoolService.GetCourses();
            return Ok(courses);
        }

        [HttpPost("students")]
        public async Task<IActionResult> CreateStudent(StudentDTO studentDto)
        {
            var createdStudent = await _schoolService.CreateStudent(studentDto);
            return CreatedAtAction(nameof(GetStudents), new { id = createdStudent.StudentId }, createdStudent);
        }

        [HttpPost("courses")]
        public async Task<IActionResult> CreateCourse(CourseDTO courseDto)
        {
            var createdCourse = await _schoolService.CreateCourse(courseDto);
            return CreatedAtAction(nameof(GetCourses), new { id = createdCourse.CourseId }, createdCourse);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudentInCourse(StudentCourseDTO studentCourseDto)
        {
            var result = await _schoolService.EnrollStudentInCourse(studentCourseDto);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("enroll")]
        public async Task<IActionResult> RemoveStudentFromCourse(StudentCourseDTO studentCourseDto)
        {
            var result = await _schoolService.RemoveStudentFromCourse(studentCourseDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
