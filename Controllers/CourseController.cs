using IFGExamAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using IFGExamAPI.Models;
using System.Data.Entity;
using System.Web.Http;

namespace IFGExamAPI.Controllers
{
    [RoutePrefix("api/Course")]
    public class CourseController : ApiController
    {
        IFGExamDBEntities db = new IFGExamDBEntities();
        // GET: Course
        [Route("GetLearnerCourses")]
        [HttpPost]
        public dynamic GetLearnerCourses(AuthVM vm)
        {
            var newSession = vm.RefreshSession();

            if (newSession.Error == null)
            {
                var courses = db.RegisteredCourses.Include(zz => zz.Learner)
                    .Include(zz => zz.Learner.UserLogin)
                    .Include(zz => zz.Course)
                    .Include(zz => zz.RegistrationStatu)
                    .Include(zz => zz.Course.SchoolSubject)
                    .Include(zz => zz.Course.CourseGrade)
                    .Where(zz => zz.Learner.UserLogin.EmailAddress == newSession.EmailAddress)
                    .Select(zz => new CourseVM
                    {
                        CourseID =  (int)zz.CourseID,
                        CourseName = zz.Course.CourseName,
                        CourseGradeID = (int)zz.Course.CourseGradeID,
                        CourseGradeLevel = (int) zz.Course.CourseGrade.CourseGradeLevel,
                        CourseSubject = zz.Course.SchoolSubject.SubjectName,
                        DateRegistered = zz.RegisterDate,
                        IsRegistered = zz.IsRegistered
                    })
                    .ToList();

                dynamic toReturn = new ExpandoObject();
                toReturn.Session = newSession;
                toReturn.Courses = courses;
                return toReturn;
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Courses = null;

                return toReturn;
            }
        }
    }
}