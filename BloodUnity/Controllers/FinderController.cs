using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class FinderController : Controller
    {
        // GET: Finder
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult FinderDonors()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", 0);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", 0);
            return View(new FinderMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinderDonors(FinderMV finderMV)
        {
            ///var list = new List<FinderSearchResultMV>();
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]),out userid);
            var setdate = DateTime.Now.AddDays(-120);
            var donors = DB.DonorTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.CityID == finderMV.CityID && d.LastDonationDate < setdate).ToList();
            foreach (var donor in donors)
            {
                var user = DB.UserTables.Find(donor.UserID);
                if (userid != user.UserID)
                {
                    if (user.AccountStatusID == 2)
                    {
                        var adddonor = new FinderSearchResultMV();
                        adddonor.UserID = user.UserID;
                        adddonor.BloodGroup = donor.BloodGroupsTable.BloodGroup;
                        adddonor.BloodGroupID = donor.BloodGroupID;
                        adddonor.ContactNo = donor.ContactNo;
                        adddonor.DonorID = donor.DonorID;
                        adddonor.FullName = donor.FullName;
                        adddonor.Address = donor.Location;
                        adddonor.UserType = "Person";
                        adddonor.UserTypeID = user.UserTypeID;
                        finderMV.SearchResult.Add(adddonor);
                    }
                }
            }

            var bloodbanks = DB.BloodBankStockTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.Quantity > 0).ToList();
            foreach (var bloodbank in bloodbanks)
            {
                var getbloodbank = DB.BloodBankTables.Find(bloodbank.BloodBankID);
                var user = DB.UserTables.Find(getbloodbank.UserID);
                if (userid != user.UserID)
                {
                    if (user.AccountStatusID == 2)
                    {
                        var adddonor = new FinderSearchResultMV();
                        adddonor.UserID = user.UserID;
                        adddonor.BloodGroup = bloodbank.BloodGroupsTable.BloodGroup;
                        adddonor.BloodGroupID = bloodbank.BloodGroupID;
                        adddonor.ContactNo = bloodbank.BloodBankTable.PhoneNo;
                        adddonor.DonorID = bloodbank.BloodBankID;
                        adddonor.FullName = bloodbank.BloodBankTable.BloodBankName;
                        adddonor.Address = bloodbank.BloodBankTable.Address;
                        adddonor.UserType = "Blood Bank";
                        adddonor.UserTypeID = user.UserTypeID;
                        finderMV.SearchResult.Add(adddonor);
                    }
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", finderMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", finderMV.CityID);
            return View(finderMV);
        }

        public ActionResult RequestForBlood(int? donorid, int? usertypeid, int? bloodgroupid)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return Redirect("/Home/MainHome#registrationsection");
            }

            var request = new RequestMV();
            request.AcceptedID = (int)donorid;
            if (usertypeid == 2)
            {
                request.AcceptedTypeID = 1;
            }
            else if (usertypeid == 5)
            {
                request.AcceptedTypeID = 2;
            }
            request.RequiredBloodGroupID = (int)bloodgroupid;


            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestForBlood(RequestMV requestMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return Redirect("/Home/MainHome#registrationsection");
            }
            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;
            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2)//donor
            {

                int.TryParse(Convert.ToString(Session["DonorID"]), out RequestByID);

            }
            else if (UserTypeID == 3)//seeker
            {
                RequestTypeID = 1;
                int.TryParse(Convert.ToString(Session["SeekerID"]), out RequestByID);
            }
            else if (UserTypeID == 4)//hospital
            {
                RequestTypeID = 2;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);
            }
            else if (UserTypeID == 5)//bloodbank
            {
                RequestTypeID = 3;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out RequestByID);
            }


            if (ModelState.IsValid)
            {
                var request = new RequestTable();
                request.RequestDate = DateTime.Now;
                request.AcceptedTypeID = requestMV.AcceptedTypeID;
                request.AcceptedID = requestMV.AcceptedID;
                request.RequiredBloodGroupID = requestMV.RequiredBloodGroupID;
                request.ExpectedDate = requestMV.ExpectedDate;
                request.RequestDetails = requestMV.RequestDetails;
                request.RequestStatusID = 1;
                request.RequestByID = RequestByID;
                request.RequestTypeID=RequestTypeID;
                DB.RequestTables.Add(request);
                DB.SaveChanges();
                return RedirectToAction("ShowAllRequests");
            }
            return View(requestMV);
        }

        public ActionResult ShowAllRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");
            }
            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;
            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2)//donor
            {

                int.TryParse(Convert.ToString(Session["DonorID"]), out RequestByID);

            }
            else if (UserTypeID == 3)//seeker
            {
                RequestTypeID = 1;
                int.TryParse(Convert.ToString(Session["SeekerID"]), out RequestByID);
            }
            else if (UserTypeID == 4)//hospital
            {
                RequestTypeID = 2;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);
            }
            else if (UserTypeID == 5)//bloodbank
            {
                RequestTypeID = 3;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out RequestByID);
            }
            var requests=DB.RequestTables.Where(r=>r.RequestByID==RequestByID && r.RequestTypeID==RequestTypeID).ToList();
            //var list = new List<RequestMV>();
            //foreach(var request in requests)
            //{
            //    var addrequest=new RequestMV();
            //    addrequest.RequestID = request.RequestID;
            //    addrequest.RequestDate = request.RequestDate;
            //    addrequest.RequestByID=request.RequestByID;
            //    addrequest.AcceptedID = request.AcceptedID;
            //    addrequest.AcceptedFullName = "";
            //    addrequest.AcceptedTypeID = request.AcceptedTypeID;
            //    addrequest.AcceptedType = "";
            //    addrequest.RequiredBloodGroupID = request.RequiredBloodGroupID;
            //    addrequest.BloodGroup = "";
            //    addrequest.RequestTypeID = addrequest.RequestTypeID;
            //    addrequest.RequestType = "";
            //    addrequest.RequestStatus = "";
            //    addrequest.RequestStatusID = request.RequestStatusID;
            //    addrequest.ExpectedDate = request.ExpectedDate;
            //    addrequest.RequestDetails = request.RequestDetails;
            //    list.Add(addrequest);
            //}
            return View(requests);
        }

    }
}