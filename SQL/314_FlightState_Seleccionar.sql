DROP FUNCTION IF EXISTS FlightState_Seleccionar () $$

CREATE OR REPLACE FUNCTION FlightState_Seleccionar (inID integer)
RETURNS SETOF "FlightState" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT *
	FROM
		"FlightState"
	WHERE
		"flightStateID" = ' || inID ;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$