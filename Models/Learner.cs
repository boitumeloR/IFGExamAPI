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
    
    public partial class Learner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Learner()
        {
            this.RegisteredCourses = new HashSet<RegisteredCourse>();
        }
    
        public int LearnerID { get; set; }
        public string LearnerSchool { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> LearnerGradeID { get; set; }
        public byte[] LearnerAgreementDoc { get; set; }
        public string LearnerAddress { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourse> RegisteredCourses { get; set; }
        public virtual LearnerGrade LearnerGrade { get; set; }
        public virtual UserLogin UserLogin { get; set; }
    }
}
