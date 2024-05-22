
CREATE VIEW helper_VW_Biz_Process AS
  SELECT Pr.BizprocessID,
    Pr.Messagetype,
    pr.action_type,
    st.CLASSNAME,
    st.METHODNAME,
    st.NEXTSEQNUM_FAIL,
    st.NEXTSEQNUM_SUCCESS,
    st.Processstateid,
    st.SEQUENCENUMBER
  FROM Biz_Process Pr
  Left Join Biz_Process_States St
  ON Pr.Bizprocessid = St.Bizprocessid;
/