using BloodUnity.Helper_Class;
using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            
            var list = new List<BloodBankStockMV>();
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

        public ActionResult AllCampaigns()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            var allcampaigns = DB.CampaignTables.Where(c => c.BloodBankID == bloodbankID);
            if(allcampaigns.Count()>0)
            {
                allcampaigns=allcampaigns.OrderByDescending(o => o.CampaignID);
            }
            return View(allcampaigns);
        }

        public ActionResult NewCampaign()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var campaignMV=new CampaignMV();
            return View(campaignMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCampaign(CampaignMV campaignMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            campaignMV.BloodBankID= bloodbankID;
            if(ModelState.IsValid)
            {
                var campaign = new CampaignTable();
                campaign.BloodBankID = bloodbankID;
                campaign.CampaignDate = campaignMV.CampaignDate;
                campaign.StartTime = campaignMV.StartTime;
                campaign.EndTime = campaignMV.EndTime;
                campaign.Location=campaignMV.Location;
                campaign.CampaignDetails = campaignMV.CampaignDetails;
                campaign.CampaignTitle = campaignMV.CampaignTitle;
                campaign.CampaignPhoto = "~/Content/CampaignPhoto";
                DB.CampaignTables.Add(campaign);
                DB.SaveChanges();
                if(campaignMV.CampaignPhotoFile != null)
                {
                    var folder = "~/Content/CampaignPhoto";
                    var file = string.Format("{0}.jpg", campaignMV.CampaignID);
                    var response = FileHelpers.UploadPhoto(campaignMV.CampaignPhotoFile, folder, file);
                    if(response)
                    {
                        var pic = string.Format("{0}/{1}", folder,file);
                        campaign.CampaignPhoto = pic;
                        DB.Entry(campaign).State = EntityState.Modified;
                        DB.SaveChanges();
                    }

                }
                return RedirectToAction("AllCampaigns");
            }

            return View(campaignMV);
        }

  
    }
}