
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/01/2015 18:02:38
-- Generated from EDMX file: E:\User\Documents\Visual Studio 2013\Projects\LocalSearchEngine\LocalSearchEngine\DataAccess\FileManagementDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FileManagement];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FileImage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentImages] DROP CONSTRAINT [FK_FileImage];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DocumentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentFiles];
GO
IF OBJECT_ID(N'[dbo].[DocumentImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentImages];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DocumentFiles'
CREATE TABLE [dbo].[DocumentFiles] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ItemType] nvarchar(max)  NOT NULL,
    [FolderPath] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [Size] bigint  NOT NULL
);
GO

-- Creating table 'DocumentImages'
CREATE TABLE [dbo].[DocumentImages] (
    [Id] uniqueidentifier  NOT NULL,
    [Width] int  NOT NULL,
    [Height] int  NOT NULL,
    [PixelFormat] nvarchar(max)  NOT NULL,
    [PFingerPrint] varbinary(max)  NULL,
    [FileId] uniqueidentifier  NOT NULL,
    [IsWithinFile] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DocumentFiles'
ALTER TABLE [dbo].[DocumentFiles]
ADD CONSTRAINT [PK_DocumentFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentImages'
ALTER TABLE [dbo].[DocumentImages]
ADD CONSTRAINT [PK_DocumentImages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [FileId] in table 'DocumentImages'
ALTER TABLE [dbo].[DocumentImages]
ADD CONSTRAINT [FK_FileImage]
    FOREIGN KEY ([FileId])
    REFERENCES [dbo].[DocumentFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FileImage'
CREATE INDEX [IX_FK_FileImage]
ON [dbo].[DocumentImages]
    ([FileId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------