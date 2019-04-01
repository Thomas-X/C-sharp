using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Space_Fridge_Forum.Models;

namespace Space_Fridge_Forum.Controllers
{
    [Authorize]
    public class FridgesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fridges
        public ActionResult Index()
        {
            var fridges = db.Fridges.Include(f => f.User);
            var filterFridges = fridges.ToList().Where(x => x.UserId == User.Identity.GetUserId());
            return View(filterFridges);
        }

        // GET: Fridges/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fridge fridge = db.Fridges.Find(id);
            if (fridge == null)
            {
                return HttpNotFound();
            }
            if (fridge.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            return View(fridge);

        }
        
        // GET: Fridges/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fridge fridge = db.Fridges.Find(id);
            if (fridge == null)
            {
                return HttpNotFound();
            }
            if (fridge.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", fridge.UserId);
            return View(fridge);
        }

        // POST: Fridges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Name,HexColor")] Fridge fridge)
        {
            if (fridge.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(fridge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", fridge.UserId);
            return View(fridge);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
