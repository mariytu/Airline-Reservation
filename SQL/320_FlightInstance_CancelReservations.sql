DROP FUNCTION IF EXISTS FlightInstance_CancelReservations () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 18-11-2015
-- Descripcion	: Cancela algunas reservas de vuelo
-- =============================================

CREATE OR REPLACE FUNCTION FlightInstance_CancelReservations (inID integer, inTotal integer)
RETURNS void AS
$BODY$ DECLARE 
	sql TEXT;
	r "FlightReservation"%rowtype;
BEGIN
	
	sql = 'SELECT * FROM "FlightReservation" WHERE "flightInstanceID" = ' || inID || ' LIMIT ' || inTotal;
	
	FOR r IN EXECUTE(sql)
	LOOP
		DELETE FROM "FlightReservation" 
		WHERE r.reservationID = "reservationID" AND "flightInstanceID" = inID;
		
		UPDATE "ItineraryReservation" 
		SET "reservationState" = 4 
		WHERE "reservationID" = r.reservationID;
		
	END LOOP;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$