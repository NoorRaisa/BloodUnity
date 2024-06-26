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
    public class AccountStatusController : Controller
    {
        private OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult AllAccountStatus()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountStatuses = DB.AccountStatusTables.ToList();
            var listaccountstatus = new List<AccountStatusMV>();
            foreach (var accountstatus in accountStatuses)
            {
                var addAccountStatus = new AccountStatusMV();
                addAccountStatus.AccountStatusID = accountstatus.AccountStatusID;
                addAccountStatus.AccountStatus = accountstatus.AccountStatus;
                listaccountstatus.Add(addAccountStatus);
            }
            return View(listaccountstatus);
        }
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountStatuses = new AccountStatusMV();
            return View(accountStatuses);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountStatusMV AccountStatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var checkstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == AccountStatusMV.AccountStatus).FirstOrDefault();
                if (checkstatus == null)
                {
                    var accountStatusTables = new AccountStatusTable();
                    accountStatusTables.AccountStatusID = AccountStatusMV.AccountStatusID;
                    accountStatusTables.AccountStatus = AccountStatusMV.AccountStatus;
                    DB.AccountStatusTables.Add(accountStatusTables);
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatus");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exists");
                }

            }
            return View(AccountStatusMV);
        }
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountStatus = DB.AccountStatusTables.Find(id);
            if (accountStatus == null)
            {
                return HttpNotFound();
            }
            var AccountStatusMV = new AccountStatusMV();
            AccountStatusMV.AccountStatusID = accountStatus.AccountStatusID;
            AccountStatusMV.AccountStatus = accountStatus.AccountStatus;
            return View(AccountStatusMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountStatusMV AccountStatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var checkstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == AccountStatusMV.AccountStatus && b.AccountStatusID != AccountStatusMV.AccountStatusID).FirstOrDefault();
                if (checkstatus == null)
                {
                    var accountStatusTable = new AccountStatusTable();
                    accountStatusTable.AccountStatusID = AccountStatusMV.AccountStatusID;
                    accountStatusTable.AccountStatus = AccountStatusMV.AccountStatus;
                    DB.Entry(accountStatusTable).State = EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatus");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exists");
                }

            }
            return View(AccountStatusMV);
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
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var AccountStatusMV = new AccountStatusMV();
            AccountStatusMV.AccountStatusID = accountstatus.AccountStatusID;
            AccountStatusMV.AccountStatus = accountstatus.AccountStatus;
            return View(AccountStatusMV);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            DB.AccountStatusTables.Remove(accountstatus);
            DB.SaveChanges();
            return RedirectToAction("AllAccountStatus");
        }
    }
}