-- Auth related stored procedures
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.spAuth_CreateUser', 'P') IS NOT NULL
  DROP PROCEDURE dbo.spAuth_CreateUser;
GO
CREATE PROCEDURE dbo.spAuth_CreateUser
  @Username NVARCHAR(100),
  @Password NVARCHAR(200),
  @FullName NVARCHAR(200),
  @RoleName NVARCHAR(50)
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @RoleId INT = (SELECT TOP 1 RoleId FROM dbo.Roles WHERE Name = @RoleName);
  IF @RoleId IS NULL
  BEGIN
    RAISERROR('Role not found', 16, 1);
    RETURN;
  END
  IF EXISTS(SELECT 1 FROM dbo.Users WHERE Username = @Username)
  BEGIN
    RAISERROR('Username already exists', 16, 1);
    RETURN;
  END
  DECLARE @Salt VARBINARY(128) = CRYPT_GEN_RANDOM(16);
  DECLARE @Hash VARBINARY(256) = HASHBYTES('SHA2_256', @Salt + CONVERT(VARBINARY(MAX), @Password));
  INSERT INTO dbo.Users(Username, PasswordHash, PasswordSalt, FullName, RoleId)
  VALUES(@Username, @Hash, @Salt, @FullName, @RoleId);
  SELECT SCOPE_IDENTITY() AS UserId;
END
GO

IF OBJECT_ID('dbo.spAuth_Login', 'P') IS NOT NULL
  DROP PROCEDURE dbo.spAuth_Login;
GO
CREATE PROCEDURE dbo.spAuth_Login
  @Username NVARCHAR(100),
  @Password NVARCHAR(200)
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @Salt VARBINARY(128);
  DECLARE @Hash VARBINARY(256);
  SELECT @Salt = PasswordSalt, @Hash = PasswordHash
  FROM dbo.Users WHERE Username = @Username;
  IF @Salt IS NULL
  BEGIN
    RAISERROR('Invalid credentials', 16, 1);
    RETURN;
  END
  DECLARE @Computed VARBINARY(256) = HASHBYTES('SHA2_256', @Salt + CONVERT(VARBINARY(MAX), @Password));
  IF @Computed <> @Hash
  BEGIN
    RAISERROR('Invalid credentials', 16, 1);
    RETURN;
  END
  SELECT TOP 1 u.UserId, u.Username, u.FullName, r.Name AS RoleName
  FROM dbo.Users u
  JOIN dbo.Roles r ON u.RoleId = r.RoleId
  WHERE u.Username = @Username;
END
GO
