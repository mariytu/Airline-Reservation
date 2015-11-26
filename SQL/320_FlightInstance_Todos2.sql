DROP FUNCTION IF EXISTS flightinstance_todos_reserva () $$

CREATE OR REPLACE FUNCTION flightinstance_todos_reserva (inIndex integer, inNext integer, origen bigint, destino bigint)
RETURNS TABLE (
	"flightInstanceID" bigint,
	"state" bigint,
	"cost" integer,
	"flightNumber" bigint,
	"realDeparture" timestamp without time zone,
	"realArrival" timestamp without time zone,
	"estimatedDeparture" timestamp without time zone,
	"estimatedArrival" timestamp without time zone,
	"aircraftID" bigint
) AS 
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = ' SELECT 
				"aux"."flightInstanceID",			
				"aux"."state",			
				"aux"."cost",		
				"aux"."flightNumber",			
				"aux"."realDeparture",			
				"aux"."realArrival",			
				"aux"."estimatedDeparture",			
				"aux"."estimatedArrival",			
				"aux"."aircraftID"

			FROM (SELECT *

				FROM "FlightInstance"

				LEFT JOIN "FlightSchedules" on "FlightSchedules"."flightNumberID"="FlightInstance"."flightNumber"
				LEFT JOIN "Airport" on "Airport"."airportID" ="FlightSchedules"."originAirport"
				WHERE "Airport"."cityID"='|| origen ||') aux

			LEFT JOIN "Airport" on "Airport"."airportID" ="aux"."destinyAirport"
			WHERE "Airport"."cityID"=' || destino || ' AND "aux".state =3
			ORDER BY "flightInstanceID" LIMIT ' || inNext || ' OFFSET ' || inIndex;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$