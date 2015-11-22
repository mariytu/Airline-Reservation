CREATE OR REPLACE FUNCTION ValidarMinimo()
RETURNS TRIGGER AS $validar_minimo$ DECLARE
	capacidad "Aircraft"."aircraftCapacity"%TYPE;
	pasajeros integer;
BEGIN
	
	IF (TG_OP = 'UPDATE') THEN -- Validacion del evento que invoco
        IF (NEW."state" = OLD."state") THEN -- No cambio el estado del vuelo
			RETURN NEW;
		ELSIF (NEW."state" <> 1) THEN -- No ha cambiado el estado a In Flight
			RETURN NEW;
		END IF;
	ELSIF (TG_OP = 'INSERT') THEN
		IF (NEW."state" <> 1) THEN -- No ha cambiado el estado a In Flight
			RETURN NEW;
		END IF;
	END IF;
	
	-- Capacidad del avión!!!
	SELECT	"Aircraft"."aircraftCapacity" INTO capacidad 
	FROM	"FlightInstance", "Aircraft" 
	WHERE	"FlightInstance"."flightInstanceID" = NEW."flightInstanceID" AND 
			"FlightInstance"."aircraftID" = "Aircraft"."aircraftID"
	
	-- Contar los pasajeros que hay en el vuelo
	SELECT COUNT(*) INTO pasajeros
	FROM "FlightReservation" 
	WHERE "FlightReservation"."flightInstanceID" = NEW."flightInstanceID"
	
	IF ((SELECT pasajeros*100/capacidad) > 10) THEN -- Validacion de la regla de negocio
		RETURN NEW;
	END IF;
	
	RAISE EXCEPTION 'Este vuelo se debe cancelar.';
	RETURN NULL;
END;
$validar_minimo$ LANGUAGE plpgsql;

CREATE TRIGGER  validar_minimo
BEFORE INSERT OR UPDATE ON "FlightInstance"
	FOR EACH ROW EXECUTE PROCEDURE ValidarMinimo();