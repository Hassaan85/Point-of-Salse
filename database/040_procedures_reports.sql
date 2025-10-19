-- Basic reports
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID('dbo.spReports_SalesSummary', 'P') IS NOT NULL DROP PROCEDURE dbo.spReports_SalesSummary;
GO
CREATE PROCEDURE dbo.spReports_SalesSummary
  @From DATETIME2(3),
  @To DATETIME2(3)
AS
BEGIN
  SET NOCOUNT ON;
  SELECT CAST(CONVERT(date, CreatedAt) AS DATE) AS [Date],
         COUNT(*) AS NumSales,
         SUM(Subtotal) AS Subtotal,
         SUM(Discount) AS Discount,
         SUM(Tax) AS Tax,
         SUM(Total) AS Total
  FROM dbo.Sales
  WHERE CreatedAt >= @From AND CreatedAt < @To
  GROUP BY CONVERT(date, CreatedAt)
  ORDER BY [Date];
END
GO

IF OBJECT_ID('dbo.spReports_ProductPerformance', 'P') IS NOT NULL DROP PROCEDURE dbo.spReports_ProductPerformance;
GO
CREATE PROCEDURE dbo.spReports_ProductPerformance
  @From DATETIME2(3),
  @To DATETIME2(3)
AS
BEGIN
  SET NOCOUNT ON;
  SELECT si.ProductId, p.Name,
         SUM(si.Quantity) AS QuantitySold,
         SUM(si.LineTotal) AS Revenue
  FROM dbo.SaleItems si
  JOIN dbo.Sales s ON si.SaleId = s.SaleId
  JOIN dbo.Products p ON si.ProductId = p.ProductId
  WHERE s.CreatedAt >= @From AND s.CreatedAt < @To
  GROUP BY si.ProductId, p.Name
  ORDER BY Revenue DESC;
END
GO
