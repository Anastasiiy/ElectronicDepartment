using ElectronicDepartment.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicDepartment.DomainEntities
{
    public class Lesson : BaseEntity
    {
        public int? CourseTeacherId { get; set; }

        [ForeignKey(nameof(CourseTeacherId))]
        public virtual CourseTeacher CourseTeacher { get; set; } = default!;

        public virtual List<StudentOnLesson> StudentOnLessons { get; set; } = new List<StudentOnLesson>();

        public LessonType LessonType { get; set; }

        public DateTime LessonStart { get; set; }

        public int Duration { get; set; }
    }
}