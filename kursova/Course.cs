using System.ComponentModel.DataAnnotations;

namespace ElectronicDepartment.DomainEntities
{
    public class Course : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(600)]
        public string Description { get; set; } = default!;

        public virtual List<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();
    }
}