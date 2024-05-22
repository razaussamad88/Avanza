
IF OBJECT_ID('POPULATE_NETWORK_MAP_QUERY', 'P') IS NOT NULL
  DROP PROCEDURE "POPULATE_NETWORK_MAP_QUERY";
GO

CREATE PROCEDURE "POPULATE_NETWORK_MAP_QUERY"
AS
BEGIN
  DECLARE @sNetworkId INT = NULL;

  DECLARE CInternalMapFieldList CURSOR LOCAL
	FOR 
	SELECT Network_Id FROM NETWORK WHERE NETWORK_TYPE = 'Delivery Channel' AND Network_Id is not null;
  
  SET NOCOUNT ON;
  print 'MPG_SET_NETWORK_MAP_QUERY : Started...';
	
  OPEN CInternalMapFieldList;
  FETCH CInternalMapFieldList INTO @sNetworkId;
  WHILE @@FETCH_STATUS = 0 
	  BEGIN
		--TO DO
		print 'Populating for Network ID : ' + cast(@sNetworkId as varchar);
		exec dbo.MPG_SET_NETWORK_MAP_QUERY @sNetworkId;
		FETCH CInternalMapFieldList INTO @sNetworkId;
	  END;

  CLOSE CInternalMapFieldList;
  DEALLOCATE CInternalMapFieldList;
  
  print 'MPG_SET_NETWORK_MAP_QUERY : Completed!';
  
  -- COMMIT;

END;