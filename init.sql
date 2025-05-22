IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PilotDb')
BEGIN
    CREATE DATABASE PilotDb;
END
GO