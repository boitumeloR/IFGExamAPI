using System;
using System.Collections.Generic;
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

        }
    }
}