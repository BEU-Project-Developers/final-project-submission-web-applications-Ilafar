using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public int Seats { get; set; }
        public string Image { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public List<CourseComment> CourseComments { get; set; }


    }
}
