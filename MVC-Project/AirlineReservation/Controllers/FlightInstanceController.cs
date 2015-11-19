using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineReservation.Controllers
{
    public class FlightInstanceController : Controller
    {
        /// <summary>
        /// GET /FlightInstance/pagina
        /// </summary>
        public ActionResult Index(int pagina)
        {
            if (TempData["shortMessage"] != null)
            {
                @ViewBag.Message = TempData["shortMessage"].ToString();
            }
            var flightInstances = new Models.FlightInstance().Todos(10, pagina);
            return View(flightInstances);
        }

        /// <summary>
        /// GET /FlightInstance/Create
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST /FlightInstance/PassengerCount/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PassengerCount(long id)
        {
            var flightInstance = new Models.FlightInstance();
            if (flightInstance.Seleccionar(id))
            {
                int pasajeros = flightInstance.PasajerosCount();
                string message = "La instancia de vuelo con ID (" + id + ") tiene " + pasajeros;

                message = message + ( pasajeros == 1 ? " pasaje vendido." : " pasajes vendidos.");

                TempData["shortMessage"] = message;
            }

            return RedirectToAction("Index", new { pagina = 1 });
        }

        /// <summary>
        /// GET /FlightInstance/ChangeAirplane
        /// </summary>
        public ActionResult ChangeAirplane(long id)
        {
            if (TempData["shortMessage"] != null)
            {
                @ViewBag.Message = TempData["shortMessage"].ToString();
            }

            var flightInstance = new Models.FlightInstance();
            if (flightInstance.Seleccionar(id))
            {
                return View(flightInstance);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// POST /FlightInstance/ChangeAirplane/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAirplane(Models.FlightInstance flightInstance)
        {

            string mensaje = flightInstance.ChangeAirplane();

            TempData["shortMessage"] = mensaje;

            return RedirectToAction("ChangeAirplane", new { id = flightInstance.ID });
        }

        /// <summary>
        /// POST /FlightInstance/CancelInstance/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelInstance(long id)
        {
            var flightInstance = new Models.FlightInstance();

            if (flightInstance.Seleccionar(id))
            {
                String mensaje = flightInstance.CancelInstance();
                TempData["shortMessage"] = mensaje;
            }

            return RedirectToAction("Index", new { pagina = 1 });
        }
    }
}