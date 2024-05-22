
IF OBJECT_ID('helper_SP_DELETE_PERMISSIONS', 'P') IS NOT NULL
	Drop PROCEDURE helper_SP_DELETE_PERMISSIONS
GO


CREATE PROCEDURE helper_SP_DELETE_PERMISSIONS(@rAppID varchar(50), @rGroupID varchar(50)) as 
BEGIN
SET NOCOUNT ON;

	DECLARE @tmpGroupID varchar(50),
			@tmpAppID varchar(50)


	SELECT @tmpGroupID = ISNULL(lower(GROUP_ID), '') 
	FROM SEC_GROUP WHERE GROUP_ID = @rGroupID; 
	

	SELECT @tmpAppID = ISNULL(lower(APP_ID), '')
	FROM SEC_APPLICATION WHERE APP_ID = @rAppID; 

	IF LEN(@tmpGroupID) <= 0 OR LEN(@tmpAppID) <= 0
	BEGIN
		RAISERROR (
			N'GroupID or AppID is invalid.', -- Message text.
			10, -- Severity,
			1 -- State,
		);
	END

	/** Delete Group-Wise Permission **/

	Delete From SEC_GROUP_PERMISSION 
	Where GROUP_ID = @tmpGroupID;
	/** Delete Group-Wise Permission **/

	

	/** Delete Permission Map **/

	Delete From SEC_APP_PERMISSION_MAP 
	Where	APP_ID = @tmpAppID
	AND		PERMISSION_ID in (
				Select PERMISSION_ID 
				FROM SEC_GROUP_PERMISSION 
				where lower(GROUP_ID) = @tmpGroupID
		);
	/** Delete Permission Map **/
	
END
-- exec helper_SP_DELETE_PERMISSIONS 'backoffice', 'admin'
