using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class FinderController : Controller
    {
        // GET: Finder
        OnlineBloodBankDbEntities DB=new OnlineBloodBankDbEntities();
        public ActionResult FinderDonors()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup",0);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", 0);
            return View(new FinderMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinderDonors(FinderMV finderMV)
        {
            ///var list = new List<FinderSearchResultMV>();
            var setdate = DateTime.Now.AddDays(-120);
            var donors=DB.DonorTables.Where(d=>d.BloodGroupID==finderMV.BloodGroupID && d.LastDonationDate < setdate).ToList();
            foreach(var donor in donors)
            {
                var user=DB.UserTables.Find(donor.UserID);
                if (user.AccountStatusID == 2)
                {
                    var adddonor = new FinderSearchResultMV();
                    adddonor.BloodGroup = donor.BloodGroupsTable.BloodGroup;
                    adddonor.BloodGroupID = donor.BloodGroupID;
                    adddonor.ContactNo = donor.ContactNo;
                    adddonor.DonorID = donor.DonorID;
                    adddonor.FullName = donor.FullName;
                    adddonor.UserType = "Person";
                    adddonor.UserTypeID = user.UserTypeID;
                    finderMV.SearchResult.Add(adddonor);
                }
            }

            var bloodbanks = DB.BloodBankStockTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.Quantity > 0).ToList();
            foreach (var bloodbank in bloodbanks)
            {
                var getbloodbank = DB.BloodBankTables.Find(bloodbank.BloodBankID);
                var user = DB.UserTables.Find(getbloodbank.UserID);
                if (user.AccountStatusID == 2)
                {
                    var adddonor = new FinderSearchResultMV();
                    adddonor.BloodGroup = bloodbank.BloodGroupsTable.BloodGroup;
                    adddonor.BloodGroupID = bloodbank.BloodGroupID;
                    adddonor.ContactNo = bloodbank.BloodBankTable.PhoneNo;
                    adddonor.DonorID = bloodbank.BloodBankID;
                    adddonor.FullName = bloodbank.BloodBankTable.BloodBankName;
                    adddonor.UserType = "Blood Bank";
                    adddonor.UserTypeID = user.UserTypeID;
                    finderMV.SearchResult.Add(adddonor);
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", finderMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", finderMV.CityID);
            return View(finderMV);
        }

    }
}