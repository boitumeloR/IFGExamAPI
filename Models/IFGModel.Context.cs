﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IFGExamAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IFGExamDBEntities : DbContext
    {
        public IFGExamDBEntities()
            : base("name=IFGExamDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Centre> Centres { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseGrade> CourseGrades { get; set; }
        public virtual DbSet<Learner> Learners { get; set; }
        public virtual DbSet<LearnerGrade> LearnerGrades { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<RegisteredCourse> RegisteredCourses { get; set; }
        public virtual DbSet<SchoolSubject> SchoolSubjects { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
