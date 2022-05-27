using System.ComponentModel.DataAnnotations;

namespace ElectronicDepartment.DomainEntities
{
    public class StudentOnLesson : BaseEntity
    {
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = default!;

        public string StudentId { get; set; }
        public virtual Student Student { get; set; } = default!;

        [Range(0, 100)]
        public int? Mark { get; set; } = null;
    }
}