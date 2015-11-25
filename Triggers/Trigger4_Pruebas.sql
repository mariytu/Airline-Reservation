-- Consulta de actualización del estado de la instancia de vuelo
UPDATE "FlightInstance"
SET "state"=3
WHERE "flightInstanceID" = ID_INSTANCE;

-- Consulta para verificar el estado de la instancia de vuelo
SELECT 	"FlightInstance"."flightInstanceID", "FlightState"."stateName"
FROM 	"FlightInstance", "FlightState"
WHERE 	"FlightInstance"."flightInstanceID" = ID_INSTANCE AND
	"FlightInstance"."state" = "FlightState"."flightStateID"

-- Instancias de Pruebas
-- InstanceID	Capacidad	Pasajeros	Porcentaje	Respuesta
--		1		320				96			30%
--		2		200				60			30%		
--		11		310				30			9,7%	Cancelar vuelo
--		16424	400				30			7,5%	Cancelar vuelo
-- Fecha del vuelo: "2015-11-26 12:15:00"	reservationID => 87955, 87987, 88049		-- Dependiendo de la hora del jueves debiese permitir check-in
-- Fecha del vuelo: "2015-11-26 16:46:00"	reservationID => 88110, 88125, 88132		-- Debiese permitir check-in
-- Fecha del vuelo: "2015-11-26 10:33:00"	reservationID => 85909, 85914, 85920		-- Perdio el vuelo
-- Fecha del vuelo: "2015-11-27 23:17:00"	reservationID => 134237, 134243, 134251		-- No debiese permitir el check-in
-- Fecha del vuelo: "2015-11-28 15:20:00"	reservationID => 176540, 176545, 176605		-- No debiese permitir el check-in