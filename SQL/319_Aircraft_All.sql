DROP FUNCTION IF EXISTS Aircraft_All () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 18-11-2015
-- Descripcion	: Selecciona todos los aircraft
-- =============================================

CREATE OR REPLACE FUNCTION Aircraft_All ()
RETURNS SETOF "Aircraft" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT * 
	FROM
		"Aircraft"';
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$