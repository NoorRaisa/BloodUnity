using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BloodUnity.Models
{
    public class RequestTypeMV
    {
        public int RequestTypeID { get; set; }
        [Required(ErrorMessage="Please Fill This Field")]
        [Display(Name="Request Type")]

        public string RequestType { get; set; }
    }
}