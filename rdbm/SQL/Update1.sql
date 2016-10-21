﻿/*
Deployment script for C:\USERS\BARLD\DOCUMENTS\VISUAL STUDIO 2015\PROJECTS\RDBM\RDBM\EMPLOYE.MDF

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO


GO
PRINT N'Dropping [dbo].[Employee_Position]...';


GO
ALTER TABLE [dbo].[Position] DROP CONSTRAINT [Employee_Position];


GO
PRINT N'Dropping [dbo].[Project_Position]...';


GO
ALTER TABLE [dbo].[Position] DROP CONSTRAINT [Project_Position];


GO
/*
The column [dbo].[Position].[ProjectID] is being dropped, data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[Position]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Position] (
    [PositionName] VARCHAR (100) NOT NULL,
    [Description]  TEXT          NULL,
    [HourFee]      MONEY         NOT NULL,
    [BSN]          INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([BSN] ASC, [PositionName] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Position])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_Position] ([BSN], [PositionName], [Description], [HourFee])
        SELECT   [BSN],
                 [PositionName],
                 [Description],
                 [HourFee]
        FROM     [dbo].[Position]
        ORDER BY [BSN] ASC, [PositionName] ASC;
    END

DROP TABLE [dbo].[Position];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Position]', N'Position';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[Employee_Position]...';


GO
ALTER TABLE [dbo].[Position] WITH NOCHECK
    ADD CONSTRAINT [Employee_Position] FOREIGN KEY ([BSN]) REFERENCES [dbo].[Employee] ([BSN]);


GO
PRINT N'Checking existing data against newly created constraints';


GO


GO
ALTER TABLE [dbo].[Position] WITH CHECK CHECK CONSTRAINT [Employee_Position];


GO
PRINT N'Update complete.';


GO

CREATE TABLE [dbo].[ProjectPosition](
	PositionName VARCHAR(100)	NOT NULL,
	BSN			INT				NOT NULL,
	ProjectID	INT				NOT NULL,
	PRIMARY KEY CLUSTERED ([BSN] ASC, [PositionName] ASC, [ProjectID] ASC),
	CONSTRAINT ProjectPosition_Project FOREIGN KEY (ProjectID) REFERENCES Project(ProjectID),
	CONSTRAINT ProjectPosition_Position FOREIGN KEY (BSN, PositionName) REFERENCES Position(BSN, PositionName)
);

GO