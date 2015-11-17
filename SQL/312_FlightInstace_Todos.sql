DROP FUNCTION IF EXISTS FlightInstace_Todos () $$

CREATE OR REPLACE FUNCTION FlightInstace_Todos (inIndex integer, inNext integer)
RETURNS SETOF "FlightInstance" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT *
	FROM
		"FlightInstance"
	ORDER BY "flightInstanceID" LIMIT ' || inNext || ' OFFSET ' || inIndex;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$