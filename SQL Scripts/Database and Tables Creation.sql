-- Create the database if it doesn't already exist
IF NOT EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = N'FileRebuilderDB'
)
BEGIN
    CREATE DATABASE FileRebuilderDB;
END
GO

-- Switch to the new database
USE FileRebuilderDB;
GO

-- Create FileContent table if it doesn't already exist
IF NOT EXISTS (
    SELECT * 
    FROM sys.tables 
    WHERE name = N'FileContent'
)
BEGIN
    CREATE TABLE FileContent
    (
        Id INT IDENTITY PRIMARY KEY,
        Content VARBINARY(MAX) NOT NULL
    );
END
GO

-- Create FileMetadata table if it doesn't already exist
IF NOT EXISTS (
    SELECT * 
    FROM sys.tables 
    WHERE name = N'FileMetadata'
)
BEGIN
    CREATE TABLE FileMetadata
    (
        Id INT IDENTITY PRIMARY KEY,
        FileName NVARCHAR(255),
        OriginalPath NVARCHAR(500),
        ContentId INT FOREIGN KEY REFERENCES FileContent(Id)
    );
END
GO