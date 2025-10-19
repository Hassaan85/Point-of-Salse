-- Sales checkout with JSON inputs
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.spSales_Checkout', 'P') IS NOT NULL DROP PROCEDURE dbo.spSales_Checkout;
GO
CREATE PROCEDURE dbo.spSales_Checkout
  @UserId INT,
  @CustomerId INT = NULL,
  @ItemsJson NVARCHAR(MAX), -- [{"ProductId":1,"Quantity":2,"Discount":0}]
  @PaymentsJson NVARCHAR(MAX), -- [{"Method":"Cash","Amount":10.00}]
  @SaleDiscount DECIMAL(18,2) = 0 -- absolute discount
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  DECLARE @Subtotal DECIMAL(18,2) = 0,
          @Tax DECIMAL(18,2) = 0,
          @Total DECIMAL(18,2) = 0;

  -- Temp table for items
  CREATE TABLE #Items (
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    LineDiscount DECIMAL(18,2) NOT NULL,
    TaxAmount DECIMAL(18,2) NOT NULL,
    LineTotal DECIMAL(18,2) NOT NULL
  );

  -- Validate items and compute amounts
  ;WITH Parsed AS (
    SELECT CAST(JSON_VALUE([value],'$.ProductId') AS INT) AS ProductId,
           CAST(JSON_VALUE([value],'$.Quantity') AS INT) AS Quantity,
           CAST(ISNULL(JSON_VALUE([value],'$.Discount'),'0') AS DECIMAL(18,2)) AS Discount
    FROM OPENJSON(@ItemsJson)
  )
  INSERT INTO #Items(ProductId, Quantity, UnitPrice, LineDiscount, TaxAmount, LineTotal)
  SELECT p.ProductId,
         i.Quantity,
         p.Price,
         i.Discount,
         CAST(ROUND(((p.Price * i.Quantity - i.Discount) * (p.TaxRate/100.0)),2) AS DECIMAL(18,2)) AS TaxAmount,
         CAST(ROUND((p.Price * i.Quantity - i.Discount) + ((p.Price * i.Quantity - i.Discount) * (p.TaxRate/100.0)),2) AS DECIMAL(18,2)) AS LineTotal
  FROM Parsed i
  JOIN dbo.Products p ON p.ProductId = i.ProductId AND p.IsActive = 1;

  SELECT @Subtotal = CAST(ROUND(SUM(UnitPrice * Quantity),2) AS DECIMAL(18,2))
  FROM #Items;

  SELECT @Tax = CAST(ROUND(SUM(TaxAmount),2) AS DECIMAL(18,2)) FROM #Items;

  SET @Total = CAST(ROUND((SELECT SUM(LineTotal) FROM #Items) - @SaleDiscount, 2) AS DECIMAL(18,2));

  BEGIN TRAN;

  INSERT INTO dbo.Sales(UserId, CustomerId, Subtotal, Discount, Tax, Total)
  VALUES(@UserId, @CustomerId, @Subtotal, @SaleDiscount, @Tax, @Total);
  DECLARE @SaleId INT = SCOPE_IDENTITY();

  INSERT INTO dbo.SaleItems(SaleId, ProductId, Quantity, UnitPrice, Discount, TaxAmount, LineTotal)
  SELECT @SaleId, ProductId, Quantity, UnitPrice, LineDiscount, TaxAmount, LineTotal
  FROM #Items;

  -- Decrement inventory and write history
  UPDATE i
  SET i.Quantity = i.Quantity - it.Quantity,
      i.UpdatedAt = SYSUTCDATETIME()
  FROM dbo.Inventory i
  JOIN #Items it ON it.ProductId = i.ProductId;

  INSERT INTO dbo.StockHistory(ProductId, ChangeQty, Reason, ReferenceId)
  SELECT ProductId, -Quantity, 'SALE', @SaleId FROM #Items;

  -- Payments
  ;WITH P AS (
    SELECT JSON_VALUE([value],'$.Method') AS Method,
           CAST(JSON_VALUE([value],'$.Amount') AS DECIMAL(18,2)) AS Amount
    FROM OPENJSON(@PaymentsJson)
  )
  INSERT INTO dbo.Payments(SaleId, Method, Amount)
  SELECT @SaleId, Method, Amount FROM P;

  COMMIT;

  -- Return sale summary
  SELECT @SaleId AS SaleId, @Subtotal AS Subtotal, @SaleDiscount AS Discount, @Tax AS Tax, @Total AS Total;
END
GO
