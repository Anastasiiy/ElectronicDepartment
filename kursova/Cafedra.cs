using System.ComponentModel.DataAnnotations;

namespace ElectronicDepartment.DomainEntities
{
    public class Cafedra : BaseEntity
    {
        private string name;

        [Phone]
        [MaxLength(64)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    name = value;
                    NormalizedName = Name.ToUpperInvariant();
                }
            }
        }

        [Required]
        [MaxLength(64)]
        public string NormalizedName { get; set; }

        [Required]
        [MaxLength(64)]
        public string Description { get; set; }

        public virtual List<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}