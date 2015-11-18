using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineReservation.Controllers
{
    public class ItineraryReservationController : Controller
    {
        // GET: ItineraryReservation/PassengerReservations/5
        public ActionResult PassengerReservations(long id)
        {
            if (TempData["shortMessage"] != null)
            {
                @ViewBag.Message = TempData["shortMessage"].ToString();
            }

            var reservations = Models.ItineraryReservation.TodosPasajero(id);
            return View(reservations);
        }

        /// <summary>
        /// POST /ItineraryReservation/CheckInSP/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInSP(long id)
        {
            var reservation = new Models.ItineraryReservation();

            if (reservation.Seleccionar(id))
            {
                String mensaje = reservation.CheckInSP();
                TempData["shortMessage"] = mensaje;
            }

            return RedirectToAction("PassengerReservations", new { id = 1 });
        }

        /// <summary>
        /// POST /ItineraryReservation/CheckInTrans/5
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInTrans(long id)
        {
            var reservation = new Models.ItineraryReservation();

            if (reservation.Seleccionar(id))
            {
                String mensaje = reservation.CheckInTrans();
                TempData["shortMessage"] = mensaje;
            }

            return RedirectToAction("PassengerReservations", new { id = 1 });
        }
    }
}