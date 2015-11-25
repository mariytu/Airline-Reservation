CREATE OR REPLACE FUNCTION ValidarCiudad()
RETURNS TRIGGER AS $validar_ciudad$
BEGIN
	
	IF (NEW."originAirport" <> NEW."destinyAirport") THEN -- Validacion de la regla de negocio
		IF (SELECT "cityID" FROM "Airport" WHERE "airportID" = new."originAirport") <> 
			(SELECT "cityID" FROM "Airport" WHERE "airportID" = new."destinyAirport") THEN
			RETURN NEW;
		END IF;
		
		RAISE EXCEPTION 'Los aeropuertos deben ser de ciudades distintas.';
		RETURN NULL;
	END IF;
	
	RAISE EXCEPTION 'Los aeropuertos de origen y destino deben ser diferentes.';
	RETURN NULL;
END;
$validar_ciudad$ LANGUAGE plpgsql;

CREATE TRIGGER  validar_ciudad
BEFORE INSERT OR UPDATE ON "FlightSchedules"
	FOR EACH ROW EXECUTE PROCEDURE ValidarCiudad();