using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppMeteo.Models;

namespace AppMeteo.Controllers
{
    public class ValeurController : Controller
    {
        private MyContext db = new MyContext();

        // GET: /Valeur/
        public ActionResult Index()
        {
            return View(db.Valeurs.ToList());
        }
        //GET: /Valeur/Select/5
        //@param: id=MesureId
        ////sélectionne les stations associées à un pays
        public ActionResult Select(int? id)
    {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Valeur> listValeurs = db.Valeurs.Where(v=>v.Mesure.MesureId==id).ToList();
            return View("_IndexValeurs", listValeurs);
    }
        // GET: /Valeur/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Valeur valeur = db.Valeurs.Find(id);
            if (valeur == null)
            {
                return HttpNotFound();
            }
            return View(valeur);
        }

        // GET: /Valeur/Create/3001
        public ActionResult Create(int? id)
        {
            ViewBag.ValeurId = id;
            return View();
        }

        // POST: /Valeur/Create/3001
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Temperature,Pression,Precipitations,ATMO")] Valeur valeur, int id)
        {
            if (ModelState.IsValid)
            {
                valeur.ValeurId = id;
                db.Entry(valeur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(valeur);
        }

        // GET: /Valeur/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Valeur valeur = db.Valeurs.Find(id);
            if (valeur == null)
            {
                return HttpNotFound();
            }
            return View(valeur);
        }

        // POST: /Valeur/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ValeurId,Temperature,Pression,Precipitations,ATMO")] Valeur valeur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(valeur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(valeur);
        }

        // GET: /Valeur/Delete/5

        public ActionResult Delete(int id)
        {
           //valeur et Mesure ont la même PK et Mesure a Cascade OnDelete
            Mesure mesure = db.Mesures.Find(id);
            db.Mesures.Remove(mesure);
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
