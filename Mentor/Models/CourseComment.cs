using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Mentor.Models
{
    public class CourseComment
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }

}
