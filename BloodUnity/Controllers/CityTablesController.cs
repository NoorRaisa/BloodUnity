using BloodUnity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace BloodUnity.Controllers
{
    public class CityTablesController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        // GET: CityTables
        public ActionResult Index()
        {
            return View(DB.CityTables.ToList());
        }

        // GET: CityTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityTable cityTable = DB.CityTables.Find(id);
            if (cityTable == null)
            {
                return HttpNotFound();
            }
            return View(cityTable);
        }

        // GET: CityTables/Create
        public ActionResult Create()
        {
            var city = new CityTable(); 
            return View(city);
        }

        // POST: CityTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityID,City")] CityTable cityTable)
        {
            if (ModelState.IsValid)
            {
                DB.CityTables.Add(cityTable);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cityTable);
        }

        // GET: CityTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityTable cityTable = DB.CityTables.Find(id);
            if (cityTable == null)
            {
                return HttpNotFound();
            }
            return View(cityTable);
        }

        // POST: CityTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityID,City")] CityTable cityTable)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(cityTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cityTable);
        }

        // GET: CityTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityTable cityTable = DB.CityTables.Find(id);
            if (cityTable == null)
            {
                return HttpNotFound();
            }
            return View(cityTable);
        }

        // POST: CityTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CityTable cityTable = DB.CityTables.Find(id);
            DB.CityTables.Remove(cityTable);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
