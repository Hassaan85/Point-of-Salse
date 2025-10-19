-- Seed roles and admin user
SET NOCOUNT ON;
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name = 'Admin') INSERT INTO dbo.Roles(Name) VALUES('Admin');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name = 'Manager') INSERT INTO dbo.Roles(Name) VALUES('Manager');
IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name = 'Cashier') INSERT INTO dbo.Roles(Name) VALUES('Cashier');

-- Create default admin user if not exists
IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Username = 'admin')
BEGIN
  EXEC dbo.spAuth_CreateUser @Username = 'admin', @Password = 'Admin@123', @FullName = 'System Administrator', @RoleName = 'Admin';
END
