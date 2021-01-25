DECLARE @DB_NAME SYSNAME = 'FourthDown';

IF DB_ID(@DB_NAME) IS NULL
    BEGIN
        DECLARE @Qry NVARCHAR(255) = N'CREATE DATABASE ' + QUOTENAME(@DB_NAME) + ';';
        DECLARE @Msg NVARCHAR(255) = N'Creating database ' + QUOTENAME(@DB_NAME) + ';';

        PRINT @Msg;

        EXECUTE sp_executesql @Qry;
    END
    
