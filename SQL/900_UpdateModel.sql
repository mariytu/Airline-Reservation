-- Ejemplo para agregar una columna a una tabla utilizando SQL dinamico
-- =============================================
-- Autor		: miturriaga
-- Creacion		: 01-11-2015
-- Descripcion	: Agrega la columna Institucion en la tabla Participante
-- =============================================
-- SET @addInstitucionOnParticipante = (SELECT IF(
--     (SELECT COUNT(*)
--         FROM INFORMATION_SCHEMA.COLUMNS
--         WHERE  TABLE_NAME = 'Participante'
--         AND TABLE_SCHEMA = DATABASE()
--         AND COLUMN_NAME = 'Institucion'
--     ) > 0,
--     "SELECT 1",
--     "ALTER TABLE `Participante` ADD `Institucion` text"
-- )) $$
-- PREPARE addInstitucion FROM @addInstitucionOnParticipante $$
-- EXECUTE addInstitucion $$
-- DEALLOCATE PREPARE addInstitucion $$


-- Ejemplo para eliminar una columna a una tabla utilizando SQL dinamico
-- =============================================
-- Autor		: miturriaga
-- Creacion		: 01-11-2015
-- Descripcion	: Elimina la columna Institucion en la tabla Participante
-- =============================================
-- SET @dropInstitucionOnParticipante = (SELECT IF(
--     (SELECT COUNT(*)
--         FROM INFORMATION_SCHEMA.COLUMNS
--         WHERE  TABLE_NAME = 'Participante'
--         AND TABLE_SCHEMA = DATABASE()
--         AND COLUMN_NAME = 'Institucion'
--     ) > 0,
--     "ALTER TABLE `Participante` DROP `Institucion`",
--     "SELECT 1"
-- )) $$
-- PREPARE dropInstitucion FROM @dropInstitucionOnParticipante $$
-- EXECUTE dropInstitucion $$
-- DEALLOCATE PREPARE dropInstitucion $$