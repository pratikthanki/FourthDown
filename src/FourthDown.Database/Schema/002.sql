
IF(OBJECT_ID('dbo.CombineWorkouts') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.CombineWorkouts';
        CREATE TABLE [dbo].[CombineWorkouts]
        (
            Id                      INT                 NULL,
            ShieldId                VARCHAR(255)        NULL,
            FirstName               VARCHAR(255)        NULL,
            LastName                VARCHAR(255)        NULL,
            College                 VARCHAR(255)        NULL,
            Position                VARCHAR(255)        NULL,
            Season                  INT                 NULL,
            FortyYardDash           FLOAT               NULL,
            BenchPress              FLOAT               NULL,
            VerticalJump            FLOAT               NULL,
            BroadJump               FLOAT               NULL,
            ThreeConeDrill          FLOAT               NULL,
            TwentyYardShuttle       FLOAT               NULL,
            SixtyYardShuttle        FLOAT               NULL
        )
    END 
GO


IF(OBJECT_ID('dbo.Teams') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.Teams';
        CREATE TABLE [dbo].[Teams]
        (
            [City]               VARCHAR(255)        NULL,
            [Name]               VARCHAR(255)        NULL,
            [Abbreviation]       VARCHAR(255)        NULL,
            [Conference]         VARCHAR(255)        NULL,
            [Division]           VARCHAR(255)        NULL,
            [Label]              VARCHAR(255)        NULL,
            [TeamNameLabel]      VARCHAR(255)        NULL
        )
    END 
GO


IF(OBJECT_ID('dbo.Schedule') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.Schedule';
        CREATE TABLE [dbo].[Schedule]
        (
            [GameId]             VARCHAR(255)        NULL,
            [Season]             INT                 NULL,
            [GameType]           VARCHAR(255)        NULL,
            [Week]               INT                 NULL,
            [Gameday]            DATETIME2           NULL,
            [Weekday]            VARCHAR(255)        NULL,
            [Gametime]           VARCHAR(255)        NULL,
            [AwayTeam]           VARCHAR(255)        NULL,
            [AwayScore]          INT                 NULL,
            [HomeTeam]           VARCHAR(255)        NULL,
            [HomeScore]          INT                 NULL,
            [Location]           VARCHAR(255)        NULL,
            [Result]             INT                 NULL,
            [Total]              INT                 NULL,
            [Overtime]           BIT                 NULL,
            [OldGameId]          VARCHAR(255)        NULL,
            [AwayRest]           INT                 NULL,
            [HomeRest]           INT                 NULL,
            [AwayMoneyline]      FLOAT               NULL,
            [HomeMoneyline]      FLOAT               NULL,
            [SpreadLine]         FLOAT               NULL,
            [AwaySpreadOdds]     FLOAT               NULL,
            [HomeSpreadOdds]     FLOAT               NULL,
            [TotalLine]          FLOAT               NULL,
            [UnderOdds]          FLOAT               NULL,
            [OverOdds]           FLOAT               NULL,
            [DivGame]            BIT                 NULL,
            [Roof]               VARCHAR(255)        NULL,
            [Surface]            VARCHAR(255)        NULL,
            [Temp]               INT                 NULL,
            [Wind]               INT                 NULL,
            [AwayCoach]          VARCHAR(255)        NULL,
            [HomeCoach]          VARCHAR(255)        NULL,
            [Referee]            VARCHAR(255)        NULL,
            [StadiumId]          VARCHAR(255)        NULL,
            [Stadium]            VARCHAR(255)        NULL,
        )
    END 
GO


