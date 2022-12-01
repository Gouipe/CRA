using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRA.Context;
using CRA.Models;

namespace CRA.Controllers
{
    public class LigneSaisiesController : Controller
    {
        private CRAContext db = new CRAContext();

        // GET: LigneSaisies
        public ActionResult Index()
        {
            return View(db.LigneSaisies.ToList());
        }

        // GET: LigneSaisies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LigneSaisie ligneSaisie = db.LigneSaisies.Find(id);
            if (ligneSaisie == null)
            {
                return HttpNotFound();
            }
            return View(ligneSaisie);
        }

        // GET: LigneSaisies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LigneSaisies/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ligne_id,MissionDay,SendingDay,Comment,FractionDay,State")] LigneSaisie ligneSaisie)
        {
            if (ModelState.IsValid)
            {
                db.LigneSaisies.Add(ligneSaisie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ligneSaisie);
        }

        // GET: LigneSaisies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LigneSaisie ligneSaisie = db.LigneSaisies.Find(id);
            if (ligneSaisie == null)
            {
                return HttpNotFound();
            }
            return View(ligneSaisie);
        }

        // POST: LigneSaisies/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ligne_id,MissionDay,SendingDay,Comment,FractionDay,State")] LigneSaisie ligneSaisie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ligneSaisie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ligneSaisie);
        }
            
        // GET: LigneSaisies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LigneSaisie ligneSaisie = db.LigneSaisies.Find(id);
            if (ligneSaisie == null)
            {
                return HttpNotFound();
            }
            return View(ligneSaisie);
        }

        // POST: LigneSaisies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LigneSaisie ligneSaisie = db.LigneSaisies.Find(id);
            db.LigneSaisies.Remove(ligneSaisie);
            db.SaveChanges();
            return RedirectToAction("Index");
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
