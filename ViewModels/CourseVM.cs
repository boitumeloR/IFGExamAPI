using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFGExamAPI.ViewModels
{
    public class CourseVM
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool IsRegistered { get; set; }
        public int CourseGradeID { get; set; }
        public int CourseGradeLevel { get; set; }
        public string CourseSubject { get; set; }
        public int RegistrationStatusID { get; set; }
        public string RegistrationStatusName { get; set; }
        
    }
}