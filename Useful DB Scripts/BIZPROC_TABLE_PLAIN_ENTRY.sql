
CREATE OR REPLACE PROCEDURE BIZPROC_TABLE_PLAIN_ENTRY
(
  v_BizProcName Varchar2 := Null,
  v_Msg_Type Varchar2 := Null,
  v_Desc Varchar2 := Null,
  v_Classname Varchar2 := Null,
  v_MethodName Varchar2 := Null
)
AS

BEGIN
    
    Insert Into Biz_Process(BIZPROCESSID,BIZPROCESSNAME,CHANNELID,MESSAGETYPE,DESCRIPTION,ACTION_TYPE)
    VALUES ((SELECT Max(BIZPROCESSID) + 1 FROM BIZ_PROCESS), v_BizProcName,'1',v_Msg_Type,v_Desc,'1');

    Insert Into Biz_Process_States(Processstateid,Classname,Methodname,Bizprocessid,Nextseqnum_Success,Nextseqnum_Fail,Sequencenumber)
    Values ((Select Max(Processstateid) + 1 From Biz_Process_States), v_Classname,v_MethodName,
      (Select Bizprocessid From Biz_Process Where  Messagetype = v_Msg_Type),
      0,0,1); 
  
END;
/


CREATE OR REPLACE PROCEDURE BIZPROC_TABLE_DELETE_ENTRY
(
  v_Msg_Type Varchar2:= Null
)
AS
  v_BIZPROCESSID numeric := NULL;
  
BEGIN

	Select NVL(B.Bizprocessid, 0) into v_BIZPROCESSID From Biz_Process B Where B.Messagetype = v_Msg_Type;
  
    IF (v_BIZPROCESSID is not null AND v_BIZPROCESSID != 0)
    THEN
        Delete From Biz_Process_States Where Bizprocessid = v_BIZPROCESSID;
        Delete FROM Biz_Process Where Bizprocessid = v_BIZPROCESSID;
    END IF;
  
END;
/
