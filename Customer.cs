using System;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;

namespace ecommerce;
/*
Customer Details:
Properties: 
CustomerID (Auto Increment -CID3001)
CustomerName
City
MobileNumber
WalletBalance
EmailID
Methods: 
WalletRecharge: Get the recharge amount through parameters and update wallet balance.
DeductBalance: Get the deducted amount through parameters and update wallet balance.
*/
public class Customer
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Converting null literal or possible null value to non-nullable type.


    public  string CustomerName { get; set; }
    public string City { get; set; }
    private string MobileNumber { get; set; }
    private  int WalletBalance { get; set; }
    private string EmailID { get; set; }

    public string CustomerID { get; set; }

    public Customer(string customername, string city, string mobilenumber, int walletbalance, string emailid,string customerID)
    {
        CustomerName = customername;
        City = city;
        MobileNumber = mobilenumber;
        WalletBalance = walletbalance;
        EmailID = emailid;
        CustomerID = customerID;
        Methods.UpdateIdIndexCount("./data/counters/customer-id-counter.txt");
    }
    public  int getWalletBalance()
    {
        return WalletBalance;
    }

    public object SerializeCustomerData()
    {
        return new { CustomerID, CustomerName, City, MobileNumber, WalletBalance, EmailID };
    }

    
    public static async Task WalletRecharge(string CustomerID, int recharge_amount,int walletbalance)
    {
        string[] users_arr = await Methods.GetFileContent("./data/users/customers.txt", null);
        Customer customer_obj = null;

        int counter = 0;
        foreach (var line in users_arr)
        {
            if (users_arr[counter].Contains(CustomerID))
            {
                customer_obj = JsonSerializer.Deserialize<Customer>(users_arr[counter]);
                walletbalance += recharge_amount;
                users_arr[counter] = JsonSerializer.Serialize<Customer>(customer_obj);
                break;
            }
        }
        // update the content to the user in db file
        Methods.WriteTofile("./data/users/customers.txt", users_arr);

    }
    // public static  string getUsersDeatqails()
    // {
    //     return CustomerName.ToString();
    // }

#pragma warning restore CS8600
#pragma warning restore CS8602

}
