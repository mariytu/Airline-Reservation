CREATE OR REPLACE FUNCTION ValidarFechas()
RETURNS TRIGGER AS $validar_fechas$
BEGIN
	IF (TG_OP = 'UPDATE') THEN -- Validacion del evento que invoco
        IF (NEW."estimatedDeparture" = OLD."estimatedDeparture") THEN -- No cambio la fecha del vuelo
			RETURN NEW;
	END IF;
	
	IF (NEW."estimatedDeparture" >= (SELECT now()+'3 day'::interval)) THEN -- Validacion de la regla de negocio
		RETURN NEW;
	END IF;
	
	RAISE EXCEPTION 'La fecha estimada de partida debe ser superior a ' || (SELECT now()+'3 day'::interval);
	RETURN NULL;
END;
$validar_fechas$ LANGUAGE plpgsql;

CREATE TRIGGER  validar_fechas
BEFORE INSERT OR UPDATE ON "FlightInstance"
	FOR EACH ROW EXECUTE PROCEDURE ValidarFechas();