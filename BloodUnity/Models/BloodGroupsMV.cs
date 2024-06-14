using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace BloodUnity.Models
{
    public class BloodGroupsMV
    {
        public int BloodGroupID { get; set; }
        [Required(ErrorMessage="Please fill this filed")]
        [Display(Name ="Blood Group")]
        public string BloodGroup { get; set; }
    }
}