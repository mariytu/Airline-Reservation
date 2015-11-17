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
    }
}