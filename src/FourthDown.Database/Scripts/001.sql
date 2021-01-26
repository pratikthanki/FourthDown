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

IF(OBJECT_ID('dbo.ChangeScripts') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.ChangeScripts';
        CREATE TABLE dbo.ChangeScripts
        (
            ChangeScriptId              INT IDENTITY(1, 1)  NOT NULL,
            ChangeScriptName            VARCHAR(255)        NOT NULL,
            ChangeScriptDeployStart     DATETIME2(7)        NOT NULL    CONSTRAINT FK_db_ChangeScripts_ChangeScriptDeployStart DEFAULT(SYSUTCDATETIME()),
            ChangeScriptDeployEnd       DATETIME2(7)        NULL,
            ChangeScriptDeployDuration                      AS ISNULL(DATEDIFF(MILLISECOND, ChangeScriptDeployStart, ChangeScriptDeployEnd), -1),
            ChangeScriptDeploySuccess   BIT                 NOT NULL    CONSTRAINT FK_db_ChangeScripts_ChangeScriptDeploySuccess DEFAULT(1),
            CONSTRAINT PK_db_ChangeScripts PRIMARY KEY CLUSTERED (ChangeScriptId)
        );
    END
GO
