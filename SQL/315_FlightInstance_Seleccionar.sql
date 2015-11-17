DROP FUNCTION IF EXISTS FlightInstance_Seleccionar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona un flight instance
-- =============================================

CREATE OR REPLACE FUNCTION FlightInstance_Seleccionar (inID integer)
RETURNS SETOF "FlightInstance" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT *
	FROM
		"FlightInstance"
	WHERE
		"flightInstanceID" = ' || inID;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$