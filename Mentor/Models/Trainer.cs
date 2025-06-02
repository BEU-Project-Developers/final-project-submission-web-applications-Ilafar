using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mentor.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Expertise { get; set; }
        [Required]
        public string Bio { get; set; }
        public string TrainerImage { get; set; }
        public List<Course> Courses { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedinUrl { get; set; }

    }
}
