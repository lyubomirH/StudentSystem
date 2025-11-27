using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentSystem.Data.Models;

namespace StudentSystem.Data.Models
{
    public class Students
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<Course> CourseEnrollments { get; set; } = new List<Course>();
        public ICollection<HomeworkSubmissions> HomeworkSubmissions { get; set; } = new List<HomeworkSubmissions>();
    }
}