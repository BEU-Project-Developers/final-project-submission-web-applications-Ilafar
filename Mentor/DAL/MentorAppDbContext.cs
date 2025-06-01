using Mentor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mentor.DAL
{
    public class MentorAppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PricingService> PricingServices { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<MentorTags> MentorTags { get; set; }
        public DbSet<WhyUs> WhyUs { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        public MentorAppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricingService>().HasKey(ps => new { ps.PricingId, ps.ServiceId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
