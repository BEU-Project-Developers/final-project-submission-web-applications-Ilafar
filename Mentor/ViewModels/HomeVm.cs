using Mentor.Models;

namespace Mentor.ViewModels
{
    public class HomeVm
    {
        public List<Course> Courses { get; set; }
        public List<Trainer> Trainers { get; set; }
        public List<AppUser> Users { get; set; }
        public List<MentorTags> MentorTags { get; set; }
    }
}
