-- Consultas de prueba
-- No se puede realizar un pago trascurridos 3 dias desde que se creo la reserva
INSERT INTO "Payment"("paymentDate", "paymentAmount", "paymentTypeID")
VALUES ((SELECT now()), (SELECT now()+'12 hours'::interval), 100, 2);

INSERT INTO "Payment"("paymentDate", "paymentAmount", "paymentTypeID")
VALUES ((SELECT now()), (SELECT now()+'4 day'::interval), 100, 2);