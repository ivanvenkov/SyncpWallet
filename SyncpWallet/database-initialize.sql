-- Create the database
CREATE DATABASE SyncpWalletDB;
GO

-- Use the newly created database
USE SyncpWalletDB;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
);
GO

-- Create the Wallets table
CREATE TABLE Wallets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX),
    Currency NVARCHAR(3),
    Amount DECIMAL(18, 2),
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
GO

CREATE TABLE Transactions(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WalletId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    TransactionType NVARCHAR(10) NOT NULL CHECK (TransactionType IN ('Expense', 'Income')),
    Created DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (WalletId) REFERENCES Wallets(Id) ON DELETE CASCADE
);
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE sp_SeedUsers
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users)
    BEGIN

        INSERT INTO Users (Name)
        VALUES 
            ('TestUser1'),
            ('TestUser2'),
            ('TestUser3');
    END
    ELSE
    BEGIN
        PRINT 'Users table already seeded.';
    END;
END;
GO

EXEC sp_SeedUsers;
GO


CREATE OR ALTER PROCEDURE usp_WalletInsert
	@name NVARCHAR(MAX),
	@amount DECIMAl,
	@currency NVARCHAR(255),
    @userId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO dbo.Wallets(Name, Amount, Currency, UserId) VALUES(@name, @amount, @currency, @userId)
	SELECT CAST(SCOPE_IDENTITY() AS int) AS WalletId
END
GO

CREATE OR ALTER PROCEDURE usp_CheckWalletNameForDuplicates
    @UserId INT,
    @WalletName NVARCHAR(MAX)
AS
BEGIN
    SELECT 1
    FROM Wallets
    WHERE UserId = @UserId AND Name = @WalletName;
END;
GO

CREATE OR ALTER PROCEDURE usp_GetWalletsByUserId
    @UserId INT
AS
BEGIN
    SELECT Id, Name, Currency, Amount, UserId
    FROM Wallets
    WHERE UserId = @UserId;
END;
GO

CREATE OR ALTER PROCEDURE usp_AddTransaction
    @WalletId INT,
    @Amount DECIMAL(18, 2),
    @TransactionType NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @CurrentAmount DECIMAL(18, 2);
        SELECT @CurrentAmount = Amount FROM Wallets WHERE Id = @WalletId;

        IF @CurrentAmount IS NULL
        BEGIN
            RAISERROR ('Wallet not found.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF @TransactionType = 'Expense' AND @CurrentAmount < @Amount
        BEGIN
            RAISERROR ('Insufficient funds for this transaction.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        IF @TransactionType = 'Income'
        BEGIN
            UPDATE Wallets
            SET Amount = Amount + @Amount
            WHERE Id = @WalletId;
        END
        ELSE IF @TransactionType = 'Expense'
        BEGIN
            UPDATE Wallets
            SET Amount = Amount - @Amount
            WHERE Id = @WalletId;
        END;

        INSERT INTO Transactions (WalletId, Amount, TransactionType)
        VALUES (@WalletId, @Amount, @TransactionType);

        COMMIT TRANSACTION;

        SELECT Id, WalletId, Amount, TransactionType, Created
        FROM Transactions
        WHERE Id = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH;
END;
GO