//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IFGExamAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLogin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserLogin()
        {
            this.Learners = new HashSet<Learner>();
        }
    
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string UserPassword { get; set; }
        public string UserSecret { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> SessionExpiry { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
        public Nullable<int> UserRoleID { get; set; }
        public Nullable<int> CentreID { get; set; }
    
        public virtual Centre Centre { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Learner> Learners { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
