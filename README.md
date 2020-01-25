# LunchVarbergApi
Ändra DefaultConnectionString i appsettings.json så den leder till din databas.

Sätt upp din databas med dessa kommandona:

USE [LunchVarberg]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 2020-01-24 16:43:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[id] [int] NOT NULL,
	[Name] [nchar](250) NOT NULL,
	[Description] [nchar](250) NOT NULL,
	[IncludedItems] [nchar](250) NOT NULL,
	[LogoUrl] [nchar](250) NOT NULL,
	[CustomerPrice] [int] NULL,
	[LunchPrice] [varchar](50) NULL,
	[OpenHours] [varchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [LunchVarberg]
GO

/****** Object:  Table [dbo].[Menu]    Script Date: 2020-01-24 16:43:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Menu](
	[id] [int] NOT NULL,
	[Description] [nchar](250) NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([id])
GO

ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_Menu_Customer]
GO

USE [LunchVarberg]
GO

/****** Object:  Table [dbo].[UserCredentials]    Script Date: 2020-01-24 16:43:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserCredentials](
	[id] [int] NOT NULL,
	[username] [varchar](40) UNIQUE NOT NULL,
	[password] [varchar](40) NOT NULL,
	[isAdmin] [bit] NOT NULL,
 CONSTRAINT [PK__UserCred__3213E83F90AB0A15] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserCredentials]  WITH CHECK ADD  CONSTRAINT [FK__UserCredenti__id__36B12243] FOREIGN KEY([id])
REFERENCES [dbo].[Customer] ([id])
GO

ALTER TABLE [dbo].[UserCredentials] CHECK CONSTRAINT [FK__UserCredenti__id__36B12243]
GO

