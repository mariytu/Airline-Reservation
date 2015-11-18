DROP FUNCTION IF EXISTS Passenger_Count () $$

CREATE OR REPLACE FUNCTION Passenger_Count (flightInstanceID integer)
RETURNS integer AS
$BODY$BEGIN
  	RETURN
  	(
		SELECT COUNT(*)
		FROM "FlightReservation"
		WHERE "FlightReservation"."flightInstanceID" = flightInstanceID
	);
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$