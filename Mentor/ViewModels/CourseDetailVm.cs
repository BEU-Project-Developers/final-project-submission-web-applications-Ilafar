using Mentor.Models;

namespace Mentor.ViewModels
{
    public class CourseDetailVm
    {
        public Course Course { get; set; }
        public CourseComment CourseComment { get; set; }
        public int? TotalComments { get; set; }
        public bool IsCourseBought { get; set; }
        public int BuyCount { get; set; }
    }
}
