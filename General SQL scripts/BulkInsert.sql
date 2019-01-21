BULK INSERT  [abc096].[IMP_TRANSACT_D_tempLN]
    FROM '\\grat1-dev-ap2t\d$\Reporting\Import\report.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    ERRORFILE = '\\grat1-dev-ap2t\d$\Reporting\Import\reportErrorRows.csv',
    TABLOCK
    )
