using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodUnity.Controllers
{
    public class BloodBankController : Controller
    {
        OnlineBloodBankDbEntities DB= new OnlineBloodBankDbEntities();
        // GET: BloodBank
        public ActionResult BloodBankStock()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var list = new List<BloodBankStockMV>();
            int bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]),out bloodbankID);
            var stocklist = DB.BloodBankStockTables.Where(b => b.BloodBankID == bloodbankID);
            foreach(var stock in stocklist)
            {
                string bloodbank = stock.BloodBankTable.BloodBankName;
                string bloodgroup = stock.BloodGroupsTable.BloodGroup;

                var bloodBankStockmv = new BloodBankStockMV();

                bloodBankStockmv.BloodBankStockID=stock.BloodBankStockID;
                bloodBankStockmv.BloodBankID=stock.BloodBankStockID;
                bloodBankStockmv.BloodBank = bloodbank;
                bloodBankStockmv.BloodGroupID=stock.BloodGroupID;
                bloodBankStockmv.BloodGroup= bloodgroup;
                bloodBankStockmv.Quantity=stock.Quantity;
                bloodBankStockmv.Status = stock.Status == true ? "Ready to Use" : "Not Ready to Use";
                bloodBankStockmv.Description=stock.Description;
                list.Add(bloodBankStockmv);
            }

            return View(list);
        }

        public ActionResult NewCampaign()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }
}