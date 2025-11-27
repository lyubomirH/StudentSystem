using StudentSystem.Date.Entites.Enum;
using System.ComponentModel.DataAnnotations;
using StudentSystem.Data.Models;

namespace StudentSystem.Data.Models
{
    public class Resources
    {
        [Key]
        public int ResourcesId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [Required]
        public ResourcesEnum ResourceType { get; set; }

        [Required]
        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;
    }
}