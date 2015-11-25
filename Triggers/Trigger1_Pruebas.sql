-- Consultas de prueba
UPDATE "ItineraryReservation"
SET "reservationState" = 2 -- 2 es el ID del estado check-in.
WHERE "reservationID" = ID_RESERVA; -- Este es el ID de la reserva que vamos a actualizar.

SELECT 	"ItineraryReservation"."reservationID" as "ID Reserva",
	"ReservationState"."reservationName" as "Estado Reserva",
	"FlightInstance"."estimatedDeparture" as "Fecha de Partida",
	now() as "Fecha de Hoy"
FROM 	"ItineraryReservation", "ReservationState", "FlightReservation", "FlightInstance"
WHERE 	"ItineraryReservation"."reservationID" = ID_RESERVA AND
	"ItineraryReservation"."reservationState" = "ReservationState"."reservationID" AND
	"ItineraryReservation"."reservationID" = "FlightReservation"."reservationID" AND
	"FlightReservation"."flightInstanceID" = "FlightInstance"."flightInstanceID";

-- Instancias de Pruebas
-- Fecha del vuelo: "2015-11-26 12:15:00"	reservationID => 87955, 87987, 88049		-- Dependiendo de la hora del jueves debiese permitir check-in
-- Fecha del vuelo: "2015-11-26 16:46:00"	reservationID => 88110, 88125, 88132		-- Debiese permitir check-in
-- Fecha del vuelo: "2015-11-26 10:33:00"	reservationID => 85909, 85914, 85920		-- Perdio el vuelo
-- Fecha del vuelo: "2015-11-27 23:17:00"	reservationID => 134237, 134243, 134251		-- No debiese permitir el check-in
-- Fecha del vuelo: "2015-11-28 15:20:00"	reservationID => 176540, 176545, 176605		-- No debiese permitir el check-in

-- Para encontrar algún vuelo por la fecha
-- SELECT "flightInstanceID","estimatedDeparture"
-- FROM "FlightInstance"
-- WHERE "flightInstanceID">1000 AND "flightInstanceID"<1500;

-- Teniendo el ID del vuelo, encontrar las reservas asociadas a ese vuelo
-- SELECT "reservationID","flightInstanceID"
-- FROM "FlightReservation"
-- WHERE "flightInstanceID" = 1015;