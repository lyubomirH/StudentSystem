using StudentSystem.Configration;
using StudentSystem.Date.Entites;

namespace StudentSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new StudentSystemContext();

            // Seed the database
            SeedData.Initialize(context);

            Console.WriteLine("Database seeded successfully!");
            Console.WriteLine("=== LINQ QUERIES RESULTS ===\n");

            // Query 1: Get all students with their enrolled courses
            // Kiril
            Query1_StudentsWithCourses(context);

            // Query 2: Get courses with their resources
            //Martin
            Query2_CoursesWithResources(context);

            // Query 3: Get students with submitted homework
            //Stefan
            Query3_StudentsWithHomework(context);

            // Query 4: Get courses with student count and total revenue
            //Atanas
            Query4_CoursesWithStatistics(context);

            // Query 5: Get recent activities (homework submissions in last 7 days)
            //Atanas
            Query5_RecentActivities(context);
        }

        // Query 1: Get all students with their enrolled courses
        static void Query1_StudentsWithCourses(StudentSystemContext context)
        {
            Console.WriteLine("1. STUDENTS WITH THEIR ENROLLED COURSES:");
            Console.WriteLine("=========================================");

            var studentsWithCourses = context.Students
                .Select(s => new
                {
                    StudentName = s.Name,
                    EnrollmentCount = s.CourseEnrollments.Count,
                    Courses = s.CourseEnrollments.Select(c => c.Name).ToList(),
                    RegisteredDate = s.RegisteredOn.ToString("yyyy-MM-dd")
                })
                .ToList();

            foreach (var student in studentsWithCourses)
            {
                Console.WriteLine($"- {student.StudentName} (Registered: {student.RegisteredDate})");
                Console.WriteLine($"  Enrolled in {student.EnrollmentCount} course(s): {string.Join(", ", student.Courses)}");
                Console.WriteLine();
            }
        }

        // Query 2: Get courses with their resources
        static void Query2_CoursesWithResources(StudentSystemContext context)
        {
            Console.WriteLine("2. COURSES WITH THEIR RESOURCES:");
            Console.WriteLine("=================================");

            var coursesWithResources = context.Courses
                .Select(c => new
                {
                    CourseName = c.Name,
                    CoursePrice = c.Price,
                    ResourceCount = c.Resources.Count,
                    Resources = c.Resources.Select(r => new
                    {
                        r.Name,
                        r.ResourceType,
                        r.Url
                    }).ToList()
                })
                .ToList();

            foreach (var course in coursesWithResources)
            {
                Console.WriteLine($"- {course.CourseName} (${course.CoursePrice})");
                Console.WriteLine($"  Resources: {course.ResourceCount}");
                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"    • {resource.Name} ({resource.ResourceType}) - {resource.Url}");
                }
                Console.WriteLine();
            }
        }

        // Query 3: Get students with submitted homework and their status
        static void Query3_StudentsWithHomework(StudentSystemContext context)
        {
            Console.WriteLine("3. STUDENTS WITH HOMEWORK SUBMISSIONS:");
            Console.WriteLine("======================================");

            var studentsWithHomework = context.Students
                .Where(s => s.HomeworkSubmissions.Any())
                .Select(s => new
                {
                    StudentName = s.Name,
                    HomeworkCount = s.HomeworkSubmissions.Count,
                    Submissions = s.HomeworkSubmissions.Select(h => new
                    {
                        h.Content,
                        h.ContentType,
                        h.SubmissionTime,
                        CourseName = h.Course.Name
                    }).OrderByDescending(h => h.SubmissionTime).ToList()
                })
                .ToList();

            foreach (var student in studentsWithHomework)
            {
                Console.WriteLine($"- {student.StudentName} ({student.HomeworkCount} submissions):");
                foreach (var submission in student.Submissions)
                {
                    Console.WriteLine($"    • {submission.CourseName}: {submission.Content}");
                    Console.WriteLine($"      Status: {submission.ContentType}, Submitted: {submission.SubmissionTime:yyyy-MM-dd HH:mm}");
                }
                Console.WriteLine();
            }
        }

        // Query 4: Get courses with student count and total revenue
        static void Query4_CoursesWithStatistics(StudentSystemContext context)
        {
            Console.WriteLine("4. COURSE STATISTICS:");
            Console.WriteLine("=====================");

            var courseStatistics = context.Courses
                .Select(c => new
                {
                    CourseName = c.Name,
                    StudentCount = c.StudentsEnrolled.Count,
                    TotalRevenue = c.Price * c.StudentsEnrolled.Count,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd"),
                    DurationDays = (c.EndDate - c.StartDate).Days
                })
                .OrderByDescending(c => c.TotalRevenue)
                .ToList();

            foreach (var course in courseStatistics)
            {
                Console.WriteLine($"- {course.CourseName}");
                Console.WriteLine($"  Students: {course.StudentCount}");
                Console.WriteLine($"  Revenue: ${course.TotalRevenue:F2}");
                Console.WriteLine($"  Duration: {course.DurationDays} days ({course.StartDate} to {course.EndDate})");
                Console.WriteLine();
            }
        }

        // Query 5: Get recent activities (homework submissions in last 7 days)
        static void Query5_RecentActivities(StudentSystemContext context)
        {
            Console.WriteLine("5. RECENT ACTIVITIES (Last 7 Days):");
            Console.WriteLine("===================================");

            var sevenDaysAgo = DateTime.Now.AddDays(-7);

            var recentActivities = context.Homeworks
                .Where(h => h.SubmissionTime >= sevenDaysAgo)
                .OrderByDescending(h => h.SubmissionTime)
                .Select(h => new
                {
                    StudentName = h.Student.Name,
                    CourseName = h.Course.Name,
                    h.Content,
                    h.ContentType,
                    SubmissionTime = h.SubmissionTime,
                    DaysAgo = (DateTime.Now - h.SubmissionTime).Days
                })
                .ToList();

            if (recentActivities.Any())
            {
                foreach (var activity in recentActivities)
                {
                    Console.WriteLine($"- {activity.StudentName} submitted homework for {activity.CourseName}");
                    Console.WriteLine($"  Content: {activity.Content}");
                    Console.WriteLine($"  Status: {activity.ContentType}, {activity.DaysAgo} day(s) ago");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No recent activities found in the last 7 days.");
                Console.WriteLine();
            }

            Console.WriteLine("SUMMARY STATISTICS:");
            Console.WriteLine("===================");
            Console.WriteLine($"Total Students: {context.Students.Count()}");
            Console.WriteLine($"Total Courses: {context.Courses.Count()}");
            Console.WriteLine($"Total Resources: {context.Resources.Count()}");
            Console.WriteLine($"Total Homework Submissions: {context.Homeworks.Count()}");

            var mostPopularCourse = context.Courses
                .OrderByDescending(c => c.StudentsEnrolled.Count)
                .FirstOrDefault();

            if (mostPopularCourse != null)
            {
                Console.WriteLine($"Most Popular Course: {mostPopularCourse.Name} ({mostPopularCourse.StudentsEnrolled.Count} students)");
            }
        }
    }
}
