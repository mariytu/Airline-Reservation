-- Consultas de prueba
-- No se puede crear el vuelo por problemas con la fecha estimada de partida.
INSERT INTO "FlightInstance"("state", "cost", "flightNumber", "estimatedDeparture", "estimatedArrival", "aircraftID")
VALUES (3, 550000, 10, (SELECT now()), (SELECT now()+'12 hours'::interval), 10);
	
-- Se puede crear la instancia de vuelo
INSERT INTO "FlightInstance"("state", "cost", "flightNumber", "estimatedDeparture", "estimatedArrival", "aircraftID")
VALUES (3, 550000, 10, (SELECT now()+'4 day'::interval), (SELECT (SELECT now()+'4 day'::interval)+'12 hours'::interval), 10);

-- Para obtener el ultimo de la tabla
SELECT *
FROM "FlightInstance"
ORDER BY "flightInstanceID" DESC LIMIT 1;