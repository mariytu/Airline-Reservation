DROP FUNCTION IF EXISTS ItineraryReservation_CheckIn () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 18-11-2015
-- Descripcion	: Realiza el check-in de una reserva de vuelo
-- =============================================

CREATE OR REPLACE FUNCTION ItineraryReservation_CheckIn ( inID integer)
RETURNS void AS

$BODY$BEGIN
		IF (SELECT COUNT(*) 
			FROM "ItineraryReservation"
			LEFT JOIN "ReservationState" ON "ReservationState"."reservationID" = "ItineraryReservation"."reservationState"
			WHERE   "ItineraryReservation"."reservationID" = inID AND
					"ReservationState"."reservationName" = 'Reserved'
			) = 1 THEN
		    IF (SELECT  "Payment"."paymentAmount"
				FROM "ItineraryReservation"
				LEFT JOIN "Payment" ON "Payment"."paymentID" = "ItineraryReservation"."paymentID"
				WHERE   "ItineraryReservation"."reservationID" = inID
			) > 0 THEN
				UPDATE "ItineraryReservation" SET "reservationState" = 2 WHERE "ItineraryReservation"."reservationID" = inID;
		    	--RAISE 'Check-in realizado exitosamente';
			ELSE
				RAISE 'No se pudo realizar Check-in, no ha pagado su Itinerary Reservation';
			END IF;
		ELSE
			RAISE 'Ya se realizo un Check-in';
		END IF;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$