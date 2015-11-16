DROP FUNCTION IF EXISTS Aircraft_Crear () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Crea un aircraft nuevo
-- =============================================

CREATE OR REPLACE FUNCTION Aircraft_Crear (
	OUT outID integer,
	IN inName character varying(55),
	IN inCapacity integer,
	IN inCode character varying(3)
)
RETURNS integer AS
$BODY$ BEGIN

	INSERT INTO "Aircraft"
	(
		"aircraftName",
		"aircraftCapacity",
		"aircraftCode"
	)
	VALUES
	(
		inName,
		inCapacity,
		inCode
	);

	outID = lastval();
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$