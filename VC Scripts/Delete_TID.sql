
 DECLARE @locktimestamp        datetime
 BEGIN
 SET NOCOUNT ON SET LOCK_TIMEOUT 0;
 IF @pvMULTIAPP = 'Y'
  BEGIN
  DECLARE termCursor CURSOR FOR
  select locktimestamp from relation where famnm = @pvMODELNAME  and termid = @pvTERMID
  OPEN termCursor  IF @@error <> 0
  BEGIN SELECT @pvError = description from master.dbo.sysmessages where error = @@error RETURN
  END

  FETCH NEXT FROM termCursor INTO @locktimestamp
  if @@ROWCOUNT = 0
  BEGIN set @pvError = 'TERMINAL_NOTFOUND' CLOSE termCursor DEALLOCATE termCursor RETURN
  END

  WHILE @@FETCH_STATUS = 0
  BEGIN IF @locktimestamp is not NULL
  BEGIN set @pvError = 'TERMINAL_DOWNLOADING' CLOSE termCursor
  DEALLOCATE termCursor RETURN
  END 
  FETCH NEXT FROM termCursor INTO @locktimestamp END CLOSE termCursor
  DEALLOCATE termCursor

  delete from relation where famnm = @pvMODELNAME and termid = @pvTERMID

  IF @@error <> 0
    BEGIN SELECT @pvError = description from master.dbo.sysmessages where error = @@error RETURN END
    set @pvError = 'NO_ERROR' END
  ELSE
   BEGIN select @locktimestamp = locktimestamp from relation where famnm = @pvMODELNAME  and appnm = @pvAPPNAME and termid = @pvTERMID  if @@ROWCOUNT = 0
   BEGIN set @pvError = 'TERMINAL_NOTFOUND' RETURN END
   IF @@error <> 0
   BEGIN SELECT @pvError = description from master.dbo.sysmessages where error = @@error RETURN END
   IF @locktimestamp is not NULL BEGIN set @pvError = 'TERMINAL_DOWNLOADING' RETURN END

   delete from relation where famnm = @pvMODELNAME and appnm = @pvAPPNAME and termid = @pvTERMID

   IF @@error <> 0

   BEGIN SELECT @pvError = description from master.dbo.sysmessages where error = @@error RETURN END
   set @pvError = 'NO_ERROR' END
 END


declare @p5 varchar(255)
set @p5='NO_ERROR'
exec terminal_delete 'Vx-520','MA','55555555','Y',@p5 output
select @p5
