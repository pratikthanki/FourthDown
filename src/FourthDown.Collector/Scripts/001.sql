USE master
GO

DECLARE @DB_NAME SYSNAME = 'FourthDown';

IF DB_ID(@DB_NAME) IS NULL
    BEGIN
        DECLARE @Query NVARCHAR(255) = N'CREATE DATABASE ' + QUOTENAME(@DB_NAME) + ';';
        DECLARE @Message NVARCHAR(255) = N'Creating database ' + QUOTENAME(@DB_NAME) + ';';

        PRINT @Message;

        EXECUTE sp_executesql @Query;
    END
GO

USE FourthDown
GO

IF(OBJECT_ID('dbo.SchemaVersions') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.SchemaVersions';
        CREATE TABLE [dbo].[SchemaVersions](
            [Id] [int] IDENTITY(1,1) NOT NULL,
            [ScriptName] [nvarchar](255) NOT NULL,
            [Applied] [datetime] NOT NULL,
            CONSTRAINT [PK_SchemaVersions_Id] PRIMARY KEY CLUSTERED([Id] ASC) ON [PRIMARY]
        ) ON [PRIMARY]
    END
GO
