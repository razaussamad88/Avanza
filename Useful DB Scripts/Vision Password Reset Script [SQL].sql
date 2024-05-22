Select * From Sec_User with(nolock)
Where LOGIN_ID in ('admin','superuser1','superuser2','mpguser1','mpguser3')
order by UPDATED_ON desc;


-- Hashsalt = SYMMETRY

-- For unlock user
Update Sec_User	Set Pwd_Retry_Count = 0, Is_Active=1, IS_LOGGED_IN=0, IS_RESET_REQUIRED=0, RESET_PASSWORD=0, UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
Where LOGIN_ID in ('admin','superuser1','superuser2','mpguser1','mpguser3')


-- For reset existing password [new pwd: vision]
Update Sec_User 
Set Password = 'CBRj5397V0s/0YfytfO34Ix3JLci4+9urisg3bwimmszwQHd9sCfLPnoJhliaJGaf72KLMjejkS3oM4HNF2Egw==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
Where Login_Id = 'admin';


-- For reset existing password [new pwd: mpguser1]
Update Sec_User Set Password = 'EAtbJc+C1FBiZ1K3fMBFTnOB8+HyCr0u62F+z5sr3P3zeR9G+s6gdMLrUaiZ6j0gSvLBC1kZ4S5VQiYXr8Gn7Q==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
Where Login_Id = 'mpguser1';


-- For reset existing password [new pwd: mpguser3]
Update Sec_User Set Password = 'IOT265Hp2k+9t/icKXIfy7Tr+mcS30OiCpCEQPDKACC7eEFxPfYsZx9zSMsy5FY73rGKlPuuf4/2w4bN9CB9lA==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
Where Login_Id = 'mpguser3';



-- For reset existing password [new pwd: Avanza@1234]
Update Sec_User Set Password = '+GY4SjjFrZkZvMld8E71C/Lh4nQtPUvZudRZNmCiUl1uMNUE7PMqIK6HgiybdvYwzwR0TRiGmvt308IjQNmeuQ==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
Where Login_Id = 'superuser1';


-- For reset existing password [new pwd: Avanza@1234]
Update Sec_User Set Password = 'MJUvbWxtLVzAHgMxvyn8EKglK6RBLK3DEnEGwxByjp6HdETi15zgkTMQP63nV1VKVwKZXhKtk+2DiMznJFpHsQ==', UPDATED_ON=GetDate(), PWD_UPDATED_ON=GetDate(), STATUS_UPDATED_ON=GetDate()
Where Login_Id = 'superuser2';
