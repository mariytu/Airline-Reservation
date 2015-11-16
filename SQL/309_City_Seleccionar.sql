DROP FUNCTION IF EXISTS City_Seleccionar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona una ciudad
-- =============================================

CREATE OR REPLACE FUNCTION City_Seleccionar (inID integer)
RETURNS SETOF "City" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT
		"cityID"	as City_ID,
		"cityName"	as City_Name,
		"timeZone"	as City_TimeZone,
		"countryID"	as City_CountryID
	FROM
		"City"
	WHERE
		"cityID" = ' || inID;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$