DROP FUNCTION IF EXISTS Aircraft_Modificar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Modifica un aircraft existente
-- =============================================

CREATE OR REPLACE FUNCTION Aircraft_Modificar (
	IN inID integer,
	IN inName character varying(55),
	IN inCapacity integer,
	IN inCode character varying(3)
)
RETURNS void AS
$BODY$ BEGIN

	UPDATE "Aircraft" 
	SET
		"aircraftName" = inName,
		"aircraftCapacity" = inCapacity,
		"aircraftCode" = inCode
	WHERE inID = "aircraftID";
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$