

CREATE VIEW [dbo].[vw_getRANDValue]
AS
	SELECT RAND() AS Value
GO




Create FUNCTION	[dbo].[fnGetRandomRange]
(
	@start INT,
	@end INT
)
RETURNS INT
AS

BEGIN

	DECLARE	@iNum INT

	SELECT	@iNum = FLOOR( (SELECT Value FROM vw_getRANDValue) * (@end - @start + 1 ) ) + @start

	RETURN	@iNum
END
GO


