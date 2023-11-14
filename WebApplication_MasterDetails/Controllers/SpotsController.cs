using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication_MasterDetails.Models;

namespace WebApplication_MasterDetails.Controllers
{
    public class SpotsController : Controller
    {
        TravelDbContext db = new TravelDbContext();
        public ActionResult Index()
        {
            return View(db.Spots.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="SpotId,SpotName")]Spot spot)
        {
            if(ModelState.IsValid)
            {
                db.Spots.Add(spot);
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Spot spot = db.Spots.Find(id);
            if(spot == null)
            {
                return HttpNotFound();
            }
            return View(spot);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SpotId,SpotName")] Spot spot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spot).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest) ;
                
            }
            Spot spot = db.Spots.Find(id);
            if(spot == null)
            {
                return HttpNotFound() ;
            }
            return View(spot);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Spot spot = db.Spots.Find(id);
            db.Spots.Remove(spot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}