using StudentSystem.Date.Entites.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace StudentSystem.Data.Models
{
    public class HomeworkSubmissions
    {
        [Key]
        public int HomeworkId { get; set; }

        public string Content { get; set; } = null!;

        public ContentTypeEnum ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }

        public Students Student { get; set; } = null!;

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;
    }
}