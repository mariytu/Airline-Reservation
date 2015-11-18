DROP FUNCTION IF EXISTS ItineraryReservation_CheckIn () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 18-11-2015
-- Descripcion	: Realiza el check-in de una reserva de vuelo
-- =============================================

CREATE OR REPLACE FUNCTION ItineraryReservation_CheckIn (
	IN inID integer
)
RETURNS void AS
$BODY$ BEGIN

	-- IMPLEMENTACION DEL SP INCLUIR EXCEPTION

--EXCEPTION

--WHEN SQLSTATE '23503' THEN  
	--RAISE EXCEPTION 'No se pudo borrar el  avion, ya que tiene vuelos asociados a el ';
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$