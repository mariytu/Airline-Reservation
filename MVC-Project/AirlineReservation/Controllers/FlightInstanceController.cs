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
    }
}