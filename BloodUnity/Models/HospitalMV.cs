using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodUnity.Models
{
    public class HospitalMV
    {
        public int HospitalID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public int CityID { get; set; }
        public string City {  get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}