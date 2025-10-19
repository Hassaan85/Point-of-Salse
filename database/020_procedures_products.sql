-- Product CRUD procedures
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.spProducts_GetAll', 'P') IS NOT NULL DROP PROCEDURE dbo.spProducts_GetAll;
GO
CREATE PROCEDURE dbo.spProducts_GetAll
  @Search NVARCHAR(200) = NULL
AS
BEGIN
  SET NOCOUNT ON;
  SELECT p.ProductId, p.Name, p.SKU, p.Barcode, p.Price, p.TaxRate, p.ReorderThreshold, p.IsActive,
         ISNULL(i.Quantity, 0) AS Quantity
  FROM dbo.Products p
  LEFT JOIN dbo.Inventory i ON p.ProductId = i.ProductId
  WHERE (@Search IS NULL OR p.Name LIKE '%' + @Search + '%' OR p.SKU LIKE '%' + @Search + '%' OR p.Barcode LIKE '%' + @Search + '%')
  ORDER BY p.Name;
END
GO

IF OBJECT_ID('dbo.spProducts_GetById', 'P') IS NOT NULL DROP PROCEDURE dbo.spProducts_GetById;
GO
CREATE PROCEDURE dbo.spProducts_GetById
  @ProductId INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT TOP 1 p.ProductId, p.Name, p.SKU, p.Barcode, p.Price, p.TaxRate, p.ReorderThreshold, p.IsActive,
         ISNULL(i.Quantity, 0) AS Quantity
  FROM dbo.Products p
  LEFT JOIN dbo.Inventory i ON p.ProductId = i.ProductId
  WHERE p.ProductId = @ProductId;
END
GO

IF OBJECT_ID('dbo.spProducts_Create', 'P') IS NOT NULL DROP PROCEDURE dbo.spProducts_Create;
GO
CREATE PROCEDURE dbo.spProducts_Create
  @Name NVARCHAR(200),
  @SKU NVARCHAR(100),
  @Barcode NVARCHAR(128) = NULL,
  @Price DECIMAL(18,2),
  @TaxRate DECIMAL(5,2) = 0,
  @ReorderThreshold INT = 0
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Products(Name, SKU, Barcode, Price, TaxRate, ReorderThreshold)
  VALUES(@Name, @SKU, @Barcode, @Price, @TaxRate, @ReorderThreshold);
  DECLARE @NewId INT = SCOPE_IDENTITY();
  IF NOT EXISTS(SELECT 1 FROM dbo.Inventory WHERE ProductId = @NewId)
  BEGIN
    INSERT INTO dbo.Inventory(ProductId, Quantity) VALUES(@NewId, 0);
  END
  SELECT @NewId AS ProductId;
END
GO

IF OBJECT_ID('dbo.spProducts_Update', 'P') IS NOT NULL DROP PROCEDURE dbo.spProducts_Update;
GO
CREATE PROCEDURE dbo.spProducts_Update
  @ProductId INT,
  @Name NVARCHAR(200),
  @SKU NVARCHAR(100),
  @Barcode NVARCHAR(128) = NULL,
  @Price DECIMAL(18,2),
  @TaxRate DECIMAL(5,2) = 0,
  @ReorderThreshold INT = 0,
  @IsActive BIT = 1
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Products
  SET Name = @Name,
      SKU = @SKU,
      Barcode = @Barcode,
      Price = @Price,
      TaxRate = @TaxRate,
      ReorderThreshold = @ReorderThreshold,
      IsActive = @IsActive
  WHERE ProductId = @ProductId;
  SELECT @ProductId AS ProductId;
END
GO

IF OBJECT_ID('dbo.spProducts_Delete', 'P') IS NOT NULL DROP PROCEDURE dbo.spProducts_Delete;
GO
CREATE PROCEDURE dbo.spProducts_Delete
  @ProductId INT
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Products SET IsActive = 0 WHERE ProductId = @ProductId;
END
GO
