using Microsoft.AspNetCore.Mvc;
using PokemonTrainerAPI.Abstract;
using PokemonTrainerAPI.Data;
using PokemonTrainerAPI.DTO;
using PokemonTrainerAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokemonTrainerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController
    {
        private readonly ISchoolService _schoolService;

        public CourseController(AppDbContext context, ISchoolService schoolService) : base(context)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetAllCourses()
        {
            var courses = await _schoolService.GetCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
        {
            var course = await _schoolService.GetCourse(id);
            if (course == null)
            {
                return NotFound($"Course with ID {id} not found.");
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> CreateCourse([FromBody] CourseDTO courseDto)
        {
            var newCourse = await _schoolService.CreateCourse(courseDto);
            return CreatedAtAction(nameof(GetCourse), new { id = newCourse.CourseId }, newCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDTO courseDto)
        {
            if (id != courseDto.CourseId)
            {
                return BadRequest("ID mismatch.");
            }
            var success = await _schoolService.UpdateCourse(id, courseDto);
            if (!success)
            {
                return NotFound($"Course with ID {id} not found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var success = await _schoolService.DeleteCourse(id);
            if (!success)
            {
                return NotFound($"Course with ID {id} not found.");
            }
            return NoContent();
        }
    }
}
