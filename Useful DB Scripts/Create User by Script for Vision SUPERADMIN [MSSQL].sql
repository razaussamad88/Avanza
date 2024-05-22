Insert into SEC_USER (LOGIN_ID,PASSWORD,
	FULL_NAME,IS_ACTIVE,OFFICE_ID,DEPT_ID,ORG_ID,POLICY_ID,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY,EMAIL_ADDRESS_TEMP,
	APP_ID,DATE_OF_BIRTH,MOBILE_NO,IS_DELETED,IS_LOGGED_IN,PWD_RETRY_COUNT,
	STATUS_UPDATED_ON,PWD_UPDATED_ON,RESET_PASSWORD,PHOTOGRAPH)

Select 'superuser1','Vlo4NI7LmJNREp0AUp1aevEeD8fss0GwW5temIi9a0zubM6vgylwxAsTgKxlO6TaWRhQRSzIZfMAXAy3nHWN1Q==',
'Super User 1 - Vision Web',1,'1',1,'1','adminpolicy',GetDate(), 'SYSTEM', GetDate(), 'SYSTEM', 'muhammad.raza@avanzasolutions.com',
'backoffice',GetDate(),'022300122233',0,0,0,GetDate(),GetDate(),0,'default-user.jpeg' Union ALL

Select 'superuser2','brPu1gB3SkvBVxpqqMBi43JsJgHdw+1dXV0bNOM/Uk1j1Qtxgf3zEKmUBdUdCYkcotGO84gVdWCJYE4JiY9/kA==',
'Super User 2 - Vision Web',1,'1',1,'1','adminpolicy',GetDate(), 'SYSTEM', GetDate(), 'SYSTEM', 'muhammad.raza@avanzasolutions.com',
'backoffice',GetDate(),'022300122233',0,0,0,GetDate(),GetDate(),0,'default-user.jpeg';




Insert into SEC_GROUP (GROUP_ID,GROUP_NAME,APP_ID,IS_ACTIVE,IS_ADMIN,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY) 
Select 'superadmin','Super Admin - Vision Web','backoffice',1,1,GetDate(),'SYSTEM',GetDate(),'SYSTEM';


Insert into SEC_USER_GROUP (GROUP_ID,LOGIN_ID,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY) 
Select 'superadmin','superuser1',GetDate(),'SYSTEM',GetDate(),'SYSTEM' Union All
Select 'superadmin','superuser2',GetDate(),'SYSTEM',GetDate(),'SYSTEM';


Insert into SEC_USER_BRANCH(BRANCH_CODE,LOGIN_ID,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY)
Select '0','superuser1',GetDate(),'SYSTEM',GetDate(),'SYSTEM' Union All
Select '0','superuser2',GetDate(),'SYSTEM',GetDate(),'SYSTEM';


Insert into SEC_USER_ENTITY(ENTITY_ID,LOGIN_ID,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY)
Select 1,'superuser1',GetDate(),'SYSTEM',GetDate(),'SYSTEM' Union All
Select 1,'superuser2',GetDate(),'SYSTEM',GetDate(),'SYSTEM';






/** Insert Group-Wise Permission **/

Insert into SEC_GROUP_PERMISSION (GROUP_ID,PERMISSION_ID,CAN_CREATE,CAN_UPDATE,CAN_DELETE,IS_AUDITING_ALLOWED,SEQUENCE,CREATED_ON,CREATED_BY,UPDATED_ON,UPDATED_BY) 

Select 'superadmin',PERMISSION_ID,1,1,1,1,4,GetDate(),'RX-SCRIPT',GetDate(),'RX-SCRIPT' FROM SEC_PERMISSION WHERE PERMISSION_ID not in (
	Select PERMISSION_ID FROM SEC_GROUP_PERMISSION where lower(group_id) = 'superadmin'
);
/** Insert Group-Wise Permission **/




/** Insert Permission Map **/
Insert into SEC_APP_PERMISSION_MAP (PERMISSION_ID,APP_ID,CREATED_ON) 

Select PERMISSION_ID,'backoffice',GETDATE() FROM SEC_PERMISSION WHERE PERMISSION_ID not in (
	Select PERMISSION_ID FROM SEC_APP_PERMISSION_MAP
);
/** Insert Permission Map **/





/* ======================================================================================================== */
-- RESET PASSWORD

-- -----------------------------------------------------
-- Hashsalt = SYMMETRY

		-- For unlock user
		Update Sec_User	Set Pwd_Retry_Count = 0, Is_Active=1, IS_LOGGED_IN=0, IS_RESET_REQUIRED=0, RESET_PASSWORD=0, UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where LOGIN_ID in ('admin','superuser1','superuser2')


		-- For reset existing password [new pwd: vision]
		Update Sec_User 
		Set Password = 'CBRj5397V0s/0YfytfO34Ix3JLci4+9urisg3bwimmszwQHd9sCfLPnoJhliaJGaf72KLMjejkS3oM4HNF2Egw==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where Login_Id = 'admin';


		-- For reset existing password [new pwd: Avanza@1234]
		Update Sec_User Set Password = '+GY4SjjFrZkZvMld8E71C/Lh4nQtPUvZudRZNmCiUl1uMNUE7PMqIK6HgiybdvYwzwR0TRiGmvt308IjQNmeuQ==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where Login_Id = 'superuser1';


		-- For reset existing password [new pwd: Avanza@1234]
		Update Sec_User Set Password = 'MJUvbWxtLVzAHgMxvyn8EKglK6RBLK3DEnEGwxByjp6HdETi15zgkTMQP63nV1VKVwKZXhKtk+2DiMznJFpHsQ==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where Login_Id = 'superuser2';


-- -----------------------------------------------------
-- Hashsalt = VISION

		-- For reset existing password [new pwd: vision]
		Update Sec_User 
		Set Password = 'SOPt31cHGwwa3kQ2Rri2U16ajgVkvOet834KUbGyy5XGrnrbOyTlDkZLLJm5jvG05uzYzwnFl+gjT+O9va3/6Q==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where Login_Id = 'admin';


		-- For reset existing password [new pwd: Avanza@1234]
		Update Sec_User Set Password = 'BrGGTErwgO7QESfFvE+if84V2xIaLLhoDTtj0qqme80Q0G72O8Lv9aScfPhEpiO+EN3cgckp3s93vCo2trNl9w==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where Login_Id = 'superuser1';


		-- For reset existing password [new pwd: Avanza@1234]
		Update Sec_User Set Password = 'fCUc0dVZRjOBOrKe1CYRDrlF5s12hedHadbyBGEo6bozQCz5D9P6IWCUv4rvTXrp6gAT07VErnA02t+VNUGMRA==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
		Where Login_Id = 'superuser2';
		
/* ======================================================================================================== */

