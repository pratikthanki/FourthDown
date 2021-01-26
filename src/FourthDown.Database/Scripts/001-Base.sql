-- noinspection SqlNoDataSourceInspectionForFile

DECLARE @DB_NAME SYSNAME = 'FourthDown';

IF DB_ID(@DB_NAME) IS NULL
    BEGIN
        DECLARE @Query NVARCHAR(255) = N'CREATE DATABASE ' + QUOTENAME(@DB_NAME) + ';';
        DECLARE @Message NVARCHAR(255) = N'Creating database ' + QUOTENAME(@DB_NAME) + ';';

        PRINT @Message;

        EXECUTE sp_executesql @Query;
    END
    
