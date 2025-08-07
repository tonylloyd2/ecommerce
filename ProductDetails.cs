using System;

namespace ecommerce;

public class ProductDetails
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int StockQuantity { get; set; }
    public float PricePerItem { get; set; }
    public int ShippingDuration { get; set; }

    public ProductDetails(string productName, int stockQuantity, float pricePerItem, int shippingDuration)
    {
        ProductId = Methods.GetIdIndexCount("./data/counters/product-id-counter.txt", "PID");
        Methods.UpdateIdIndexCount("./data/counters/product-id-counter.txt");
        ProductName = productName;
        StockQuantity = stockQuantity;
        PricePerItem = pricePerItem;
        ShippingDuration = shippingDuration;
    }

    public object SerializeProductData()
    {
        return new { ProductId, ProductName, StockQuantity, PricePerItem,ShippingDuration};
    }


}
