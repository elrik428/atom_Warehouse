SET ZPATH=C:\Program Files\7-Zip
SET INPATH=\\10.7.17.11\Storage\TransactReports\DCCReports
SET OUTPATH=\\10.7.17.11\Storage\TransactReports\DCCReports


SET FileName=ZAC_Greece_*.zip
COPY "%INPATH%\%FileName%" "%OUTPATH%\"
cd %ZPATH%
7z e %OUTPATH%\%FileName% -o%OUTPATH% -y
MOVE /Y "%OUTPATH%\%FileName%" "%OUTPATH%\Processed\"
