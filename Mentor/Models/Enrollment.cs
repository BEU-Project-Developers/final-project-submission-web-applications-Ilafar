﻿namespace Mentor.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
