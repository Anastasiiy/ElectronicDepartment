namespace ElectronicDepartment.DomainEntities
{
    public class Student : ApplicationUser
    {
        public int GroupId { get; set; }

        public virtual Group Group { get; set; } = default!;

        public virtual List<StudentOnLesson> StudentOnLessons { get; set; } = new List<StudentOnLesson>();
    }
}