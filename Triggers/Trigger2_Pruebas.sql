-- Consultas de prueba
-- No se puede realizar un pago trascurridos 3 dias desde que se creo la reserva
UPDATE "Payment"
SET "paymentAmount"= 300
WHERE "Payment"."paymentID" = ID_INSTANCE;

-- Consulta de actualización del pago con fecha
UPDATE "Payment"
SET "paymentDate"= (SELECT now()) ,"paymentAmount"= 300
WHERE "Payment"."paymentID" = ID_INSTANCE;

-- Consulta para verificar la actualización del pago
SELECT *
FROM "Payment"
WHERE "Payment"."paymentID" = ID_INSTANCE;

--Consulta para validar fechas
SELECT "Payment"."paymentDate", "ItineraryReservation"."dateReservationMade"
FROM "Payment", "ItineraryReservation"
WHERE "Payment"."paymentID" = ID_INSTANCE AND "ItineraryReservation"."paymentID" = ID_INSTANCE;
