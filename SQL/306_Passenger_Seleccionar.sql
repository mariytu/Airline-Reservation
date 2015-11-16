DROP FUNCTION IF EXISTS Passenger_Seleccionar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Selecciona un passenger
-- =============================================

CREATE OR REPLACE FUNCTION Passenger_Seleccionar (inID integer)
RETURNS SETOF "Passengers" AS
$BODY$ DECLARE
	sql TEXT;
BEGIN
	sql = 'SELECT
		"passengerID"	as Passenger_ID,
		"firstName"		as Passenger_FirstName,
		"secondName"	as Passenger_SecondName,
		"lastName"		as Passenger_LastName,
		"phoneNumber"	as Passenger_PhoneNumber,
		"addressLine"	as Passenger_AddressLine,
		"emailAddress"	as Passenger_EmailAddress,
		"cityID"		as Passenger_CityID
	FROM
		"Passengers"
	WHERE
		"passengerID" = ' || inID;
	
	RETURN QUERY EXECUTE sql;
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$