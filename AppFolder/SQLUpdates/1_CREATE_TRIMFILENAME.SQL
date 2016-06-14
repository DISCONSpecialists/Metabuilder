
create  FUNCTION [dbo].[TrimFileName]
(
	-- Add the parameters for the function here
	@Filename varchar(255)
)
RETURNS varchar(255)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar varchar(255)

	-- Add the T-SQL statements to compute the return value here
	SELECT @ResultVar = SUBSTRING(@Filename, LEN(@Filename) - CHARINDEX('\', REVERSE(@Filename)) + 2, CHARINDEX('\', REVERSE(@Filename)) - 1)


	-- Return the result of the function
	RETURN @ResultVar

END
