DROP FUNCTION IF EXISTS Country_Seleccionar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona un país
-- =============================================

CREATE OR REPLACE FUNCTION Country_Seleccionar (inID integer)
RETURNS SETOF "Country" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT
		"countryID"		as Country_ID,
		"countryName"	as Country_Name
	FROM
		"Country"
	WHERE
		"countryID" = ' || inID;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$