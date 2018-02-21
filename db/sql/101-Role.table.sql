SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE #spAlterColumn_Role
    @table varchar(100),
    @column varchar(100),
    @type varchar(50),
    @required bit
AS
if exists (select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name=@table and column_name=@column)
begin
	declare @nullstring varchar(8)
	set @nullstring = case when @required=0 then 'null' else 'not null' end
	exec('alter table [auth].[' + @table + '] alter column [' + @column + '] ' + @type + ' ' + @nullstring)
end
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'Role')
CREATE TABLE [auth].[Role] (
	RoleId Int IDENTITY(1,1) NOT NULL,
	Name VarChar(50) NOT NULL,
	Description VarChar(4000) NOT NULL,
	CreateDate DateTime NOT NULL,
	CreateUserId uniqueidentifier NOT NULL,
	LastModifiedDate DateTime NOT NULL,
	LastModifiedUserId uniqueidentifier NOT NULL

)
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='RoleId')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
	    RoleId Int IDENTITY(1,1) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='RoleId') and not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='RoleId' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'RoleId', 'Int', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='Name')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
	    Name VarChar(50) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='Name') and not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='Name' and character_maximum_length=50 and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'Name', 'VarChar(50)', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='Description')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
	    Description VarChar(4000) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='Description') and not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='Role' and column_name='Description' and character_maximum_length=4000 and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'Description', 'VarChar(4000)', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='CreateUserId')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
		CreateUserId uniqueidentifier NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='CreateUserId') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='UserId' and data_type='uniqueidentifier' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'CreateUserId', 'uniqueidentifier', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='CreateDate')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
	    CreateDate DateTime NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='CreateDate') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='CreateDate' and data_type='datetime' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'CreateDate', 'DateTime', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='LastModifiedUserId')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
		LastModifiedUserId uniqueidentifier NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='LastModifiedUserId') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='LastModifiedUserId' and data_type='uniqueidentifier' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'LastModifiedUserId', 'uniqueidentifier', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='LastModifiedDate')
  BEGIN
	ALTER TABLE [auth].[Role] ADD
	    LastModifiedDate DateTime NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='LastModifiedDate') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='Role' and column_name='LastModifiedDate' and data_type='datetime' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_Role 'Role', 'LastModifiedDate', 'DateTime', 1
  END
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'Role' and CONSTRAINT_Name = 'PK_Role' and CONSTRAINT_TYPE = 'PRIMARY KEY')
ALTER TABLE [auth].[Role] WITH NOCHECK ADD
	CONSTRAINT PK_Role PRIMARY KEY CLUSTERED
	(
		RoleId
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'Role' and CONSTRAINT_Name = 'UN_Role' and CONSTRAINT_TYPE = 'UNIQUE')
ALTER TABLE [auth].[Role] ADD
	CONSTRAINT UN_Role UNIQUE
	(
		Name
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'Role' and CONSTRAINT_Name = 'FK_Role_CreatedUser' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].[Role] ADD
	CONSTRAINT FK_Role_CreatedUser FOREIGN KEY
	(
		CreateUserId
	)
	REFERENCES [auth].[User]
	(
		UserId
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'Role' and CONSTRAINT_Name = 'FK_Role_LastModifiedUser' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].[Role] ADD
	CONSTRAINT FK_Role_LastModifiedUser FOREIGN KEY
	(
		LastModifiedUserId
	)
	REFERENCES [auth].[User]
	(
		UserId
	)
GO

drop procedure #spAlterColumn_Role
GO
