
  DECLARE @clusterName varchar(50);
  DECLARE @modbyUserId varchar(50);
  DECLARE @UTID numeric;

  BEGIN
  select @clusterName = CLUSTERNM from CLUSTERINFO where CLUSTERID = @pvCLUSTERID;
  select @modbyUserId = USERID from CORE_USER where USERNAME = @pvMODBY;
  select @UTID = (select distinct(utid) from terminfovfirel where MODELNAME = @pvFAMNM and TERMID = @pvTERMID);

   INSERT INTO TERMNOTES (UTID,DATECREATED,MODIFIEDBY,MODIFICATIONHISTORY,FAMNM,APPNM,TERMID,CLUSTERNM,MODIFIEDBYUSERNAME,CLUSTERID) VALUES(@UTID,GetDate(),@modbyUserId,@pvModHistory ,@pvFAMNM,@pvAPPNM,@pvTERMID,@clusterName,@pvMODBY,@pvCLUSTERID);
    IF @@error <> 0 BEGIN SELECT @pvError = description from master.dbo.sysmessages where error = @@error RETURN END END

declare @p7 varchar(255)
set @p7=NULL
exec TERMINAL_NOTES_RANGE 'EURONET','Vx-520','VMAC0132, ACT0108, QT000510, EPOS0200, EOS020101, EMV800, CTLS20116, CLA013646, EPOS0200P','55555555','LNestoras','Terminal deleted. Applications [VMAC0132, ACT0108, QT000510, EPOS0200, EOS020101, EMV800, CTLS20116, CLA013646, EPOS0200P]',@p7 output
select @p7
