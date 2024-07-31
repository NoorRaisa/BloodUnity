using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodUnity.Models
{
    public class RequestListMV
    {
        public int RequestID { get; set; }
        public string RequestDate { get; set; }
       
        public int AcceptedID { get; set; }
        public string AcceptedFullName { get; set; }
        public int AcceptedTypeID { get; set; }
        public string AcceptedType { get; set; }
        public int RequiredBloodGroupID { get; set; }
        public string BloodGroup { get; set; }
        public int RequestByID { get; set; }
        public string RequestBy { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestType { get; set; }
        public string RequestStatus { get; set; }
        public int RequestStatusID { get; set; }
       
        public DateTime ExpectedDate { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }

        public string RequestDetails { get; set; }
    }
}