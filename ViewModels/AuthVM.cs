using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFGExamAPI.ViewModels
{
    public class AuthVM
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserSecret { get; set; }
        public string SessionID { get; set; }
        public DateTime SessionExpiry { get; set; }
        public int UserRoleID { get; set; }
    }
}