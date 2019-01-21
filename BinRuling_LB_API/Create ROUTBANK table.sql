USE [ZACRPT]
GO

/****** Object:  Table [dbo].[ROUTEBANK]    Script Date: 29/Ιαν/2018 09:28:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ROUTEBANK](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BANKDEscr] [varchar](50) NULL,
	[uploadhostid] [varchar](3) NOT NULL,
	[DESTPORT] [varchar](20) NOT NULL,
	[bankdescr_short] [varchar](20) NULL,
	[ORDER_view] [int] NULL
) ON [PRIMARY]
GO
