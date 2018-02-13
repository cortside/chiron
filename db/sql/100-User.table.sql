SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE #spAlterColumn_User
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

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'auth' AND TABLE_NAME = 'user')
CREATE TABLE [auth].[user] (
	UserId Int IDENTITY(1,1) NOT NULL,
	UserStatus VarChar(10) NULL,
	Username VarChar(100) NOT NULL,
	Password VarChar(100) NOT NULL,
	Salt VarChar(50) NOT NULL,
	LoginCount Int NOT NULL CONSTRAINT [DF_User_LoginCount] DEFAULT (0),
	LastLogin DateTime NULL,
	LastLoginIPAddress VarChar(50) NULL,
	EffectiveDate DateTime NOT NULL,
	ExpirationDate DateTime NULL,
	SecurityToken VarChar(50) NULL,
	TokenExpiration DateTime NULL,
	TermsOfUseAcceptanceDate DateTime NULL,
	CreateDate DateTime NOT NULL,
	CreateUserId Int NOT NULL,
	LastModifiedDate DateTime NOT NULL,
	LastModifiedUserId Int NOT NULL
)
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='UserId')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    UserId Int IDENTITY(1,1) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='UserId') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='UserId' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'UserId', 'Int', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='UserStatus')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    UserStatus VarChar(10) NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='UserStatus') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='UserStatus' and character_maximum_length=10 and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'UserStatus', 'VarChar(10)', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='Username')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    Username VarChar(100) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='Username') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='Username' and character_maximum_length=100 and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'Username', 'VarChar(100)', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='Password')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    Password VarChar(100) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='Salt') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='Salt' and character_maximum_length=50 and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'Salt', 'VarChar(50)', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='Salt')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    Salt VarChar(50) NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='Password') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='Password' and character_maximum_length=100 and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'Password', 'VarChar(100)', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LoginCount')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    LoginCount Int NOT NULL
	CONSTRAINT
	    [DF_User_LoginCount] DEFAULT 0 WITH VALUES
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='LoginCount') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='LoginCount' and is_nullable='YES')
  BEGIN
	declare @cdefault varchar(1000)
	select @cdefault = '[' + object_name(cdefault) + ']' from syscolumns where id=object_id('[auth].[User]') and name = 'LoginCount'

	if @cdefault is not null
		exec('alter table [auth].[user] DROP CONSTRAINT ' + @cdefault)
		
	if exists(select * from sysobjects where name = 'DF_User_LoginCount' and xtype='D')
          begin
            declare @table sysname
            select @table=object_name(parent_obj) from  sysobjects where (name = 'DF_User_LoginCount' and xtype='D')
            exec('alter table ' + @table + ' DROP CONSTRAINT [DF_User_LoginCount]')
          end
	
	exec #spAlterColumn_User 'User', 'LoginCount', 'Int', 0
	if not exists(select * from sysobjects where name = 'DF_User_LoginCount' and xtype='D')
		alter table [auth].[user]
			ADD CONSTRAINT [DF_User_LoginCount] DEFAULT 0 FOR LoginCount WITH VALUES
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastLogin')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    LastLogin DateTime NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='LastLogin') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='LastLogin' and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'LastLogin', 'DateTime', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastLoginIPAddress')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    LastLoginIPAddress VarChar(50) NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='LastLoginIPAddress') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='LastLoginIPAddress' and character_maximum_length=50 and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'LastLoginIPAddress', 'VarChar(50)', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='EffectiveDate')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    EffectiveDate DateTime NOT NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='EffectiveDate') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='EffectiveDate' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'EffectiveDate', 'DateTime', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='ExpirationDate')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    ExpirationDate DateTime NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='ExpirationDate') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='ExpirationDate' and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'ExpirationDate', 'DateTime', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='SecurityToken')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    SecurityToken VarChar(50) NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='SecurityToken') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='SecurityToken' and character_maximum_length=50 and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'SecurityToken', 'VarChar(50)', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='TokenExpiration')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    TokenExpiration DateTime NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='TokenExpiration') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='TokenExpiration' and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'TokenExpiration', 'DateTime', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='TermsOfUseAcceptanceDate')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    TermsOfUseAcceptanceDate DateTime NULL
  END
GO


if exists(select * from information_schema.columns where TABLE_SCHEMA = 'auth' and table_name='user' and column_name='TermsOfUseAcceptanceDate') and not exists(select * from information_schema.columns where table_schema = 'auth' and table_name='user' and column_name='TermsOfUseAcceptanceDate' and is_nullable='YES')
  BEGIN
	exec #spAlterColumn_User 'User', 'TermsOfUseAcceptanceDate', 'DateTime', 0
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='CreateUserId')
  BEGIN
	ALTER TABLE [auth].[user] ADD
		CreateUserId Int NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='CreateUserId') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='UserId' and data_type='int' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'CreateUserId', 'Int', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='CreateDate')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    CreateDate DateTime NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='CreateDate') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='CreateDate' and data_type='datetime' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'CreateDate', 'DateTime', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastModifiedUserId')
  BEGIN
	ALTER TABLE [auth].[user] ADD
		LastModifiedUserId Int NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastModifiedUserId') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastModifiedUserId' and data_type='int' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'LastModifiedUserId', 'Int', 1
  END
GO

if not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastModifiedDate')
  BEGIN
	ALTER TABLE [auth].[user] ADD
	    LastModifiedDate DateTime NOT NULL
  END
GO

if exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastModifiedDate') and not exists(select * from information_schema.columns where TABLE_SCHEMA='auth' and table_name='User' and column_name='LastModifiedDate' and data_type='datetime' and is_nullable='NO')
  BEGIN
	exec #spAlterColumn_User 'User', 'LastModifiedDate', 'DateTime', 1
  END
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'user' and CONSTRAINT_Name = 'PK_User' and CONSTRAINT_TYPE = 'PRIMARY KEY')
ALTER TABLE [auth].[user] WITH NOCHECK ADD
	CONSTRAINT PK_User PRIMARY KEY CLUSTERED
	(
		UserId
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'user' and CONSTRAINT_Name = 'UN_User' and CONSTRAINT_TYPE = 'UNIQUE')
ALTER TABLE [auth].[user] ADD
	CONSTRAINT UN_User UNIQUE
	(
		Username
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'user' and CONSTRAINT_Name = 'FK_User_CreatedUser' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].[user] ADD
	CONSTRAINT FK_User_CreatedUser FOREIGN KEY
	(
		CreateUserId
	)
	REFERENCES [auth].[user]
	(
		UserId
	)
GO

if not exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab where CONSTRAINT_SCHEMA = 'auth' and TABLE_NAME = 'user' and CONSTRAINT_Name = 'FK_User_LastModifiedUser' and CONSTRAINT_TYPE = 'FOREIGN KEY')
ALTER TABLE [auth].[user] ADD
	CONSTRAINT FK_User_LastModifiedUser FOREIGN KEY
	(
		LastModifiedUserId
	)
	REFERENCES [auth].[user]
	(
		UserId
	)
GO

drop procedure #spAlterColumn_User
GO
