CREATE OR REPLACE FUNCTION ValidarCiudad()
RETURNS TRIGGER AS $validar_ciudad$
BEGIN
	
	IF (NEW."originAirport" <> NEW."destinyAirport") THEN -- Validacion de la regla de negocio
		RETURN NEW;
	END IF;
	
	RAISE EXCEPTION 'Los aeropuertos de origen y destino deben ser diferentes.';
	RETURN NULL;
END;
$validar_ciudad$ LANGUAGE plpgsql;

CREATE TRIGGER  validar_ciudad
BEFORE INSERT OR UPDATE ON "FlightSchedules"
	FOR EACH ROW EXECUTE PROCEDURE ValidarCiudad();