using System.ComponentModel.DataAnnotations;

namespace Mentor.Models
{
    public class MentorTags
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
