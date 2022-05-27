using ElectronicDepartment.Common.Enums;

namespace ElectronicDepartment.DomainEntities
{
    public class Teacher : ApplicationUser
    {
        public AcademicAcredition AcademicAcredition { get; set; }

        public int? CafedraId { get; set; }

        public virtual Cafedra Cafedra { get; set; }

        public virtual List<CourseTeacher> Courses { get; set; } = new List<CourseTeacher>();
    }
}