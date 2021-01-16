using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IFGExamAPI.Models;
using IFGExamAPI.ViewModels;
using System.Data.Entity;
using System.Dynamic;

namespace IFGExamAPI.Controllers
{
    [RoutePrefix("api/Learner")]
    public class LearnerController : ApiController
    {
        IFGExamDBEntities db = new IFGExamDBEntities();
        // GET: Learner

        [Route("GetAllLearners")]
        [HttpPost]

        public dynamic GetAllLearners(AuthVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.RefreshSession();
            if (newSession.Error == null)
            {
                var learners = db.Learners.Include(zz => zz.UserLogin)
                    .Select(zz => new LearnerVM
                    {
                        LearnerID = (int)zz.LearnerID,
                        LearnerName = zz.UserLogin.Name,
                        LearnerSurname = zz.UserLogin.Surname,
                        EmailAddress = zz.UserLogin.EmailAddress,
                        IDNumber = zz.UserLogin.IDNumber
                    }).ToList();

                dynamic toReturn = new ExpandoObject();
                toReturn.Session = newSession;
                toReturn.Learners = learners;
                return toReturn;
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Learners = null;

                return toReturn;
            }
        }

        [Route("ApplyFilters")]
        [HttpPost]

        public dynamic ApplyFilters(LearnerVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();
            if (newSession.Error == null)
            {
                if (vm.CentreID != null && vm.CourseID != null)
                {
                    var learners = db.Learners.Include(zz => zz.UserLogin)
                        .Include(zz => zz.RegisteredCourses)
                        .Where(zz => zz.UserLogin.CentreID == vm.CentreID && zz.RegisteredCourses.Any(xx => xx.CourseID == vm.CourseID) == true)
                        .Select(zz => new LearnerVM
                        {
                            LearnerID = (int)zz.LearnerID,
                            LearnerName = zz.UserLogin.Name,
                            LearnerSurname = zz.UserLogin.Surname,
                            EmailAddress = zz.UserLogin.EmailAddress,
                            IDNumber = zz.UserLogin.IDNumber
                        }).ToList();

                    dynamic toReturn = new ExpandoObject();
                    toReturn.Session = newSession;
                    toReturn.Learners = learners;
                    return toReturn;
                }
                else if (vm.CourseID == null)
                {
                    var learners = db.Learners.Include(zz => zz.UserLogin)
                        .Include(zz => zz.RegisteredCourses)
                        .Where(zz => zz.UserLogin.CentreID == vm.CentreID)
                        .Select(zz => new LearnerVM
                        {
                            LearnerID = (int)zz.LearnerID,
                            LearnerName = zz.UserLogin.Name,
                            LearnerSurname = zz.UserLogin.Surname,
                            EmailAddress = zz.UserLogin.EmailAddress,
                            IDNumber = zz.UserLogin.IDNumber
                        }).ToList();

                    dynamic toReturn = new ExpandoObject();
                    toReturn.Session = newSession;
                    toReturn.Learners = learners;
                    return toReturn;
                }
                else if (vm.CentreID == null)
                {
                    var learners = db.Learners.Include(zz => zz.UserLogin)
                        .Include(zz => zz.RegisteredCourses)
                        .Where(zz => zz.RegisteredCourses.Any(xx => xx.CourseID == vm.CourseID) == true)
                        .Select(zz => new LearnerVM
                        {
                            LearnerID = (int)zz.LearnerID,
                            LearnerName = zz.UserLogin.Name,
                            LearnerSurname = zz.UserLogin.Surname,
                            EmailAddress = zz.UserLogin.EmailAddress,
                            IDNumber = zz.UserLogin.IDNumber
                        }).ToList();

                    dynamic toReturn = new ExpandoObject();
                    toReturn.Session = newSession;
                    toReturn.Learners = learners;
                    return toReturn;
                }
                else
                {
                    var learners = db.Learners.Include(zz => zz.UserLogin)
                        .Select(zz => new LearnerVM
                        {
                            LearnerID = (int)zz.LearnerID,
                            LearnerName = zz.UserLogin.Name,
                            LearnerSurname = zz.UserLogin.Surname,
                            EmailAddress = zz.UserLogin.EmailAddress,
                            IDNumber = zz.UserLogin.IDNumber
                        }).ToList();

                    dynamic toReturn = new ExpandoObject();
                    toReturn.Session = newSession;
                    toReturn.Learners = learners;
                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Learners = null;

                return toReturn;
            }
        }

        [Route("GetDeregistrations")]
        [HttpPost]

        public dynamic GetDeregistrations(AuthVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.RefreshSession();
            if (newSession.Error == null)
            {
                var learners = db.RegisteredCourses.Include(zz => zz.Learner)
                    .Include(zz => zz.Learner.UserLogin)
                    .Include(zz => zz.Course)
                    .Include(zz => zz.RegistrationStatu)
                    .Select(zz => new
                    {
                        LearnerID = (int)zz.LearnerID,
                        LearnerName = zz.Learner.UserLogin.Name,
                        LearnerSurname = zz.Learner.UserLogin.Surname,
                        CourseID = zz.Course.CourseID,
                        CourseName = zz.Course.CourseName,
                        Status = zz.RegistrationStatu.RegistrationStatusName,
                        DeregisterReason = zz.DeregisterReason
                    }).ToList();

                dynamic toReturn = new ExpandoObject();
                toReturn.Session = newSession;
                toReturn.Learners = learners;
                return toReturn;
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Session = newSession;
                toReturn.Learners = null;

                return toReturn;
            }
        }

        [Route("ApproveDeregistration")]
        [HttpPost]

        public dynamic ApproveDeregistration(LearnerVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.Session.RefreshSession();

            if (newSession.Error == null)
            {
                var enrollment = db.RegisteredCourses.Where(zz => zz.CourseID == (int)vm.CourseID && zz.LearnerID == vm.LearnerID).FirstOrDefault();

                if (enrollment != null)
                {
                    if (vm.IsDeregistered)
                    {
                        try
                        {
                            enrollment.RegistrationStatusID = 3;
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
                        try
                        {
                            enrollment.RegistrationStatusID = 1;
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
                }
                else
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Session = newSession;
                    toReturn.Success = true;
                    toReturn.Error = "Enrollment not found";

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

        [Route("GetLearnerCentres")]
        [HttpGet]
        public dynamic GetLearnerCentres()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Centres.Select(zz => new
            {
                CentreID = zz.CentreID,
                CentreName = zz.CentreName
            }).ToList();
        }

        [Route("GetLearnerCourses")]
        [HttpGet]
        public dynamic GetLearnerCourses()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Courses.Select(zz => new
            {
                CourseID = zz.CourseID,
                CourseName = zz.CourseName
            }).ToList();
        }
    }
}