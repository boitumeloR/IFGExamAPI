using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using IFGExamAPI.Models;
using IFGExamAPI.ViewModels;
using System.Dynamic;

namespace IFGExamAPI.Controllers
{
    [RoutePrefix("api/Reporting")]
    public class ReportingController : ApiController
    {
        IFGExamDBEntities db = new IFGExamDBEntities();
        // GET: Reporting
        [Route("PerformanceReport")]
        [HttpPost]

        public dynamic PerformanceReport(AuthVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var newSession = vm.RefreshSession();
            if (newSession.Error == null)
            {
                var chartData = db.RegisteredCourses.Include(zz => zz.Course)
                    .GroupBy(zz => new { zz.CourseID, zz.Course.CourseName })
                    .Select(zz => new
                    {
                        CourseName = zz.Key.CourseName,
                        MarkAverage = zz.Average(xx => xx.LearnerMark)
                    }).ToList();

                dynamic toReturn = new ExpandoObject();
                toReturn.Session = newSession;
                toReturn.ChartData = chartData;
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
    }
}