using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
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
                RequestTypeID = 4;
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
                request.RequestTypeID = RequestTypeID;
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
                return RedirectToAction("Login", "Home");
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
            var requests = DB.RequestTables.Where(r => r.RequestByID == RequestByID && r.RequestTypeID == RequestTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM,yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;
                if (request.AcceptedTypeID == 1) //donor
                {
                    var getdonor = DB.DonorTables.Find(request.AcceptedID);
                    addrequest.AcceptedFullName = getdonor.FullName;
                    addrequest.ContactNo = getdonor.ContactNo;
                    addrequest.Address = getdonor.Location;
                }
                else if (request.AcceptedTypeID == 2) //bloodbank
                {
                    var getbloodbank = DB.BloodBankTables.Find(request.AcceptedID);
                    addrequest.AcceptedFullName = getbloodbank.BloodBankName;
                    addrequest.ContactNo = getbloodbank.PhoneNo;
                    addrequest.Address = getbloodbank.Address;

                }

                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodGroupID = request.RequiredBloodGroupID;
                var bloodgroup = DB.BloodGroupsTables.Find(addrequest.RequiredBloodGroupID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = addrequest.RequestTypeID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM,yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }
            return View(list);
        }
        public ActionResult CancelRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            request.RequestStatusID = 4;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("ShowAllRequests");

        }

        public ActionResult CancelRequestByDonor(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            request.RequestStatusID = 4;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("DonorRequests");

        }

        public ActionResult AcceptRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            request.RequestStatusID = 2;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("DonorRequests");

        }

        public ActionResult DonorRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int UserTypeID = 0;
            int AcceptedTypeID = 0;
            int AcceptedByID = 0;
            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2)//donor
            {
                AcceptedTypeID = 1;
                int.TryParse(Convert.ToString(Session["DonorID"]), out AcceptedByID);

            }
            else if (UserTypeID == 3)//seeker
            {

                int.TryParse(Convert.ToString(Session["SeekerID"]), out AcceptedByID);
            }
            else if (UserTypeID == 4)//hospital
            {

                int.TryParse(Convert.ToString(Session["HospitalID"]), out AcceptedByID);
            }
            else if (UserTypeID == 5)//bloodbank
            {
                AcceptedTypeID = 2;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out AcceptedByID);
            }
            var requests = DB.RequestTables.Where(r => r.AcceptedID == AcceptedByID && r.AcceptedTypeID == AcceptedTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM,yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;
                if (request.RequestTypeID == 1) //Seeker
                {
                    var getseeker = DB.SeekerTables.Find(request.RequestByID);
                    addrequest.RequestBy = getseeker.FullName;
                    addrequest.ContactNo = getseeker.ContactNo;
                    addrequest.Address = getseeker.Address;
                }
                else if (request.RequestTypeID == 2) //Hospital
                {
                    var gethospital = DB.HospitalTables.Find(request.RequestByID);
                    addrequest.AcceptedFullName = gethospital.FullName;
                    addrequest.ContactNo = gethospital.PhoneNo;
                    addrequest.Address = gethospital.Address;

                }
                else if (request.RequestTypeID == 3) //Blood Bank
                {
                    var getbloodbank = DB.BloodBankTables.Find(request.RequestByID);
                    addrequest.AcceptedFullName = getbloodbank.BloodBankName;
                    addrequest.ContactNo = getbloodbank.PhoneNo;
                    addrequest.Address = getbloodbank.Address;

                }

                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodGroupID = request.RequiredBloodGroupID;
                var bloodgroup = DB.BloodGroupsTables.Find(addrequest.RequiredBloodGroupID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = addrequest.RequestTypeID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM,yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }
            return View(list);
        }
        public ActionResult CompleteRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            if (request.AcceptedTypeID == 1) // Donor
            {
                var donor = DB.DonorTables.Find(request.AcceptedID);
                donor.LastDonationDate = DateTime.Now;
                DB.Entry(donor).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();

                request.RequestStatusID = 3;
                DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("ShowAllRequests");
            }


            var bloodbank = DB.BloodBankTables.Find(request.AcceptedID);
            var bloodbankstockMV = new BloodBankStockMV();
            bloodbankstockMV.BloodBankStockID = request.RequestID;
            bloodbankstockMV.BloodBankID = bloodbank.BloodBankID;
            bloodbankstockMV.BloodGroupID = request.RequiredBloodGroupID;
            bloodbankstockMV.BloodBank = bloodbank.BloodBankName;
            var bloodgroup = DB.BloodGroupsTables.Find(request.RequiredBloodGroupID);
            bloodbankstockMV.BloodGroup = bloodgroup.BloodGroup;
            bloodbankstockMV.Quantity = 1;
            return View(bloodbankstockMV);
        }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult CompleteRequest(BloodBankStockMV bloodBankStockMV)
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
                {
                    return RedirectToAction("Login", "Home");
                }
                try
                {
                    var request = DB.RequestTables.Find(bloodBankStockMV.BloodBankStockID);

                    var bloodstock = DB.BloodBankStockTables.Where(b => b.BloodBankID == bloodBankStockMV.BloodBankID && b.BloodGroupID == bloodBankStockMV.BloodGroupID).FirstOrDefault();
                    if (bloodstock.Quantity < bloodBankStockMV.Quantity)
                    {
                        ModelState.AddModelError(string.Empty, "Available Quantity is " + bloodstock.Quantity + "!");
                        return View(bloodBankStockMV);
                    }

                    bloodstock.Quantity = bloodstock.Quantity - bloodBankStockMV.Quantity;
                    DB.Entry(bloodstock).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();

                    request.RequestStatusID = 3;
                    DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();

                    return RedirectToAction("ShowAllRequests");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Please Provide Quantity!");
                    return View(bloodBankStockMV);
                }



            }
        }
    }
