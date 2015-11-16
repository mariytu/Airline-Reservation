DROP FUNCTION IF EXISTS Aircraft_Seleccionar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona un aircraft
-- =============================================

CREATE OR REPLACE FUNCTION Aircraft_Seleccionar (inID integer)
RETURNS SETOF "Aircraft" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT
		"aircraftID"		as Aircraft_ID,
		"aircraftName"		as Aircraft_Name,
		"aircraftCapacity"	as Aircraft_Capacity,
		"aircraftCode"		as Aircraft_Code
	FROM
		"Aircraft"
	WHERE
		"aircraftID" = ' || inID;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$