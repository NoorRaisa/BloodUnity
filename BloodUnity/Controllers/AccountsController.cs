using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class AccountsController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult AllNewUserRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var list = new List<RegistrationMV>();
            var users = DB.UserTables.Where(u => u.AccountStatusID == 1).ToList();

            return View(users);
        }
        public ActionResult UserDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);

            return View(user);
        }
        public ActionResult UserApproved(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 2;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");
        }
        public ActionResult UserRejected(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 3;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");

        }
        public ActionResult AddNewDonorByBloodBank()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var CollectBloodMV = new CollectBloodMV();
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(CollectBloodMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewDonorByBloodBank(CollectBloodMV collectBloodMV)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            var currentdate = DateTime.Now.Date;
            var currentcampaign = DB.CampaignTables.Where(c => c.CampaignDate == currentdate && c.BloodBankID == bloodbankID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                using (var transaction = DB.Database.BeginTransaction())
                {
                    try
                    {
                        var checkdonor = DB.DonorTables.Where(d => d.NID.Trim().Replace("-", "") == collectBloodMV.DonorDetails.NID.Replace("-", "").Trim()).FirstOrDefault();
                        if (checkdonor == null)
                        {
                            var user = new UserTable();
                            user.UserName = collectBloodMV.DonorDetails.FullName.Trim();
                            user.Password = "12345";
                            user.EmailAddress = collectBloodMV.DonorDetails.EmailAddress;
                            user.AccountStatusID = 2;
                            user.UserTypeID = 2;
                            user.Description = "Added by Blood Bank";
                            DB.UserTables.Add(user);
                            //DB.SaveChanges();

                            var Donor = new DonorTable();
                            Donor.FullName = collectBloodMV.DonorDetails.FullName;
                            Donor.BloodGroupID = collectBloodMV.BloodGroupID;
                            Donor.Location = collectBloodMV.DonorDetails.Location;
                            Donor.ContactNo = collectBloodMV.DonorDetails.ContactNo;
                            Donor.LastDonationDate = DateTime.Now;
                            Donor.NID = collectBloodMV.DonorDetails.NID;
                            Donor.GenderID = collectBloodMV.GenderID;
                            Donor.CityID = collectBloodMV.CityID;
                            Donor.UserID = user.UserID;
                            DB.DonorTables.Add(Donor);
                            DB.SaveChanges();
                            checkdonor = DB.DonorTables.Where(d => d.NID.Trim().Replace("-", "") == collectBloodMV.DonorDetails.NID.Trim().Replace("-", "")).FirstOrDefault();

                        }

                        if ((DateTime.Now - checkdonor.LastDonationDate).TotalDays < 120)
                        {
                            ModelState.AddModelError(string.Empty, "Donor Already Donated Blood in Previous 120 Days!");
                            transaction.Rollback();
                        }
                        else
                        {

                            var checkbloodgroupstock = DB.BloodBankStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                            if (checkbloodgroupstock == null)
                            {
                                var bloodbankstock = new BloodBankStockTable();
                                bloodbankstock.BloodBankID = bloodbankID;
                                bloodbankstock.BloodGroupID = collectBloodMV.BloodGroupID;
                                bloodbankstock.Quantity = 0;
                                bloodbankstock.Status = true;
                                bloodbankstock.Description = "";
                                DB.BloodBankStockTables.Add(bloodbankstock);
                                DB.SaveChanges();
                                checkbloodgroupstock = DB.BloodBankStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                            }
                            checkbloodgroupstock.Quantity += (int)collectBloodMV.Quantity;
                            DB.Entry(checkbloodgroupstock).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();

                            var collectblooddetail = new BloodBankStockDetailTable();
                            collectblooddetail.BloodBankStockID = checkbloodgroupstock.BloodBankStockID;
                            collectblooddetail.BloodGroupID = collectBloodMV.BloodGroupID;
                            collectblooddetail.CampaignID = currentcampaign.CampaignID;
                            collectblooddetail.Quantity = collectBloodMV.Quantity;
                            collectblooddetail.DonorID = checkdonor.DonorID;
                            collectblooddetail.DonateDateTime = DateTime.Now;
                            DB.BloodBankStockDetailTables.Add(collectblooddetail);
                            DB.SaveChanges();
                            transaction.Commit();
                            checkdonor.LastDonationDate = DateTime.Now;
                            DB.Entry(checkdonor).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();
                            return RedirectToAction("BloodBankStock", "BloodBank");
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        Console.WriteLine(e);
                        ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                        transaction.Rollback();
                    }
                    catch (DbUpdateException e)
                    {
                        Console.WriteLine(e);
                        ModelState.AddModelError(string.Empty, "Please Provide Correct Information!");
                        transaction.Rollback();
                    }
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please Provide Donor's Full Detail!");
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", collectBloodMV.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", collectBloodMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", collectBloodMV.GenderID);
            return View(collectBloodMV);
        }
    }
}