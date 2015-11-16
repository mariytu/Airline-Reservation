DROP FUNCTION IF EXISTS Passenger_Modificar () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Modifica un passenger existente
-- =============================================

CREATE OR REPLACE FUNCTION Passenger_Modificar (
	IN inID integer,
	IN inFirstName character varying(30),
	IN inSecondName character varying(30),
	IN inLastName character varying(30),
	IN inPhoneNumber integer,
	IN inAddressLine character varying(40),
	IN inEmailAddress character varying(45),
	IN inCityID integer
)
RETURNS void AS
$BODY$ BEGIN

	UPDATE "Passengers" 
	SET
		"firstName" = inFirstName,
		"secondName" = inSecondName,
		"lastName" = inLastName,
		"phoneNumber" = inPhoneNumber,
		"addressLine" = inAddressLine,
		"emailAddress" = inEmailAddress,
		"cityID" = inCityID
	WHERE inID = "passengerID";
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$