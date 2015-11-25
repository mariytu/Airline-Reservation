DROP FUNCTION IF EXISTS FlightInstance_Todos () $$

CREATE OR REPLACE FUNCTION FlightInstance_Todos (inIndex integer, inNext integer)
RETURNS SETOF "FlightInstance" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT
		"flightInstanceID"		as Flight_Instance_ID,
		"cost"					as Cost,
		"flightNumber"			as Flight_Number,
		"estimatedDeparture"	as Estimated_Departure,
		"estimatedArrival"		as Estimated_Arrival
	FROM
		"FlightInstance"
	WHERE "FlightInstance".state =3
	ORDER BY "flightInstanceID" LIMIT ' || inNext || ' OFFSET ' || inIndex;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$