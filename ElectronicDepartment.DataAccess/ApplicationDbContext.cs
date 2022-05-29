using ElectronicDepartment.DomainEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElectronicDepartment.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<StudentOnLesson> Marks { get; set; } = default!;

        public virtual DbSet<Group> Groups { get; set; } = default!;

        public virtual DbSet<Student> Students { get; set; } = default!;

        public virtual DbSet<Teacher> Teachers { get; set; } = default!;

        public virtual DbSet<Lesson> Lessons { get; set; } = default!;

        public virtual DbSet<CourseTeacher> CourseTeachers { get; set; } = default!;

        public virtual DbSet<Course> Courses { get; set; } = default!;

        public virtual DbSet<Cafedra> Cafedras { get; set; } = default!;

        public virtual DbSet<Manager> Managers { get; set; } = default!;

        public virtual DbSet<Admin> Admins { get; set; } = default!;

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
     .Entity<CourseTeacher>()
     .HasMany(p => p.Lessons)
     .WithOne(sku => sku.CourseTeacher)
     .HasForeignKey(sku => sku.CourseTeacherId)
     .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(builder);
        }
    }
}