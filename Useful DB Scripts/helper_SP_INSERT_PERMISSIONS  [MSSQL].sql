

IF OBJECT_ID('helper_SP_INSERT_PERMISSIONS', 'P') IS NOT NULL
	Drop PROCEDURE helper_SP_INSERT_PERMISSIONS
GO


CREATE PROCEDURE helper_SP_INSERT_PERMISSIONS(@rAppID varchar(50), @rGroupID varchar(50)) as 
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

	/** Insert Group-Wise Permission **/

	Insert into SEC_GROUP_PERMISSION (GROUP_ID,PERMISSION_ID,CAN_CREATE,CAN_UPDATE,CAN_DELETE,IS_AUDITING_ALLOWED,SEQUENCE,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY) 
	Select @tmpGroupID,PERMISSION_ID,1,1,1,1,4,GetDate(),'helper_SP_INSERT_PERMISSIONS',GetDate(),'helper_SP_INSERT_PERMISSIONS' FROM SEC_PERMISSION WHERE PERMISSION_ID not in (
		Select PERMISSION_ID FROM SEC_GROUP_PERMISSION where lower(GROUP_ID) = @tmpGroupID
	);
	/** Insert Group-Wise Permission **/

	

	/** Insert Permission Map **/
	
	Insert into SEC_APP_PERMISSION_MAP (PERMISSION_ID,APP_ID,CREATED_ON) 
	Select PERMISSION_ID,@tmpAppID,GETDATE() FROM SEC_PERMISSION WHERE PERMISSION_ID not in (
		Select PERMISSION_ID FROM SEC_APP_PERMISSION_MAP
	);
	/** Insert Permission Map **/

END
-- exec helper_SP_INSERT_PERMISSIONS 'backoffice', 'admin'