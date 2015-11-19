using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineReservation.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult Index()
        {
            var reser = new AirlineReservation.Models.Reservacion();
            reser.fecha = DateTime.Now;
            return View( new AirlineReservation.Models.Reservacion());
        }


        public ActionResult BuscarVuelos(AirlineReservation.Models.Reservacion reservacion)
        {
            // hacer la busqueda por los datos que llegan 



            return View();
        }

      
    }
}
