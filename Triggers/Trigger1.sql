CREATE OR REPLACE FUNCTION CheckIn()
RETURNS TRIGGER AS $check_in$ DECLARE
	fecha "FlightInstance"."estimatedDeparture"%TYPE;
BEGIN

	IF (TG_OP = 'UPDATE') THEN -- Validacion del evento que invoco
        IF (NEW."reservationState" = OLD."reservationState") THEN -- No cambio el estado de la reserva
			RETURN NEW;
		ELSIF (NEW."reservationState" <> 2) THEN -- No ha cambiado el estado a check-in
			RETURN NEW;
		END IF;
	ELSIF (TG_OP = 'INSERT') THEN
		IF (NEW."reservationState" <> 2) THEN -- No ha cambiado el estado a check-in
			RETURN NEW;
		END IF;
	END IF;
	
	-- Obtener fecha estimada de partida del primer vuelo de la reserva
	SELECT	"FlightInstance"."estimatedDeparture" INTO fecha 
	FROM	"ItineraryReservation", "FlightReservation", "FlightInstance"
	WHERE	"ItineraryReservation"."reservationID" = NEW."reservationID" AND 
			"ItineraryReservation"."reservationID" = "FlightReservation"."reservationID" AND 
			"FlightReservation"."number" = 1 AND 
			"FlightReservation"."flightInstanceID" = "FlightInstance"."flightInstanceID" LIMIT 1;
	
	IF ((SELECT now()) >= (SELECT fecha-'1 day'::interval) AND (SELECT now()) <= (SELECT fecha-'2 hours'::interval)) THEN -- Validacion de la regla de negocio
		RETURN NEW;
	END IF;
	
	RAISE EXCEPTION 'Todavia no puede hacer check-in';
	RETURN NULL;
END;
$check_in$ LANGUAGE plpgsql;

CREATE TRIGGER  check_in
BEFORE INSERT OR UPDATE ON "ItineraryReservation"
	FOR EACH ROW EXECUTE PROCEDURE CheckIn();