//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NicePose.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NiceposeUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NiceposeUser()
        {
            this.UserPictures = new HashSet<UserPicture>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public System.DateTime createDate { get; set; }
        public bool isActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserPicture> UserPictures { get; set; }
    }
}
