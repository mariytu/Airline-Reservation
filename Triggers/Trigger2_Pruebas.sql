-- Consultas de prueba
-- No se puede realizar un pago trascurridos 3 dias desde que se creo la reserva
INSERT INTO "Payment"("paymentDate", "paymentAmount", "paymentTypeID")
VALUES ((SELECT now()), 100, 2);

UPDATE "Payment"
SET "paymentDate"= (SELECT now()) ,"paymentAmount"= 300
WHERE "Payment"."paymentID" = ID_INSTANCE;

SELECT *
FROM "Payment"
WHERE "Payment"."paymentID" = ID_INSTANCE;
