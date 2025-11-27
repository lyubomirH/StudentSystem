using Humanizer.Localisation;
using StudentSystem.Date.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Data.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [MaxLength(80)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<Resources> Resources { get; set; } = new HashSet<Resources>();

        public ICollection<HomeworkSubmissions> Homeworks { get; set; } = new HashSet<HomeworkSubmissions>();

        public ICollection<Students> StudentsEnrolled { get; set; } = new HashSet<Students>();
    }
}