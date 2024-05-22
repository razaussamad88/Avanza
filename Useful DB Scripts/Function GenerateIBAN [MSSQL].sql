-- Drop FUNCTION GEN_IBAN
CREATE FUNCTION GEN_IBAN
(
    @accountNum VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN 
	Declare @var1 VARCHAR(MAX)

	-- ALPHABET MAP
	--	N R S P : 23 27 28 25 (23272825)
	--	P K 0 0 : 25 20 00 (252000)
    Select @var1 = concat('23272825', dbo.LPAD(@accountNum, 16, '0'), '252000')
	
	return concat('PK', dbo.mod_97_10_check(@var1), 'NRSP',  dbo.LPAD(@accountNum, 16, '0'));

END
GO





CREATE FUNCTION LPAD
(
    @string VARCHAR(MAX), -- Initial string
    @length INT,          -- Size of final string
    @pad CHAR             -- Pad character
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    RETURN REPLICATE(@pad, @length - LEN(@string)) + @string;
END
GO


-- Drop FUNCTION mod_97_10_check
CREATE FUNCTION mod_97_10_check(@str VARCHAR(MAX)) 
RETURNS VARCHAR(2)
AS
BEGIN
	return dbo.LPAD(98 - (cast(@str as decimal(69)) % 97), 2, '0');
END
GO

