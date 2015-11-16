DROP FUNCTION IF EXISTS Passenger_Crear () $$

-- =============================================
-- Autor		: miturriaga
-- Creacion		: 16-11-2015
-- Descripcion	: Crea un passenger nuevo
-- =============================================

CREATE OR REPLACE FUNCTION Passenger_Crear (
	OUT outID integer,
	IN inFirstName character varying(30),
	IN inSecondName character varying(30),
	IN inLastName character varying(30),
	IN inPhoneNumber integer,
	IN inAddressLine character varying(40),
	IN inEmailAddress character varying(45),
	IN inCityID integer
)
RETURNS integer AS
$BODY$ BEGIN

	INSERT INTO "Passengers"
	(
		"firstName",
		"secondName",
		"lastName",
		"phoneNumber",
		"addressLine",
		"emailAddress",
		"cityID"
	)
	VALUES
	(
		inFirstName,
		inSecondName,
		inLastName,
		inPhoneNumber,
		inAddressLine,
		inEmailAddress,
		inCityID
	);

	outID = lastval();
	
END;
$BODY$ LANGUAGE 'plpgsql' VOLATILE
COST 100 $$