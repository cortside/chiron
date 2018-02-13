SET ANSI_NULLS ON 
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[Item].[Item]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE Item.[Item] (
	ItemId Int IDENTITY(1,1) NOT NULL,
	Title varchar(50) NOT NULL,
	Description varchar(255) NOT NULL,
	Cost money NOT NULL,
	Price money NOT NULL,
	BuyerId int NULL,
	CreateDate DateTime NOT NULL,
	CreateUserId Int NOT NULL,
	LastModifiedDate DateTime NOT NULL,
	LastModifiedUserId Int NOT NULL
)
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'Item.PK_Item') and OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE Item.[Item] WITH NOCHECK ADD
	CONSTRAINT PK_Item PRIMARY KEY CLUSTERED
	(
		ItemId
	)
GO