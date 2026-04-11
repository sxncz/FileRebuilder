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

-- Add Hash Column
IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE Name = N'Hash' 
      AND Object_ID = Object_ID(N'FileContent')
)
BEGIN
    ALTER TABLE FileContent
    ADD Hash NVARCHAR(64);
END
GO

-- Add unique constraint
IF NOT EXISTS (
    SELECT * 
    FROM sys.indexes 
    WHERE name = N'UQ_FileContent_Hash'
      AND object_id = OBJECT_ID(N'FileContent')
)
BEGIN
    ALTER TABLE FileContent
    ADD CONSTRAINT UQ_FileContent_Hash UNIQUE (Hash);
END
GO