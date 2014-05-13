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

namespace AppMeteo.Controllers
{
    public class StationController : Controller
    {
        private MyContext db = new MyContext();

        // GET: /Station/
        public ActionResult Index()
        {
            var stations = db.Stations.Include(s => s.Pays);
            return View("_IndexStations", stations.ToList());
        }
        //AJAX GET
        public List<Pays> ListePays()
        {
            return db.Countries.ToList();
        }

        //GET Station/ListeStations/id
        //@param id= PaysId

        public ActionResult ListeStations(int id)
        {
            var stations = db.Stations.Where(s => s.Pays.PaysId == id).ToList();
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                                stations.ToArray(),
                                "StationID",
                                "CodePostal")
                           , JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Recherche");
        }
        public ActionResult Recherche()
        {
            ViewBag.Pays = new SelectList(db.Countries, "PaysId", "Nom");
            return View("Recherche");
        }

        //POST : /Station/Recherche/
        //reçoit paramètre par Ajax
        [HttpPost, ActionName("Recherche")]
        [ValidateAntiForgeryToken]
        public ActionResult Recherche(FormCollection form)
        {
            if (form[2]!="Sélectionnez une station")
            {
                int StationId = int.Parse(form[2]);
                Station stationDemandee = db.Stations.Find(StationId);
                var mesures = stationDemandee.Mesures;
                foreach (Mesure item in mesures)
                {
                    item.Station = null;
                }
                var json = JsonConvert.SerializeObject(mesures, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("Vous devez sélectionner un pays ensuite une station", JsonRequestBehavior.AllowGet);
            }
        }

        //GET: /Station/Calculer
        //calcule le résultat des statistiques (T° min, max moyenne)
        public ActionResult Calculer()
        {
            var stations = db.Stations.ToList();
            return View(stations);
        }

        //GET: /Station/_Calculer/5
        //renvoie le formulaire en tant que vue partielle
        public ActionResult GetForm(int? id)
        {
            Station station = db.Stations.Find(id);
            List<Mesure> mesures=station.Mesures;
            List<DateTime> dates=new List <DateTime>();
            foreach(Mesure mesure in mesures)
            {
                dates.Add(mesure.MomentPrelevement);
            }
            ViewBag.SelectListDates = new SelectList(dates, "", "");
            return View(station);
        }
        //POST: /Station/GetStatistiques/7
        //@param id=StationId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult GetForm(int id, FormCollection form)
        {
            Station station = db.Stations.Find(id);
            string list = form[1];
            string [] dates=list.Split(new Char [] {','} );
            DateTime dateInitiale = new DateTime();
            DateTime dateFinale = new DateTime();
            if (dates[0] != "" && dates[1] != "")
            {
                dateInitiale = DateTime.Parse(dates[0]);
                dateFinale = DateTime.Parse(dates[1]);
            }
            else
            {
                ViewBag.ErrorMessage = "Vous devez sélectionner deux moments de prélèvements dans la liste déroulante.";
                return PartialView("_Resultat");
            }
            List<decimal> listTemperatures = new List<decimal>();

            foreach (Mesure item in station.Mesures)
            {
                if (item.MomentPrelevement < dateFinale && item.MomentPrelevement > dateInitiale)
                {
                    listTemperatures.Add(item.Valeur.Temperature);
                }
            }
            if (listTemperatures.Count() != 0)
            {
                ViewBag.TempMin = listTemperatures.Min().ToString();
                ViewBag.TempMax = listTemperatures.Max().ToString();
                ViewBag.TempAverage = listTemperatures.Average().ToString();
            }
            else {
                ViewBag.ErrorMessage = "Vous devez sélectionner deux moments de prélèvements distincts";
            }

            return PartialView("_Resultat");
        }
        // GET: /Station/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }
        //GET: /Select/Select/5
        //@param: id=PaysId
        //sélectionne les stations associées à un pays
        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Station> listStations = db.Stations.Where(s => s.PaysId == id).ToList();
            return View("_IndexStations", listStations);
        }

        // GET: /Station/Create
        public ActionResult Create()
        {
            ViewBag.PaysId = new SelectList(db.Countries, "PaysId", "Nom");
            return View();
        }

        // POST: /Station/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StationId,CodePostal,PaysId")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Stations.Add(station);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PaysId = new SelectList(db.Countries, "PaysId", "Nom", station.PaysId);
            return View(station);
        }

        // GET: /Station/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaysId = new SelectList(db.Countries, "PaysId", "Nom", station.PaysId);
            return View(station);
        }

        // POST: /Station/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StationId,CodePostal,PaysId")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Entry(station).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaysId = new SelectList(db.Countries, "PaysId", "Nom", station.PaysId);
            return View(station);
        }

        // GET: /Station/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: /Station/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Station station = db.Stations.Find(id);
            db.Stations.Remove(station);
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
