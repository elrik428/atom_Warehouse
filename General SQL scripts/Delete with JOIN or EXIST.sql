delete easypa_al from dbo.EasyPayAgent_2018  as easypa_al
inner join  dbo.EasyPayAgent b on  easypa_al.Customer = b.Customer and easypa_al.TerminalID = b.TerminalID and easypa_al.TrxDate = b.TrxDate and easypa_al.TrxTime = b.TrxTime and easypa_al.Amount = b.Amount and easypa_al.Trace = b.Trace
--where exists (select * from [dbo].[EasyPayAgent] b where a.Customer = b.Customer and a.TerminalID = b.TerminalID and a.TrxDate = b.TrxDate and a.TrxTime = b.TrxTime and a.Amount = b.Amount and a.Trace = b.Trace)
