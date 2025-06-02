namespace Mentor.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
        public string UserId { get; set; }   
        public int CourseId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public AppUser User { get; set; }  
        public Course Course { get; set; }
    }

}
