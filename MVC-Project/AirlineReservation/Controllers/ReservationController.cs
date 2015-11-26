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


        public ActionResult BuscarVuelos(AirlineReservation.Models.Reservacion reservacion, int pagina)
        {
            ViewBag.cliente = reservacion.PasajeroID;
            ViewBag.agente = reservacion.AgenciaID;
            return View(reservacion.VuelosPosibles(pagina, reservacion.AeropuertoOrigenID, reservacion.AeropuertoDestinoID));
        }

        [HttpPost]
        public ActionResult Reservar(int IDagent, int IDpassenger, int IDflightinstance, int IDAircraft)
        {
            AirlineReservation.Models.Reservacion reserva = new Models.Reservacion();

            DateTime hora = DateTime.Now;
            String respuesta = reserva.Reservar(hora,IDagent,IDpassenger,IDflightinstance,IDAircraft);

            ViewBag.Message = respuesta;


            return View("Index" , new AirlineReservation.Models.Reservacion());
        }

      
    }
}
