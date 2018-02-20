SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE #spAlterColumn_UserRole
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

if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'UserRole')
CREATE TABLE auth.UserRole (
	RoleId Int NOT NULL,
	UserId uniqueidentifier NOT NULL
)
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='UserRole' and column_name='RoleId')
  BEGIN
	ALTER TABLE [auth].UserRole ADD
	    RoleId uniqueidentifier NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='UserRole' and column_name='RoleId') and not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='UserRole' and column_name='RoleId' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_UserRole 'UserRole', 'RoleId', 'uniqueidentifier', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='UserRole' and column_name='UserId')
  BEGIN
	ALTER TABLE [auth].UserRole ADD
	    UserId Int NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='UserRole' and column_name='UserId') and not exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='UserRole' and column_name='UserId' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_UserRole 'UserRole', 'UserId', 'uniqueidentifier', 1
  END
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'UserRole' and CONSTRAINT_Name = 'PK_UserRole' and CONSTRAINT_TYPE = 'PRIMARY KEY')
ALTER TABLE [auth].UserRole WITH NOCHECK ADD
	CONSTRAINT PK_UserRole PRIMARY KEY CLUSTERED
	(
		RoleId,
		UserId
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'UserRole' and CONSTRAINT_Name = 'FK_UserRole_Role' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].UserRole ADD
	CONSTRAINT FK_UserRole_Role FOREIGN KEY
	(
		RoleId
	)
	REFERENCES [auth].Role
	(
		RoleId
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'UserRole' and CONSTRAINT_Name = 'FK_UserRole_User' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].UserRole ADD
	CONSTRAINT FK_UserRole_User FOREIGN KEY
	(
		UserId
	)
	REFERENCES [auth].[User]
	(
		UserId
	)
GO


drop procedure #spAlterColumn_UserRole
GO
