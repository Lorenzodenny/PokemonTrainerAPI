namespace PokemonTrainerAPI.Model
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
