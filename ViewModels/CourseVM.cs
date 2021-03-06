﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFGExamAPI.ViewModels
{
    public class CourseVM
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public DateTime DateRegistered { get; set; }
        public int CourseGradeID { get; set; }
        public int CourseCentreID { get; set; }
        public int CourseGradeLevel { get; set; }
        public double? CourseMark { get; set; }
        public string CourseSubject { get; set; }
        public string CourseComments { get; set; }
        public int LessonFrequency { get; set; }
        public int CourseSubjectID { get; set; }
        public int RegistrationStatusID { get; set; }
        public string RegistrationStatusName { get; set; }
        public string DeregisterReason { get; set; }
        public bool IsApproved { get; set; }
        public AuthVM Session { get; set; }
        
    }
}