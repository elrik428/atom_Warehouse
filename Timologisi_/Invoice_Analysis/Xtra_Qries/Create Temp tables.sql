--- Veropoulos / My Market
drop table [abc096].[VER_Transactions_Month]
CREATE TABLE [abc096].[VER_Transactions_Month](
     [MID] [varchar](15) COLLATE Greek_CI_AS NULL ,
     [TID] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [DESTCOMID] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [TCODE] [int] NULL ,
     [ORIGINATOR] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [INTERFACE] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [DTSTAMP] [datetime] NULL ,
     [MSGID] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [TAMOUNT] [float] NULL ,
     [TAUTHCODE] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [MASK] [varchar](20) COLLATE Greek_CI_AS NULL ,
     [PROCCODE] [varchar](20) COLLATE Greek_CI_AS NULL ,
     [INST] [int] NULL ,
     [RESPKIND] [varchar](3) COLLATE Greek_CI_AS NULL ,
     [TRESPONSE] [varchar](4) COLLATE Greek_CI_AS NULL ,
     [USERDATA] [varchar](256) COLLATE Greek_CI_AS NULL ,
     [DMID] [varchar](15) COLLATE Greek_CI_AS NULL ,
     [DTID] [varchar](16) COLLATE Greek_CI_AS NULL,
     [PEM] [varchar](5),--20150804
     [MERCHANT_NAME] [varchar](30),
     [ACQUIRING_BANK] [varchar](30),
     [ISSUING_BANK] [nvarchar](50) NULL,
     [BRAND] [varchar](20),--20150629
     [CARD_TYPE] [varchar](100),--20150629
     [GREEK_ISSUER] [varchar](3),
     [ON_US] [varchar](3),
     [ISSUER_BANK_ID] [integer],
     [ACQUIRER_BANK_ID] [integer],
     [STORE_CODE] [varchar](20)
     ) ON [PRIMARY]

---- Duty Free
DROP TABLE [abc096].[DF_Transactions_Month]
CREATE TABLE [abc096].[DF_Transactions_Month_tmp](
     [MID] [varchar](15) COLLATE Greek_CI_AS NULL ,
     [TID] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [DESTCOMID] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [TCODE] [int] NULL ,
     [ORIGINATOR] [varchar](16) COLLATE Greek_CI_AS NULL ,
     [INTERFACE] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [DTSTAMP] [datetime] NULL ,
     [MSGID] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [TAMOUNT] [float] NULL ,
     [TAUTHCODE] [varchar](8) COLLATE Greek_CI_AS NULL ,
     [MASK] [varchar](20) COLLATE Greek_CI_AS NULL ,
     [PROCCODE] [varchar](20) COLLATE Greek_CI_AS NULL ,
     [INST] [int] NULL ,
     [RESPKIND] [varchar](3) COLLATE Greek_CI_AS NULL ,
     [TRESPONSE] [varchar](4) COLLATE Greek_CI_AS NULL ,
     [USERDATA] [varchar](256) COLLATE Greek_CI_AS NULL ,
     [DMID] [varchar](15) COLLATE Greek_CI_AS NULL ,
     [DTID] [varchar](16) COLLATE Greek_CI_AS NULL,
     [PEM] [varchar](5) NULL,--20150804
     [MERCHANT_NAME] [varchar](30),
     [ACQUIRING_BANK] [varchar](30),
     [ISSUING_BANK] [nvarchar](50) NULL,
     [CARD_TYPE] [varchar](15),
     [GREEK_ISSUER] [varchar](3),
     [ON_US] [varchar](3),
     [ISSUER_BANK_ID] [integer],
     [ACQUIRER_BANK_ID] [integer],
	 [ISOCNTRY] [varchar] (64), --20170929
	 [REGIONEUFL] [varchar] (1)	--20170929
     ) ON [PRIMARY]

---- Kotsovolos
drop table [abc096].[KOTS_Transactions_Month]
CREATE TABLE [abc096].[KOTS_Transactions_Month](
    [MID] [varchar](15) COLLATE Greek_CI_AS NULL ,
    [TID] [varchar](16) COLLATE Greek_CI_AS NULL ,
    [DESTCOMID] [varchar](16) COLLATE Greek_CI_AS NULL ,
    [TCODE] [int] NULL ,
    [ORIGINATOR] [varchar](16) COLLATE Greek_CI_AS NULL ,
    [INTERFACE] [varchar](8) COLLATE Greek_CI_AS NULL ,
    [DTSTAMP] [datetime] NULL ,
    [MSGID] [varchar](8) COLLATE Greek_CI_AS NULL ,
    [TAMOUNT] [float] NULL ,
    [TAUTHCODE] [varchar](8) COLLATE Greek_CI_AS NULL ,
    [MASK] [varchar](20) COLLATE Greek_CI_AS NULL ,
    [PROCCODE] [varchar](20) COLLATE Greek_CI_AS NULL ,
    [INST] [int] NULL ,
    [RESPKIND] [varchar](3) COLLATE Greek_CI_AS NULL ,
    [TRESPONSE] [varchar](4) COLLATE Greek_CI_AS NULL ,
    [USERDATA] [varchar](256) COLLATE Greek_CI_AS NULL ,
    [DMID] [varchar](15) COLLATE Greek_CI_AS NULL ,
    [DTID] [varchar](16) COLLATE Greek_CI_AS NULL,
    [PEM] [varchar](5),
    [MERCHANT_NAME] [varchar](30),
    [ACQUIRING_BANK] [varchar](30),
    [ISSUING_BANK] [nvarchar](50) NULL,
    [CARD_TYPE] [varchar](15),
    [GREEK_ISSUER] [varchar](3),
    [ON_US] [varchar](3),
    [ISSUER_BANK_ID] [integer],
    [ACQUIRER_BANK_ID] [integer]
    ) ON [PRIMARY]


----



----
