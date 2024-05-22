

-- select compatibility_level from sys.databases where name = 'VISIONWEB_VIGILUS'
ALTER DATABASE VISIONWEB_VIGILUS SET COMPATIBILITY_LEVEL = 110
GO

-- ************************************************************************************



-- Running the following on the DB worked for me:
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION=ON

-- Then, after the update, setting it back using:
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION=OFF



-- ************************************************************************************
