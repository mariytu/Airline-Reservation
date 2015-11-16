using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AirlineReservation.Controllers
{
    public class AircraftController : Controller
    {
        /// <summary>
        /// GET /Aircraft/pagina
        /// </summary>
        public ActionResult Index(int pagina)
        {
            var aircrafts = new Models.Aircraft().Todos(10, pagina);
            return View(aircrafts);
        }

        /// <summary>
        /// GET /Aircraft/Details/5
        /// </summary>
        public ActionResult Details(long id)
        {
            var aircraft = new Models.Aircraft();
            if (aircraft.Seleccionar(id))
            {
                return View(aircraft);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// GET /Aircraft/Create
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST /Aircraft/Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Aircraft aircraft)
        {
            if (ModelState.IsValid)
            {
                aircraft.Crear();
                return RedirectToAction("Details", new { id = aircraft.ID });
            }

            return View(aircraft);
        }

        /// <summary>
        /// GET /Aircraft/Edit/5
        /// </summary>
        public ActionResult Edit(long id)
        {
            var aircraft = new Models.Aircraft();
            if (aircraft.Seleccionar(id))
            {
                return View(aircraft);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// POST /Aircraft/Edit/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.Aircraft aircraft)
        {
            if (ModelState.IsValid)
            {
                aircraft.Modificar();
            }
            return RedirectToAction("Details", new { id = aircraft.ID });
        }

        /// <summary>
        /// GET /Aircraft/Delete/5
        /// </summary>
        public ActionResult Delete(long id)
        {
            var aircraft = new Models.Aircraft();
            if (aircraft.Seleccionar(id))
            {
                return View(aircraft);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// POST /Aircraft/Delete/5
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var aircraft = new Models.Aircraft();
            if (aircraft.Seleccionar(id))
            {
                aircraft.Eliminar();
            }

            return RedirectToAction("Index", new { pagina = 1 });
        }
    }
}