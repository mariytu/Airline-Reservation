-- Consultas de prueba
-- Mismo aeropuerto
INSERT INTO "FlightSchedules"("departureTime", "flightTime", "realCost", "aircraftCode", "originAirport", "destinyAirport")
VALUES ('10:46:00', 6, 300000, 100, 2887, 2887);
	
-- Diferentes aeropuertos, misma ciudad
INSERT INTO "FlightSchedules"("departureTime", "flightTime", "realCost", "aircraftCode", "originAirport", "destinyAirport")
VALUES ('10:46:00', 6, 300000, 100, 7262, 8912);

-- Diferentes aeropuertos y diferentes ciudades
INSERT INTO "FlightSchedules"("departureTime", "flightTime", "realCost", "aircraftCode", "originAirport", "destinyAirport")
VALUES ('10:46:00', 6, 300000, 100, 7262, 8000);

-- Para obtener el ultimo de la tabla
SELECT *
FROM "FlightSchedules"
ORDER BY "flightNumberID" DESC LIMIT 1;