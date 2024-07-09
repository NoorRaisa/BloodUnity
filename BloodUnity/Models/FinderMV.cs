using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodUnity.Models
{
    public class FinderMV
    {
        public FinderMV() 
        {
            SearchResult = new List<FinderSearchResultMV>();
        }
        public int BloodGroupID {  get; set; }
        public int CityID { get; set; }
        public List<FinderSearchResultMV> SearchResult {  get; set; }
    }
}