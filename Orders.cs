using System;

namespace ecommerce;

public class Orders
{

    public string OrderID { get; set; }

    public string CustomerID { get; set; }

    public string ProductID { get; set; }

    public float? TotalPrice { get; set; }

    public string PurchaseDate { get; set; }

    public int Quantity { get; set; }

    public string OrderStatus { get; set; }

    public Orders(string customerID, string productid, float totalamount, string purchasedate, int quantity, string orderStatus)
    {
        OrderID = Methods.GetIdIndexCount("./data/counters/order-id-counter.txt", "OID");
        Methods.UpdateIdIndexCount("./data/counters/order-id-counter.txt");
        CustomerID = customerID;
        ProductID = productid;
        TotalPrice = totalamount;
        PurchaseDate = purchasedate;
        Quantity = quantity;
        OrderStatus = orderStatus;
    }

    public object Serializeorders()
    {
        return new { OrderID,CustomerID,ProductID,TotalPrice,PurchaseDate,Quantity,OrderStatus};
    }

}
