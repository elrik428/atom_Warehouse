select
--TER_TID,
TER_DL_TIME, COUNT(*)
--substr(TER_TID,6,3)
from TERMINAL_TMP where length(TER_TID) = 8 and substr(TER_TID,1,1) in ('0','1','2','3','4','5','6','7','8','9')
GROUP BY TER_DL_TIME
ORDER BY TER_DL_TIME;
