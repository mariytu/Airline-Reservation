using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineReservation.Controllers
{
    public class PassengerController : Controller
    {
        /// <summary>
        /// GET /Passenger/pagina
        /// </summary>
        public ActionResult Index(int pagina)
        {
            var passengers = new Models.Passenger().Todos(10, pagina);
            return View(passengers);
        }

        /// <summary>
        /// GET /Passenger/Details/5
        /// </summary>
        public ActionResult Details(long id)
        {
            var passenger = new Models.Passenger();
            if (passenger.Seleccionar(id))
            {
                return View(passenger);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// GET /Passenger/Create
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST /Passenger/Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                passenger.Crear();
                return RedirectToAction("Details", new { id = passenger.ID });
            }

            return View(passenger);
        }

        /// <summary>
        /// GET /Passenger/Edit/5
        /// </summary>
        public ActionResult Edit(long id)
        {
            var passenger = new Models.Passenger();
            if (passenger.Seleccionar(id))
            {
                return View(passenger);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// POST /Passenger/Edit/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.Passenger passenger)
        {
            var validaciones = passenger.Validar();
            if (!validaciones.Any())
            {
                passenger.Modificar();
            }

            return RedirectToAction("Details", new { id = passenger.ID });
        }

        /// <summary>
        /// GET /Passenger/Delete/5
        /// </summary>
        public ActionResult Delete(long id)
        {
            var passenger = new Models.Passenger();
            if (passenger.Seleccionar(id))
            {
                return View(passenger);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// POST /Passenger/Delete/5
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var passenger = new Models.Passenger();
            if (passenger.Seleccionar(id))
            {
                passenger.Eliminar();
            }

            return RedirectToAction("Index", new { pagina = 1 });
        }
    }
}