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
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.RefreshSession();
            var year = DateTime.Now.Year;
            if (newSession.Error == null)
            {
                var courses = db.RegisteredCourses.Include(zz => zz.Learner)
                    .Include(zz => zz.Learner.UserLogin)
                    .Include(zz => zz.Course)
                    .Include(zz => zz.RegistrationStatu)
                    .Include(zz => zz.Course.SchoolSubject)
                    .Include(zz => zz.Course.CourseGrade)
                    .Where(zz => zz.Learner.UserLogin.EmailAddress == newSession.EmailAddress && zz.RegisterDate.Year == year)
                    .Select(zz => new CourseVM
                    {
                        CourseID =  (int)zz.CourseID,
                        CourseName = zz.Course.CourseName,
                        CourseGradeID = (int)zz.Course.CourseGradeID,
                        CourseGradeLevel = (int) zz.Course.CourseGrade.CourseGradeLevel,
                        CourseSubject = zz.Course.SchoolSubject.SubjectName,
                        CourseMark  = zz.LearnerMark,
                        DateRegistered = zz.RegisterDate,
                        RegistrationStatusID = (int)zz.RegistrationStatusID,
                        RegistrationStatusName = zz.RegistrationStatu.RegistrationStatusName
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

        [Route("AvailableCourses")]
        [HttpPost]
        public dynamic AvailableCourses(AuthVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.RefreshSession();

            if (newSession.Error == null)
            {
                var learner = db.Learners.Include(zz => zz.UserLogin)
                    .Include(zz => zz.LearnerGrade)
                    .Where(zz => zz.UserLogin.EmailAddress == newSession.EmailAddress)
                    .FirstOrDefault();
                if (learner.LearnerGrade.LearnerGradeLevel >= 8)
                {
                    var courses = db.Courses.Include(zz => zz.CourseCentre)
                        .Include(zz => zz.SchoolSubject)
                        .Where(zz => zz.CourseCentreID == newSession.CentreID)
                        .Where(zz => zz.CourseGradeID == learner.LearnerGradeID)
                        .Where(zz => zz.RegisteredCourses.Where(xx => xx.CourseID == zz.CourseID).Count() < 35)
                        .Select(zz => new
                        {
                            CourseID = zz.CourseID,
                            CourseSubject = zz.SchoolSubject.SubjectName,
                            CourseName = zz.CourseName,
                            CourseDescription = zz.CourseDescription,
                            CourseStatus = false
                        }).ToList();

                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Courses = courses;
                    toReturn.MaxCourses = 3;

                    return toReturn;
                }
                else
                {
                    var courses = db.Courses.Include(zz => zz.CourseCentre)
                        .Include(zz => zz.SchoolSubject)
                        .Where(zz => zz.CourseCentreID == newSession.CentreID)
                        .Where(zz => zz.CourseGradeID == learner.LearnerGradeID)
                        .Where(zz => zz.RegisteredCourses.Where(xx => xx.CourseID == zz.CourseID).Count() < 35)
                        .Where(zz => zz.SubjectID == 1 || zz.SubjectID == 2)
                        .Select(zz => new
                        {
                            CourseID = zz.CourseID,
                            CourseSubject = zz.SchoolSubject.SubjectName,
                            CourseName = zz.CourseName,
                            CourseDescription = zz.CourseDescription,
                            CourseStatus = false
                        }).ToList();

                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Courses = courses;
                    toReturn.MaxCourses = 2;

                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Courses = null;

                return toReturn;
            }
        }

        [Route("RegisterCourses")]
        [HttpPost]

        public dynamic RegisterCourses(EnrollVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var learner = db.Learners.Include(zz => zz.UserLogin)
                    .Where(zz => zz.UserLogin.EmailAddress == newSession.EmailAddress)
                    .FirstOrDefault();

                foreach (var id in vm.Courses)
                {
                    var enroll = new RegisteredCourse
                    {
                        CourseID = id,
                        LearnerID = learner.LearnerID,
                        RegisterDate = DateTime.Now,
                        RegistrationStatusID = 1
                    };

                    try
                    {
                        db.RegisteredCourses.Add(enroll);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        dynamic error = new ExpandoObject();

                        error.Session = newSession;
                        error.Success = false;
                        error.Error = e.Message;

                        return error;
                    }
                }

                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = true;
                toReturn.Error = null;

                return toReturn;
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = false;

                return toReturn;
            }
        }

        [Route("DeregisterCourse")]
        [HttpPost]

        public dynamic DeregisterCourse(CourseVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var learner = db.Learners.Include(zz => zz.UserLogin).Where(zz => zz.UserLogin.EmailAddress == newSession.EmailAddress).FirstOrDefault();
                var year = DateTime.Now.Year;
                var dereg = db.RegisteredCourses.Where(zz => zz.CourseID == vm.CourseID && zz.LearnerID == learner.LearnerID && zz.RegisterDate.Year == year).FirstOrDefault();
                if (dereg != null)
                {
                    dereg.RegistrationStatusID = 2;
                    dereg.DeregisterReason = vm.DeregisterReason;

                    try
                    {
                        db.SaveChanges();

                        dynamic toReturn = new ExpandoObject();
                        toReturn.Session = newSession;
                        toReturn.Success = true;
                        toReturn.Error = null;

                        return toReturn;
                    }
                    catch (Exception e)
                    {
                        dynamic error = new ExpandoObject();
                        error.Session = newSession;
                        error.Success = false;
                        error.Error = e.Message;

                        return error;
                    }
                }
                else
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = false;
                    toReturn.Error = "Unknown error occured.";

                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = false;

                return toReturn;
            }
        }

        [Route("ApproveDeregistration")]
        [HttpPost]

        public dynamic ApproveDeregistration(CourseVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var year = DateTime.Now.Year;
                var dereg = db.RegisteredCourses.Where(zz => zz.CourseID == vm.CourseID).FirstOrDefault();
                if (dereg != null)
                {
                    if (vm.IsApproved == true)
                    {
                        dereg.RegistrationStatusID = 3;

                        try
                        {
                            db.SaveChanges();

                            dynamic toReturn = new ExpandoObject();
                            toReturn.Session = newSession;
                            toReturn.Success = true;
                            toReturn.Error = null;

                            return toReturn;
                        }
                        catch (Exception e)
                        {
                            dynamic error = new ExpandoObject();
                            error.Session = newSession;
                            error.Success = false;
                            error.Error = e.Message;

                            return error;
                        }
                    }
                    else
                    {
                        dereg.RegistrationStatusID = 1;

                        try
                        {
                            db.SaveChanges();

                            dynamic toReturn = new ExpandoObject();
                            toReturn.Session = newSession;
                            toReturn.Success = true;
                            toReturn.Error = null;

                            return toReturn;
                        }
                        catch (Exception e)
                        {
                            dynamic error = new ExpandoObject();
                            error.Session = newSession;
                            error.Success = false;
                            error.Error = e.Message;

                            return error;
                        }
                    }
                }
                else
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = false;
                    toReturn.Error = "Unknown error occured.";

                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = false;

                return toReturn;
            }
        }

        [Route("AdminCourses")]
        [HttpPost]

        public dynamic AdminCourses(AuthVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.RefreshSession();

            if (newSession.Error == null)
            {
                var courses = db.Courses.Include(zz => zz.SchoolSubject).Select(zz => new CourseVM
                {
                    CourseID = zz.CourseID,
                    CourseName = zz.CourseName,
                    CourseSubjectID = (int)zz.SubjectID,
                    CourseCentreID = (int)zz.CourseCentreID,
                    CourseGradeID = (int)zz.CourseGradeID,
                    CourseSubject = zz.SchoolSubject.SubjectName,
                    CourseDescription = zz.CourseDescription
                }).ToList();

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

        [Route("AddCourse")]
        [HttpPost]

        public dynamic AddCourse(CourseVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var course = new Course
                {
                    CourseName = vm.CourseName,
                    CourseDescription = vm.CourseDescription,
                    SubjectID = vm.CourseSubjectID,
                    CourseGradeID = vm.CourseGradeID,
                    CourseCentreID = vm.CourseCentreID
                };

                try
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = true;

                    return toReturn;

                }
                catch (Exception)
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = true;
                    toReturn.Error = "An unkown error occured.";

                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = false;

                return toReturn;
            }
        }

        [Route("UpdateCourse")]
        [HttpPost]

        public dynamic UpdateCourse(CourseVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var course = db.Courses.Where(zz => zz.CourseID == vm.CourseID).FirstOrDefault();

                if (course != null)
                {
                    course.CourseName = vm.CourseName;
                    course.CourseDescription = vm.CourseDescription;
                    course.SubjectID = vm.CourseSubjectID;
                    course.CourseGradeID = vm.CourseGradeID;
                    course.CourseCentreID = vm.CourseCentreID;

                    try
                    {
                        db.SaveChanges();
                        dynamic toReturn = new ExpandoObject();

                        toReturn.Session = newSession;
                        toReturn.Success = true;

                        return toReturn;

                    }
                    catch (Exception)
                    {
                        dynamic toReturn = new ExpandoObject();

                        toReturn.Session = newSession;
                        toReturn.Success = true;
                        toReturn.Error = "An unkown error occured.";

                        return toReturn;
                    }
                }
                else
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = true;
                    toReturn.Error = "Course not found";

                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = false;

                return toReturn;
            }
        }

        [Route("DeleteCourse")]
        [HttpPost]

        public dynamic DeleteCourse(CourseVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var course = db.Courses.Where(zz => zz.CourseID == vm.CourseID).FirstOrDefault();

                if (course != null)
                {
                    try
                    {
                        db.Courses.Remove(course);
                        db.SaveChanges();
                        dynamic toReturn = new ExpandoObject();

                        toReturn.Session = newSession;
                        toReturn.Success = true;

                        return toReturn;

                    }
                    catch (Exception)
                    {
                        dynamic toReturn = new ExpandoObject();

                        toReturn.Session = newSession;
                        toReturn.Success = false;
                        toReturn.Error = "An unkown error occured.";

                        return toReturn;
                    }
                }
                else
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = false;
                    toReturn.Error = "Course not found";

                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Success = false;

                return toReturn;
            }
        }

        [Route("CheckModificationStatus")]
        [HttpPost]

        public dynamic CheckModificationStatus(CourseVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var courseExists = db.RegisteredCourses.Any(zz => zz.CourseID == vm.CourseID);
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                if (courseExists == false)
                {
                    toReturn.CanModify = true;
                }
                else
                {
                    toReturn.CanModify = false;
                }

                return toReturn;
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.CanModify = false;

                return toReturn;
            }
        }

        [Route("GetCourseGrades")]
        [HttpGet]
        public dynamic GetCourseGrades()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.CourseGrades.Select(zz => new
            {
                CourseGradeID = zz.CourseGradeID,
                CourseGradeLevel = zz.CourseGradeLevel
            }).ToList();
        }

        [Route("GetCourseCentres")]
        [HttpGet]
        public dynamic GetCourseCentres()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.CourseCentres.Select(zz => new
            {
                CourseCentreID = zz.CourseCentreID,
                CourseCentreName = zz.CourseCentreName
            }).ToList();
        }

        [Route("GetCourseSubjects")]
        [HttpGet]
        public dynamic GetCourseSubjects()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.SchoolSubjects.Select(zz => new
            {
                CourseSubjectID = zz.SubjectID,
                CourseSubjectName = zz.SubjectName
            }).ToList();
        }
    }
}