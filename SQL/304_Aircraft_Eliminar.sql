DROP FUNCTION IF EXISTS Aircraft_Eliminar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Elimina un aircraft existente por el ID
-- =============================================

CREATE OR REPLACE FUNCTION Aircraft_Eliminar (
	IN inID integer
)
RETURNS void AS
$BODY$ BEGIN

	DELETE FROM "Aircraft" 
	WHERE inID = "aircraftID";
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$