using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult UserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);

            return View(user);
        }

        public ActionResult EditUserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var userprofile = new RegistrationMV();
            var user = DB.UserTables.Find(id);
            userprofile.UserTypeID = user.UserTypeID;
            userprofile.User.UserID = user.UserID;
            userprofile.User.UserName = user.UserName;
            userprofile.User.Password = user.Password;
            userprofile.User.EmailAddress = user.EmailAddress;
            userprofile.User.AccountStatusID = user.AccountStatusID;
            userprofile.User.UserTypeID = user.UserTypeID;
            userprofile.User.Description = user.Description;

            if (user.SeekerTables.Count > 0)
            {
                var seeker = user.SeekerTables.FirstOrDefault();
                userprofile.Seeker.SeekerID = seeker.SeekerID;
                userprofile.Seeker.FullName = seeker.FullName;
                userprofile.Seeker.Age = seeker.Age;
                userprofile.Seeker.CityID = seeker.CityID;
                userprofile.Seeker.BloodGroupID = seeker.BloodGroupID;
                userprofile.Seeker.ContactNo = seeker.ContactNo;
                userprofile.Seeker.NID = seeker.NID;
                userprofile.Seeker.GenderID = seeker.GenderID;
                userprofile.Seeker.Address = seeker.Address;
                userprofile.Seeker.UserID = seeker.UserID;


                userprofile.ContactNo = seeker.ContactNo;
                userprofile.CityID = seeker.CityID;
                userprofile.BloodGroupID = seeker.BloodGroupID;
                userprofile.GenderID = seeker.GenderID;
               

            }

            else if (user.HospitalTables.Count > 0)
            {
                var hospital = user.HospitalTables.FirstOrDefault();
                userprofile.Hospital.HospitalID = hospital.HospitalID;
                userprofile.Hospital.FullName = hospital.FullName;
                userprofile.Hospital.Address = hospital.Address;
                userprofile.Hospital.PhoneNo = hospital.PhoneNo;
                userprofile.Hospital.Website = hospital.Website;
                userprofile.Hospital.Email = hospital.Email;
                userprofile.Hospital.Location = hospital.Location;
                userprofile.Hospital.CityID = hospital.CityID;
                userprofile.Hospital.UserID = hospital.UserID;


                userprofile.ContactNo = hospital.PhoneNo;
                userprofile.CityID = hospital.CityID;
            }

            else if (user.BloodBankTables.Count > 0)
            {
                var bloodbank = user.BloodBankTables.FirstOrDefault();

                userprofile.BloodBank.BloodBankID = bloodbank.BloodBankID;
                userprofile.BloodBank.BloodBankName = bloodbank.BloodBankName;
                userprofile.BloodBank.Address = bloodbank.Address;
                userprofile.BloodBank.PhoneNo = bloodbank.PhoneNo;
                userprofile.BloodBank.Location = bloodbank.Location;
                userprofile.BloodBank.Website = bloodbank.Website;
                userprofile.BloodBank.Email = bloodbank.Email;
                userprofile.BloodBank.CItyID = bloodbank.CItyID;
                userprofile.BloodBank.UserID = bloodbank.UserID;
                userprofile.ContactNo = bloodbank.PhoneNo;
                userprofile.CityID = bloodbank.CItyID;

            }
            else if (user.DonorTables.Count > 0)
            {
                var donor = user.DonorTables.FirstOrDefault();

                userprofile.Donor.DonorID = donor.DonorID;
                userprofile.Donor.FullName = donor.FullName;
                userprofile.Donor.GenderID = donor.GenderID;
                userprofile.Donor.BloodGroupID = donor.BloodGroupID;
                userprofile.Donor.LastDonationDate = donor.LastDonationDate;
                userprofile.Donor.ContactNo = donor.ContactNo;
                userprofile.Donor.NID = donor.NID;
                userprofile.Donor.Location = donor.Location;
                userprofile.Donor.CityID = donor.CityID;
                userprofile.Donor.UserID = donor.UserID;


                userprofile.ContactNo = donor.ContactNo;
                userprofile.CityID = donor.CityID;
                userprofile.BloodGroupID = donor.BloodGroupID;
                userprofile.GenderID = donor.GenderID;

            }

            return View(user);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserProfile(RegistrationMV registrationMVid)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //var user = DB.UserTables.Find(id);

            return View(registrationMVid);

        }

    }
}