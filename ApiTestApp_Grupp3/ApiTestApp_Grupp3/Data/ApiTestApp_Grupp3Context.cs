using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiTestApp_Grupp3.Models;

namespace ApiTestApp_Grupp3.Data
{
    public class ApiTestApp_Grupp3Context : DbContext
    {
        public ApiTestApp_Grupp3Context (DbContextOptions<ApiTestApp_Grupp3Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestResult>()
                .HasKey(tr => new { tr.StudentId, tr.TestId });

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.Student)
                .WithMany(t => t.TestResult)
                .HasForeignKey(tr => tr.StudentId);

            modelBuilder.Entity<TestResult>()
               .HasOne(tr => tr.Test)
               .WithMany(t => t.TestResult)
               .HasForeignKey(tr => tr.TestId);

            modelBuilder.Entity<StudentQuestionAnswer>()
                .HasKey(sqa => new { sqa.StudentId, sqa.TestId, sqa.QuestionId });

            modelBuilder.Entity<StudentQuestionAnswer>()
                .HasOne(sqa => sqa.Student)
                .WithMany(sq => sq.StudentQuestionAnswer)
                .HasForeignKey(sqa => sqa.StudentId);

            modelBuilder.Entity<StudentQuestionAnswer>()
                .HasOne(sqa => sqa.Test)
                .WithMany(sq => sq.StudentQuestionAnswer)
                .HasForeignKey(sqa => sqa.TestId);

            modelBuilder.Entity<StudentQuestionAnswer>()
                .HasOne(sqa => sqa.Question)
                .WithMany(sq => sq.StudentQuestionAnswer)
                .HasForeignKey(sqa => sqa.QuestionId);

            modelBuilder.Entity<TestQuestion>().HasKey(tq => new { tq.TestId, tq.QuestionId });

            modelBuilder.Entity<TestQuestion>()
               .HasOne(tq => tq.Question)
               .WithMany(q => q.TestQuestion)
               .HasForeignKey(tq => tq.QuestionId);

            modelBuilder.Entity<TestQuestion>()
                .HasOne(tq => tq.Test)
                .WithMany(t => t.TestQuestion)
                .HasForeignKey(tq => tq.TestId);


            modelBuilder.Entity<EmployeeRole>().HasKey(er => new { er.EmployeeId, er.RoleId });

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRole)
                .HasForeignKey(er => er.EmployeeId);

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(e => e.EmployeeRole)
                .HasForeignKey(er => er.RoleId);

        }

        public DbSet<ApiTestApp_Grupp3.Models.Test> Test { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Employee> Employee { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Student> Student { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Question> Question { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.TestResult> TestResult { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.StudentQuestionAnswer> StudentQuestionAnswer { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Course> Course { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.TestQuestion> TestQuestion { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Role> Role { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.EmployeeRole> EmployeeRole { get; set; }
    }
}
