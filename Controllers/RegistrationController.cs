using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IFGExamAPI.Models;
using IFGExamAPI.ViewModels;

namespace IFGExamAPI.Controllers
{
    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        IFGExamDBEntities db = new IFGExamDBEntities();
        // GET: Registration]

        [Route("RegisterAuth")]
        [HttpPost]

        public dynamic RegisterAuth(AuthVM vm)
        {
            db.Configuration.ProxyCreationEnabled = false;

            return vm.RegisterUser();
        }

        public dynamic RegisterLearner()
        {
            var form = HttpContext.Current.Request.Form;

            UserLogin user = FindUser(form.Get("EmailAddress"));
            if (user != null)
            {
                // Update user info from next step
                user.Name = form.Get("Name");
                user.Surname = form.Get("Surname");
                user.CentreID = Convert.ToInt32(form.Get("CentreID"));
                user.IDNumber = form.Get("IDNumber");

                db.SaveChanges();

                // Save learner specific data

                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var learner = new Learner
                {
                    UserID = user.UserID,
                    LearnerSchool = form.Get("LearnerSchool"),
                    LearnerGradeID = Convert.ToInt32(form.Get("LearnerGradeID")),
                    LearnerAddress = form.Get("LearnerAddress"),
                    LearnerAgreementDoc = new byte[file.ContentLength]
                };

                file.InputStream.Read(learner.LearnerAgreementDoc, 0, file.ContentLength);

                try
                {
                    db.Learners.Add(learner);
                    db.SaveChanges();

                    dynamic toReturn = new ExpandoObject();

                    toReturn.Success = true;
                    toReturn.Error = null;
                    return toReturn;
                }
                catch (Exception e)
                {
                    dynamic toReturn = new ExpandoObject();

                    toReturn.Success = false;
                    toReturn.Error = e.Message;
                    return toReturn;
                }
            }
            else
            {
                dynamic toReturn = new ExpandoObject();

                toReturn.Success = false;
                toReturn.Error = "An error occured with your email address.";

                return toReturn;
            }
        }

        private UserLogin FindUser(string email)
        {
            db.Configuration.ProxyCreationEnabled = false;

            return db.UserLogins.Where(zz => zz.EmailAddress == email).FirstOrDefault();
        }
    }
}