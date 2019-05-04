using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectManagementSys.Models
{
    [MetadataType(typeof(userMetadata))]
    public partial class user
    {
    }
    public partial class userMetadata
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]

        public string LName { get; set; }
        [Display(Name = "Email")]

        public string Email { get; set; }
        [Display(Name = "Mobile Number")]

        public string mobile { get; set; }
        [Display(Name = "Password")]

        public string password { get; set; }
        [Display(Name = "Description")]

        public string description { get; set; }
        [Display(Name = "Type")]

        public int user_Type { get; set; }
        [Display(Name = "UserName")]

        public string username { get; set; }
        [Display(Name = "Photo")]

        public byte[] image { get; set; }
        public HttpPostedFileBase File { get; set; }

    }
}