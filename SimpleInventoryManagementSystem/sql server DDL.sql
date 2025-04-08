CREATE DATABASE InventoryDb;
GO
USE InventoryDb;
GO

CREATE TABLE Products (
    Name NVARCHAR(100) PRIMARY KEY,
    Quantity INT NOT NULL,
    Price INT NOT NULL
);