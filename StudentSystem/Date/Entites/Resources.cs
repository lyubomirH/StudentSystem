using StudentSystem.Date.Entites.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Date.Entites
{
    public class Resources
    {
        [Key]
        public int ResourcesId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public ResourcesEnum ResourceType { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}
