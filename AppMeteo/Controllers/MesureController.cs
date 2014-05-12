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
using System.Data.Entity.Core.Objects;

namespace AppMeteo.Controllers
{
    public class MesureController : Controller
    {
        private MyContext db = new MyContext();

        // GET: /Mesure/
        public ActionResult Index()
        {
            var mesures = db.Mesures.Include(m => m.Station).Include(m => m.Valeur);
            return View("_IndexMesures", mesures.ToList());
        }
        //GET: Mesures/Select/5
        //@param id = StationId
        //sélectionne les mesures associées à une station
        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Mesure> listMesures = db.Mesures.Where(m => m.StationId == id).ToList();
            return View("_IndexMesures", listMesures);
        }

        // GET: /Mesure/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mesure mesure = db.Mesures.Find(id);
            if (mesure == null)
            {
                return HttpNotFound();
            }
            return View(mesure);
        }

        // GET: /Mesure/Create
        public ActionResult Create()
        {
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationId");
            return View();
        }

        // POST: /Mesure/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MomentPrelevement,StationId")] Mesure mesure)
        {
            if (ModelState.IsValid)
            {
                Valeur valeur = new Valeur();
                mesure.Valeur = valeur;
                db.Mesures.Add(mesure);
                db.SaveChanges();
                return RedirectToAction("Create", "Valeur", new { id = valeur.ValeurId });
            }

            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationId", mesure.StationId);
            return View(mesure);
        }

        // GET: /Mesure/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mesure mesure = db.Mesures.Find(id);
            if (mesure == null)
            {
                return HttpNotFound();
            }
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationId", mesure.StationId);
            ViewBag.MesureId = new SelectList(db.Valeurs, "ValeurId", "ValeurId", mesure.MesureId);
            return View(mesure);
        }

        // POST: /Mesure/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MesureId,MomentPrelevement,StationId")] Mesure mesure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mesure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationId", mesure.StationId);
            ViewBag.MesureId = new SelectList(db.Valeurs, "ValeurId", "ValeurId", mesure.MesureId);
            return View(mesure);
        }
        // GET: /Mesure/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Mesure mesure = db.Mesures.Find(id);
            db.Mesures.Remove(mesure);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET : /Mesure/Recherche
        //renvoie formulaire
        public ActionResult Recherche()
        {
            ViewBag.Pays = new SelectList(db.Countries, "PaysId", "Nom");
            return View("Recherche");
        }
        //GET Mesure/ListeStations/id
        //@param id= PaysId

        public ActionResult ListeStations(int? id)
        {
            var stations = db.Stations.Where(s => s.Pays.PaysId == id).ToList();
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                                stations.ToArray(),
                                "StationId",
                                "CodePostal")
                           , JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Recherche");
        }
        //GET Mesure/ListeDates/id
        //@param id= StationId

        public ActionResult ListeDates(int? id)
        {
            if (id != 0 && id != null)
            {
                Station station = db.Stations.Find(id);
                List<Mesure> mesures = station.Mesures;

                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new SelectList(
                                    mesures.ToArray(),
                                    "MomentPrelevement",
                                    "MomentPrelevement")
                               , JsonRequestBehavior.AllowGet);
                }

                return RedirectToAction("Recherche");
            }
            else {
                return RedirectToAction("Recherche");
            }
        }
        //POST : /Mesure/Recherche/
        //reçoit paramètre par Ajax
        [HttpPost, ActionName("Recherche")]
        [ValidateAntiForgeryToken]
        public ActionResult Recherche(FormCollection form)
        {
            if (form[2] != "Sélectionnez une station" && form[3] != "Sélectionnez une date")
            {
                int StationId = int.Parse(form[2]);
                DateTime date = DateTime.Parse(form[3]);
                var mesure = db.Mesures.SingleOrDefault(m => m.StationId == StationId && DbFunctions.DiffDays(m.MomentPrelevement, date) == 0 && DbFunctions.DiffHours(m.MomentPrelevement, date) == 0 && DbFunctions.DiffMinutes(m.MomentPrelevement, date) == 0);
                Valeur valeur = mesure.Valeur;
                valeur.Mesure = null;
                var json = JsonConvert.SerializeObject(valeur, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("Une des sélections demandées n'est pas correcte. Sélectionnez d'abord un pays, ensuite une station et enfin une date.", JsonRequestBehavior.AllowGet);
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
