
CREATE OR REPLACE PROCEDURE BIZPROCSTATE_TABLE_PLAIN_ENTRY
(
  v_Msg_Type Varchar2 := Null,
  v_Classname Varchar2 := Null,
  v_MethodName Varchar2 := Null,

  v_Nextseqnum_Success numeric := 0,
  v_Nextseqnum_Fail numeric := 0,
  v_Sequencenumber numeric := 0
)
AS

BEGIN

    Insert Into Biz_Process_States(Processstateid,Classname,Methodname,Bizprocessid,Nextseqnum_Success,Nextseqnum_Fail,Sequencenumber)
    Values ((Select Max(Processstateid) + 1 From Biz_Process_States), v_Classname, v_MethodName,
    -- Values (NULL, v_Classname, v_MethodName,
      (Select Bizprocessid From Biz_Process Where  Messagetype = v_Msg_Type),
      v_Nextseqnum_Success, v_Nextseqnum_Fail, v_Sequencenumber); 
  
END;
/


CREATE OR REPLACE PROCEDURE BIZPROCSTATE_TBL_DELETE_ENTRY
(
  v_Msg_Type Varchar2 := Null,
  v_MethodName Varchar2 := Null
)
AS
	v_BIZPROCESSID numeric := 0;

BEGIN
	
	Select B.Bizprocessid into v_BIZPROCESSID From Biz_Process B Where B.Messagetype = v_Msg_Type;
  
	Delete From Biz_Process_States Where Bizprocessid = v_BIZPROCESSID and Methodname = v_MethodName;
  
END;
/