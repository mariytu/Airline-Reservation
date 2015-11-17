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

EXCEPTION

WHEN SQLSTATE '23503' THEN  
	RAISE EXCEPTION 'No se pudo borrar el  avion, ya que tiene vuelos asociados a el ';

	
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$