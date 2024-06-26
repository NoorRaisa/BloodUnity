using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class BloodGroupController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult AllBloodGroups()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bloodgroups = DB.BloodGroupsTables.ToList();
            var listbloodgroups = new List<BloodGroupsMV>();
            foreach (var bloodgroup in bloodgroups)
            {
                var addbloodgroup = new BloodGroupsMV();
                addbloodgroup.BloodGroupID = bloodgroup.BloodGroupID;
                addbloodgroup.BloodGroup = bloodgroup.BloodGroup;
                listbloodgroups.Add(addbloodgroup);
            }
            return View(listbloodgroups);
        }
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bloodgroups = new BloodGroupsMV();
            return View(bloodgroups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BloodGroupsMV bloodGroupsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var checkbloodgroup=DB.BloodGroupsTables.Where(b=>b.BloodGroup == bloodGroupsMV.BloodGroup ).FirstOrDefault();
                if(checkbloodgroup ==null)
                {
                    var bloodGroupsTable = new BloodGroupsTable();
                    bloodGroupsTable.BloodGroupID = bloodGroupsMV.BloodGroupID;
                    bloodGroupsTable.BloodGroup = bloodGroupsMV.BloodGroup;
                    DB.BloodGroupsTables.Add(bloodGroupsTable);
                    DB.SaveChanges();
                    return RedirectToAction("AllBloodGroups");
                }
                else
                {
                    ModelState.AddModelError("BloodGroup", "Already Exists");
                }
                
            }
            return View(bloodGroupsMV);
        }
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            if (bloodgroup == null)
            {
                return HttpNotFound();
            }
            var bloodGroupsMV = new BloodGroupsMV();
            bloodGroupsMV.BloodGroupID = bloodgroup.BloodGroupID;
            bloodGroupsMV.BloodGroup = bloodgroup.BloodGroup;
            return View(bloodGroupsMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BloodGroupsMV bloodGroupsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var checkbloodgroup = DB.BloodGroupsTables.Where(b => b.BloodGroup == bloodGroupsMV.BloodGroup && b.BloodGroupID!=bloodGroupsMV.BloodGroupID).FirstOrDefault();
                if (checkbloodgroup == null)
                {
                    var bloodGroupsTable = new BloodGroupsTable();
                    bloodGroupsTable.BloodGroupID = bloodGroupsMV.BloodGroupID;
                    bloodGroupsTable.BloodGroup = bloodGroupsMV.BloodGroup;
                    DB.Entry(bloodGroupsTable).State = EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("AllBloodGroups");
                }
                else
                {
                    ModelState.AddModelError("BloodGroup", "Already Exists");
                }

            }
            return View(bloodGroupsMV);
        }

        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            if (bloodgroup == null)
            {
                return HttpNotFound();
            }
            var bloodGroupsMV = new BloodGroupsMV();
            bloodGroupsMV.BloodGroupID = bloodgroup.BloodGroupID;
            bloodGroupsMV.BloodGroup = bloodgroup.BloodGroup;
            return View(bloodGroupsMV);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bloodgroup = DB.BloodGroupsTables.Find(id);
            DB.BloodGroupsTables.Remove(bloodgroup);
            DB.SaveChanges();
            return RedirectToAction("AllBloodGroups");
        }
    }
}