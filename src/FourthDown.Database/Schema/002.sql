
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


IF(OBJECT_ID('dbo.GameTeams') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.GameTeams';
        CREATE TABLE [dbo].[GameTeams]
        (
            [GameId]                VARCHAR(255)        NULL,
            [TeamAbbreviation]      VARCHAR(255)        NULL,
            [TeamNickName]          VARCHAR(255)        NULL,
            [PointsQ1]              INT                 NULL,
            [PointsQ2]              INT                 NULL,
            [PointsQ3]              INT                 NULL,
            [PointsQ4]              INT                 NULL,
            [PointsOvertimeTotal]   INT                 NULL,
            [PointsTotal]           INT                 NULL,
            [TimeoutsUsed]          INT                 NULL,
            [TimeoutsRemaining]     INT                 NULL,
            [IsHome]                BIT                 NULL
        )
    END 
GO


IF(OBJECT_ID('dbo.GameDrives') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.GameDrives';
        CREATE TABLE [dbo].[GameDrives]
        (
            [GameId]                                    VARCHAR(255)        NULL,
            [QuarterStart]                              INT                 NULL,
            [EndTransition]                             VARCHAR(255)        NULL,
            [EndYardLine]                               VARCHAR(255)        NULL,
            [EndedWithScore]                            BIT                 NULL,
            [FirstDowns]                                INT                 NULL,
            [GameClockEnd]                              VARCHAR(255)        NULL,
            [GameClockStart]                            VARCHAR(255)        NULL,
            [HowEndedDescription]                       VARCHAR(255)        NULL,
            [HowStartedDescription]                     VARCHAR(255)        NULL,
            [Inside20]                                  BIT                 NULL,
            [OrderSequence]                             BIGINT              NULL,
            [PlayCount]                                 INT                 NULL,
            [PlayIdEnded]                               INT                 NULL,
            [PlayIdStarted]                             INT                 NULL,
            [PlaySeqEnded]                              INT                 NULL,
            [PlaySeqStarted]                            INT                 NULL,
            [QuarterEnd]                                INT                 NULL,
            [StartTransition]                           VARCHAR(255)        NULL,
            [StartYardLine]                             VARCHAR(255)        NULL,
            [TimeOfPossession]                          VARCHAR(255)        NULL,
            [Yards]                                     INT                 NULL,
            [YardsPenalized]                            INT                 NULL,
            [PossessionTeamAbbreviation]                VARCHAR(255)        NULL,
            [PossessionTeamNickName]                    VARCHAR(255)        NULL,
            [PossessionTeamFranchiseCurrentLogoUrl]     VARCHAR(255)        NULL,

        )
    END 
GO


IF(OBJECT_ID('dbo.GameScoringSummaries') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.GameScoringSummaries';
        CREATE TABLE [dbo].[GameScoringSummaries]
        (
            [GameId]                VARCHAR(255)        NULL,
            [PlayId]                INT                 NULL,
            [PlayDescription]       VARCHAR(255)        NULL,
            [PatPlayId]             INT                 NULL,
            [HomeScore]             INT                 NULL,
            [VisitorScore]          INT                 NULL,
        )
    END 
GO


IF(OBJECT_ID('dbo.GamePlays') IS NULL)
    BEGIN
        PRINT N'Creating table dbo.GamePlays';
        CREATE TABLE [dbo].[GamePlays]
        (
            [GameId]                                    VARCHAR(255)        NULL,
            [ClockTime]                                 VARCHAR(255)        NULL,
            [Down]                                      INT                 NULL,
            [EndClockTime]                              VARCHAR(255)        NULL,
            [EndYardLine]                               VARCHAR(255)        NULL,
            [FirstDown]                                 BIT                 NULL,
            [GoalToGo]                                  BIT                 NULL,
            [NextPlayIsGoalToGo]                        BIT                 NULL,
            [NextPlayType]                              VARCHAR(255)        NULL,
            [OrderSequence]                             BIGINT              NULL,
            [PenaltyOnPlay]                             BIT                 NULL,
            [PlayClock]                                 VARCHAR(255)        NULL,
            [PlayDeleted]                               BIT                 NULL,
            [PlayDescription]                           VARCHAR(255)        NULL,
            [PlayDescriptionWithJerseyNumbers]          VARCHAR(255)        NULL,
            [PlayId]                                    INT                 NULL,
            [PlayType]                                  VARCHAR(255)        NULL,
            [PrePlayByPlay]                             VARCHAR(255)        NULL,
            [Quarter]                                   INT                 NULL,
            [ScoringPlay]                               BIT                 NULL,
            [ShortDescription]                          VARCHAR(255)        NULL,
            [SpecialTeamsPlay]                          BIT                 NULL,
            [TimeOfDay]                                 VARCHAR(255)        NULL,
            [YardLine]                                  VARCHAR(255)        NULL,
            [Yards]                                     INT                 NULL,
            [YardsToGo]                                 INT                 NULL,
            [DriveNetYards]                             INT                 NULL,
            [DrivePlayCount]                            INT                 NULL,
            [DriveSequenceNumber]                       INT                 NULL,
            [DriveTimeOfPossession]                     VARCHAR(255)        NULL,
            [PossessionTeamAbbreviation]                VARCHAR(255)        NULL,
            [PossessionTeamNickName]                    VARCHAR(255)        NULL,
            [PossessionTeamFranchiseCurrentLogoUrl]     VARCHAR(255)        NULL,
            [IsBigPlay]                                 BIT                 NULL,
            [ScoringPlayType]                           VARCHAR(255)        NULL,
            [ScoringTeamId]                             VARCHAR(255)        NULL,
            [ScoringTeamAbbreviation]                   VARCHAR(255)        NULL,
            [ScoringTeamNickName]                       VARCHAR(255)        NULL,
            [StPlayType]                                VARCHAR(255)        NULL
        )
    END 
GO
