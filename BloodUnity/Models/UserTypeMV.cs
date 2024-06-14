using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace BloodUnity.Models
{
    public class UserTypeMV
    {
        public int UserTypeID { get; set; }
        [Required(ErrorMessage="Please fill this field")]
        [Display(Name ="User Type")]
        public string UserType { get; set; }
    }
}