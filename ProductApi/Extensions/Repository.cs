namespace ProductApi.Extensions
{
    public static class Repository
    {
        public static string ProductsMerge => @"
MERGE INTO [dbo].[Products] AS target
USING (SELECT
            CAST(ID AS INT) AS Id,
            SKU,
            EAN,
            name AS Name,
            producer_name AS ManufacturerName,
            category AS Category,
            default_image AS ImgUrl,
            package_size as LogisticUnit
        FROM [dbo].[StgProducts]
		WHERE 
            ID <> '__empty_line__' AND 
            is_wire = 0 AND 
            shipping like '%24%') AS source
ON target.Id = source.Id
WHEN MATCHED AND (target.SKU <> source.SKU OR
                    target.EAN <> source.EAN OR
                    target.Name <> source.Name OR
                    target.ManufacturerName <> source.ManufacturerName OR
                    target.Category <> source.Category OR
                    target.ImgUrl <> source.ImgUrl OR
                    target.LogisticUnit <> source.LogisticUnit)
THEN
    UPDATE SET target.SKU = source.SKU,
                target.EAN = source.EAN,
                target.Name = source.Name,
                target.ManufacturerName = source.ManufacturerName,
                target.Category = source.Category,
                target.ImgUrl = source.ImgUrl,
                target.LogisticUnit = source.LogisticUnit
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, SKU, EAN, Name, ManufacturerName, Category, ImgUrl, LogisticUnit)
    VALUES (source.Id, source.SKU, source.EAN, source.Name, source.ManufacturerName, source.Category, source.ImgUrl, source.LogisticUnit);";

        public static string InventoryMerge => @"
MERGE INTO [dbo].[Inventories] AS target
USING (
    SELECT
        CAST(si.product_id AS INT) AS ProductId,
        CAST(si.qty AS DECIMAL(18,2)) AS Quantity,
        CAST(si.shipping_cost AS DECIMAL(18, 2)) AS ShippingCost
    FROM [dbo].[StgInventories] si
    INNER JOIN Products p ON CAST(si.product_id AS int) = p.Id
	WHERE shipping_cost <> ''
) AS source
ON target.ProductId = source.ProductId
WHEN MATCHED THEN
    UPDATE SET
        target.Quantity = source.Quantity,
        target.ShippingCost = source.ShippingCost
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ProductId, Quantity, ShippingCost)
    VALUES (source.ProductId, source.Quantity, source.ShippingCost);";

        public static string PricesMerge => @"
MERGE INTO Prices AS target
USING (
    SELECT
        sp.Column1 AS PriceId,
        sp.Column2 AS ProductSKU,
        CAST(REPLACE(sp.Column6,',','.') AS DECIMAL(18,2)) AS NetPrice
    FROM StgPrices sp
    INNER JOIN Products p ON p.SKU = sp.Column2
	WHERE sp.Column6 <> ''
) AS source
ON target.Id = source.PriceId
WHEN MATCHED THEN
    UPDATE SET
        target.ProductSKU = source.ProductSKU,
        target.NetPrice = source.NetPrice
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, ProductSKU, NetPrice)
    VALUES (source.PriceId, source.ProductSKU, source.NetPrice);";

        public static string SelectProduct(string sku) => @$"
SELECT 
	prod.NAME as 'Nazwa produktu',
	prod.EAN,
	prod.ManufacturerName as 'Nazwa producenta',
	prod.Category as 'Kategoria',
	prod.ImgUrl as 'URL do zdjęcia produktu',
	i.Quantity as 'Stan magazynowy',
	prod.LogisticUnit as 'Jednostka logistyczna produktu',
	p.NetPrice as 'Cena netto zakupu produktu',
	i.ShippingCost as 'Koszt dostawy'
FROM 
	[dbo].[Products] prod
	JOIN [dbo].[Inventories] i 
		ON prod.Id = i.ProductId
	JOIN [dbo].[Prices] p
		ON prod.SKU = p.ProductSKU
WHERE prod.SKU = '{sku}'";
    }
}
