DROP FUNCTION IF EXISTS Passenger_Count () $$

CREATE OR REPLACE FUNCTION Passenger_Count (flightInstanceID integer)
RETURNS integer AS
$BODY$BEGIN
  	RETURN
  	(
		SELECT COUNT(*)
		FROM "FlightReservation", "FlightInstance"
		WHERE "FlightReservation"."flightInstanceID" = flightInstanceID AND 
		"FlightReservation"."flightInstanceID" = "FlightInstance"."flightInstanceID"
	);
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$