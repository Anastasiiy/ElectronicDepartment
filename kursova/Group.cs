using System.ComponentModel.DataAnnotations;

namespace ElectronicDepartment.DomainEntities
{
    public class Group : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = default!;

        public virtual List<Student> Students { get; set; } = new List<Student>();
    }
}