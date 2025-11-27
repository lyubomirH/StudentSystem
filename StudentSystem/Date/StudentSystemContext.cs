using Microsoft.EntityFrameworkCore;
using StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Date.Entites
{
    public class StudentSystemContext : DbContext
    {
        public DbSet<Students> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<HomeworkSubmissions> Homeworks { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GameChatNetwork;User Id=sa;Password=SuperStrongPass!23;TrustServerCertificate=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(s => s.StudentId);
                entity.Property(s => s.Name).HasMaxLength(100).IsUnicode(true);
                entity.Property(s => s.PhoneNumber).HasMaxLength(10).IsUnicode(false);
                entity.Property(s => s.RegisteredOn).IsRequired();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);
                entity.Property(c => c.Name).HasMaxLength(80).IsUnicode(true);
                entity.Property(c => c.Description).IsUnicode(true);
                entity.Property(c => c.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Resources>(entity =>
            {
                entity.HasKey(r => r.ResourcesId);
                entity.Property(r => r.Name).HasMaxLength(50).IsUnicode(true);
                entity.Property(r => r.Url).IsUnicode(false);
                entity.Property(r => r.ResourceType).IsRequired();

                entity.HasOne(r => r.Course)
                      .WithMany(c => c.Resources)
                      .HasForeignKey(r => r.CourseId);
            });

            modelBuilder.Entity<HomeworkSubmissions>(entity =>
            {
                entity.HasKey(h => h.HomeworkId);
                entity.Property(h => h.Content).IsUnicode(false);
                entity.Property(h => h.ContentType).IsRequired();

                entity.HasOne(h => h.Student)
                      .WithMany(s => s.HomeworkSubmissions)
                      .HasForeignKey(h => h.StudentId);

                entity.HasOne(h => h.Course)
                      .WithMany(c => c.Homeworks)
                      .HasForeignKey(h => h.CourseId);
            });

            modelBuilder.Entity<Course>()
                .HasMany(c => c.StudentsEnrolled)
                .WithMany(s => s.CourseEnrollments)
                .UsingEntity(j => j.ToTable("StudentCourses"));
        }
    }
}