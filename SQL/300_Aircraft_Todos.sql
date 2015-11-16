DROP FUNCTION IF EXISTS Aircraft_Todos () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona todos los aircraft
-- =============================================

CREATE OR REPLACE FUNCTION Aircraft_Todos (inIndex integer, inNext integer)
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
	ORDER BY "aircraftID" LIMIT ' || inNext || ' OFFSET ' || inIndex;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$