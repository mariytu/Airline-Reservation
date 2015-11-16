DROP FUNCTION IF EXISTS City_Todos () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona todos los passenger
-- =============================================

CREATE OR REPLACE FUNCTION City_Todos ()
RETURNS SETOF "City" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT *
	FROM
		"City"';
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$