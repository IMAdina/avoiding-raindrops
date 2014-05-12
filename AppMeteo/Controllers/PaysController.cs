using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppMeteo.Models;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace AppMeteo.Controllers
{
    public class PaysController : Controller
    {
        private MyContext db = new MyContext();

        // GET: /Pays/
        public ActionResult Index()
        {
            return View(db.Countries.ToList());
        }

        // GET: /Pays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pays pays = db.Countries.Find(id);
            if (pays == null)
            {
                return HttpNotFound();
            }
            return View(pays);
        }

        // GET: /Pays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Pays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaysId,Nom,ISO")] Pays pays)
        {
            if (ModelState.IsValid)
            {
                db.Countries.Add(pays);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pays);
        }

        // GET: /Pays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pays pays = db.Countries.Find(id);
            if (pays == null)
            {
                return HttpNotFound();
            }
            return View(pays);
        }

        // POST: /Pays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaysId,Nom,ISO")] Pays pays)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pays).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pays);
        }

        // GET: /Pays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pays pays = db.Countries.Find(id);
            if (pays == null)
            {
                return HttpNotFound();
            }
            return View(pays);
        }

        // POST: /Pays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pays pays = db.Countries.Find(id);
            db.Countries.Remove(pays);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET : /Pays/Recherche
        //renvoie formulaire
        public ActionResult Recherche()
        {
            ViewBag.PaysId = new SelectList(db.Countries, "PaysId", "Nom");
            return View("Recherche");
        }

        //POST : /Pays/Recherche/
        //reçoit paramètre par Ajax
        [HttpPost, ActionName("Recherche")]
        [ValidateAntiForgeryToken]
        public ActionResult Recherche(FormCollection form)
        {
            if (!String.IsNullOrEmpty(form[1]))
            {
                Pays pays = db.Countries.Find(int.Parse(form[1]));
                string json = JsonConvert.SerializeObject(pays, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,

                });
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Vous devez sélectionner un pays dans la liste", JsonRequestBehavior.AllowGet); 
            }
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
