using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using IFGExamAPI.Models;

namespace IFGExamAPI.ViewModels
{
    public class AuthVM
    {
        IFGExamDBEntities db = new IFGExamDBEntities();
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserSecret { get; set; }
        public string SessionID { get; set; }
        public DateTime SessionExpiry { get; set; }
        public int UserRoleID { get; set; }
        public int CentreID { get; set; }
        public string Error { get; set; }

        public dynamic RegisterUser()
        {
            db.Configuration.ProxyCreationEnabled = false;
            
            if (UserExists() == false)
            {
                // Register user
                UserLogin user = new UserLogin
                {
                    EmailAddress = this.EmailAddress,
                    UserPassword = ComputeSha256Hash(this.Password),
                    UserRoleID = 2
                };

                try
                {
                    db.UserLogins.Add(user);
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
                // Exists

                dynamic toReturn = new ExpandoObject();

                toReturn.Success = false;
                toReturn.Error = "A user with that email address already exists.";

                return toReturn;
            }
        }

        public AuthVM Login()
        {
            db.Configuration.ProxyCreationEnabled = false;

            string password = ComputeSha256Hash(this.Password);
            var user = db.UserLogins.Where(zz => zz.EmailAddress == this.EmailAddress && zz.UserPassword == password).FirstOrDefault();

            if (user!= null)
            {
                user.UserSecret = Guid.NewGuid().ToString();
                user.SessionID = Guid.NewGuid().ToString();
                user.SessionExpiry = DateTime.Now.AddMinutes(30);

                var toReturn = new AuthVM
                {
                    CentreID = (int)user.CentreID,
                    UserSecret = user.UserSecret,
                    SessionExpiry = (DateTime)user.SessionExpiry,
                    SessionID = user.SessionID,
                    Error = null,
                    UserRoleID = (int)user.UserRoleID,
                    EmailAddress = user.EmailAddress
                };

                return toReturn;
            }
            else
            {
                var toReturn = new AuthVM
                {
                    Error = "A user with that email address and password does not exist."
                };

                return toReturn;
            }
        }

        public dynamic Logout()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var user = db.UserLogins.Where(zz => zz.EmailAddress == this.EmailAddress).FirstOrDefault();

            if (user != null)
            {
                user.SessionExpiry = DateTime.Now;

                db.SaveChanges();

                dynamic toReturn = new ExpandoObject();
                toReturn.Success = true;
                toReturn.Error = null;

                return toReturn;
            }
            else
            {
                dynamic toReturn = new ExpandoObject();
                toReturn.Success = false;
                toReturn.Error = "User not found";

                return toReturn;
            }
        }

        public AuthVM RefreshSession()
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var refresh = db.UserLogins.Where(zz => zz.SessionID == this.SessionID && zz.UserSecret == this.UserSecret).FirstOrDefault();

                if (refresh == null)
                {
                    AuthVM errorReturn = new AuthVM
                    {
                        Error = "User not found",
                    };
                    return errorReturn;
                }

                if (refresh.SessionExpiry < DateTime.Now)
                {
                    AuthVM errorReturn = new AuthVM
                    {
                        Error = "Session has expired! Login again",
                    };
                    return errorReturn;
                }

                refresh.SessionID = Guid.NewGuid().ToString();
                refresh.SessionExpiry = DateTime.Now.AddMinutes(30);

                db.SaveChanges();

                AuthVM toReturn = new AuthVM
                {
                    SessionID = refresh.SessionID,
                    UserSecret = refresh.UserSecret,
                    Error = null,
                    EmailAddress = refresh.EmailAddress,
                    Password = null,
                    SessionExpiry = (DateTime)refresh.SessionExpiry,
                    UserRoleID = (int)refresh.UserRoleID
                };
                return toReturn;
            }
            catch (Exception)
            {
                AuthVM errorReturn = new AuthVM
                {
                    Error = "Oops! An error occured. Login again"
                };
                return errorReturn;
            }
        }

        private bool UserExists()
        {
            db.Configuration.ProxyCreationEnabled = false;

            return db.UserLogins.Any(zz => zz.EmailAddress == this.EmailAddress);
        }

        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}