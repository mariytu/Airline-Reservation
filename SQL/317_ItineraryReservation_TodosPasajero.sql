DROP FUNCTION IF EXISTS ItineraryReservation_TodosPasajero () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona todos los itinerary reservations de un pasajero
-- =============================================

CREATE OR REPLACE FUNCTION ItineraryReservation_TodosPasajero (inID integer)
RETURNS TABLE (
	"reservationID" bigint,
	"dateReservationMade" timestamp without time zone,
	"agentID" bigint,
	"passengerID" bigint,
	"reservationState" bigint,
	"paymentID" bigint,
	"estimatedDeparture" timestamp without time zone,
	"paymentDate" timestamp without time zone,
	"paymentAmount" integer
) AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT 
		"ItineraryReservation"."reservationID",
		"ItineraryReservation"."dateReservationMade",
		"ItineraryReservation"."agentID",
		"ItineraryReservation"."passengerID",
		"ItineraryReservation"."reservationState",
		"ItineraryReservation"."paymentID",
		"FlightInstance"."estimatedDeparture",
		"Payment"."paymentDate",
		"Payment"."paymentAmount"
	FROM
		"ItineraryReservation", "Payment", "FlightReservation", "FlightInstance"
	WHERE "ItineraryReservation"."passengerID" = ' || inID || ' AND 
		"ItineraryReservation"."paymentID" = "Payment"."paymentID" AND 
		"FlightReservation"."reservationID" = "ItineraryReservation"."reservationID" AND 
		"FlightReservation"."flightInstanceID" = "FlightInstance"."flightInstanceID"';
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$