CREATE OR REPLACE FUNCTION ValidarReservaImpaga()
RETURNS TRIGGER AS $validar_reserva_impaga$ DECLARE
	fecha "Payment"."paymentDate"%TYPE;

BEGIN
	IF (TG_OP = 'UPDATE') THEN -- Validacion del evento que invoco
        IF (NEW."paymentDate" = OLD."paymentDate") THEN -- No cambio el estado de la reserva
			--IF (NEW."paymentAmount" > 0) THEN
				RETURN NEW;
		ELSIF ((NEW."paymentDate" - OLD."paymentDate") <= '3 day'::interval) THEN 
			RETURN NEW;
		END IF;
	END IF;
	
	-- Fecha de la reserva
	SELECT	"ItineraryReservation"."dateReservationMade" INTO fecha 
	FROM	"ItineraryReservation"
	WHERE	"ItineraryReservation"."paymentID" = NEW."paymentID";

	IF ((SELECT now()) <= (SELECT fecha + '3 day'::interval)) THEN
	--IF ((SELECT fechaReserva - NEW."paymentDate") <= 3) THEN -- Validacion de la regla de negocio
		IF (NEW."paymentAmount" > 0) THEN
			RETURN NEW;
		END IF;
	END IF;

	RAISE EXCEPTION 'La fecha de pago ha expirado';
	RETURN NULL;
END;
$validar_reserva_impaga$ LANGUAGE plpgsql;

CREATE TRIGGER  validar_reserva_impaga
BEFORE INSERT OR UPDATE ON "Payment"
	FOR EACH ROW EXECUTE PROCEDURE ValidarReservaImpaga();