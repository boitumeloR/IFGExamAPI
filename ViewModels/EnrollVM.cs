using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFGExamAPI.ViewModels
{
    public class EnrollVM
    {
        public List<int> Courses = new List<int>();
        public AuthVM Session { get;set; }
    }
}