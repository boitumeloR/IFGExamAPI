using IFGExamAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IFGExamAPI.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        // GET: Auth
        [Route("Login")]
        [HttpPost]
        public AuthVM Login(AuthVM vm)
        {
            return vm.Login();
        }
        [Route("Logout")]
        [HttpPost]

        public dynamic Logout(AuthVM vm)
        {
            return vm.Logout();
        }
    }
}