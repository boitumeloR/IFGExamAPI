using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFGExamAPI.ViewModels
{
    public class LearnerVM
    {
        public string LearnerSchool { get; set; }
        public string LearnerName { get; set; }
        public string LearnerSurname { get; set; }
        public string EmailAddress { get; set; }
        public string IDNumber { get; set; }
        public int LearnerID { get; set; }
        public int? CourseID { get; set; }
        public int? CentreID { get; set; }
        public string LearnerGradeID { get; set; }
        public string LearnerAddress { get; set; }
        public string UserEmail { get; set; }
        public AuthVM Session { get; set; }
    }
}