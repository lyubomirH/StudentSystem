using Microsoft.EntityFrameworkCore;
using StudentSystem.Data.Models;
using StudentSystem.Date.Entites;
using StudentSystem.Date.Entites.Enum;
using System;
using System.Linq;

namespace StudentSystem.Configration
{
    public static class SeedData
    {
        public static void Initialize(StudentSystemContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Students.Any() || context.Courses.Any())
            {
                return; // Database has been seeded
            }

            // Seed Students
            var students = new Students[]
            {
                new Students { Name = "John Doe", PhoneNumber = "1234567890", RegisteredOn = DateTime.Now.AddDays(-30), Birthday = new DateTime(2000, 1, 15) },
                new Students { Name = "Jane Smith", PhoneNumber = "0987654321", RegisteredOn = DateTime.Now.AddDays(-25), Birthday = new DateTime(1999, 5, 20) },
                new Students { Name = "Bob Johnson", PhoneNumber = "5551234567", RegisteredOn = DateTime.Now.AddDays(-15), Birthday = new DateTime(2001, 8, 10) }
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            // Seed Courses
            var courses = new Course[]
            {
                new Course {
                    Name = "Introduction to Programming",
                    Description = "Learn basic programming concepts",
                    StartDate = DateTime.Now.AddDays(-20),
                    EndDate = DateTime.Now.AddDays(40),
                    Price = 299.99m
                },
                new Course {
                    Name = "Web Development",
                    Description = "Build modern web applications",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(50),
                    Price = 399.99m
                },
                new Course {
                    Name = "Database Design",
                    Description = "Design and implement databases",
                    StartDate = DateTime.Now.AddDays(-5),
                    EndDate = DateTime.Now.AddDays(55),
                    Price = 349.99m
                }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();

            // Seed Resources
            var resources = new Resources[]
            {
                new Resources {
                    Name = "Programming Basics PDF",
                    Url = "https://example.com/programming-basics.pdf",
                    ResourceType = ResourcesEnum.Document,
                    CourseId = courses[0].CourseId
                },
                new Resources {
                    Name = "HTML/CSS Video Tutorial",
                    Url = "https://example.com/html-css-video",
                    ResourceType = ResourcesEnum.Video,
                    CourseId = courses[1].CourseId
                },
                new Resources {
                    Name = "SQL Presentation",
                    Url = "https://example.com/sql-presentation.pptx",
                    ResourceType = ResourcesEnum.Presentation,
                    CourseId = courses[2].CourseId
                }
            };

            context.Resources.AddRange(resources);
            context.SaveChanges();

            // Seed Homework Submissions
            var homeworks = new HomeworkSubmissions[]
            {
                new HomeworkSubmissions {
                    Content = "https://github.com/johndoe/programming-assignment",
                    ContentType = ContentTypeEnum.Done,
                    SubmissionTime = DateTime.Now.AddDays(-5),
                    StudentId = students[0].StudentId,
                    CourseId = courses[0].CourseId
                },
                new HomeworkSubmissions {
                    Content = "https://github.com/janesmith/web-project",
                    ContentType = ContentTypeEnum.InProgress,
                    SubmissionTime = DateTime.Now.AddDays(-2),
                    StudentId = students[1].StudentId,
                    CourseId = courses[1].CourseId
                }
            };

            context.Homeworks.AddRange(homeworks);
            context.SaveChanges();

            // Enroll students in courses (many-to-many)
            students[0].CourseEnrollments.Add(courses[0]);
            students[0].CourseEnrollments.Add(courses[1]);
            students[1].CourseEnrollments.Add(courses[1]);
            students[2].CourseEnrollments.Add(courses[0]);
            students[2].CourseEnrollments.Add(courses[2]);

            context.SaveChanges();
        }
    }
}