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