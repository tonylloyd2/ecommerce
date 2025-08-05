using System;
using System.Diagnostics.Contracts;

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

    public string? CustomerName { get; set; }
    public string? City { get; set; }
    private string? MobileNumber { get; set; }
    private int WalletBalance { get; set; }
    private string EmailID { get; set; }

    public string CustomerID { get; set; }

    public Customer(string customername, string city, string mobilenumber, int walletbalance, string emailid)
    {
        CustomerName = customername;
        City = city;
        MobileNumber = mobilenumber;
        WalletBalance = walletbalance;
        EmailID = emailid;
        CustomerID = GetCustomerIdIndexCount("./data/counters/customer-id-counter.txt");
        UpdateCustomerIdIndexCount("./data/counters/customer-id-counter.txt");
    }

    public static string GetCustomerIdIndexCount(string filepath)
    {
        int counter = Convert.ToInt32(File.ReadAllText(filepath));

        return "CID" + counter.ToString();
    }
    public static void UpdateCustomerIdIndexCount(string filepath)
    {
        int counter = Convert.ToInt32(File.ReadAllText(filepath));
        counter += 1;
        File.WriteAllText(filepath, counter.ToString());


    }
    public static Customer Register()
    {
        System.Console.WriteLine("Enter your full names : ");
        var customername = System.Console.ReadLine();

        System.Console.WriteLine("Enter your city : ");
        var city = System.Console.ReadLine();

        System.Console.WriteLine("Enter your mobile number : ");
        var mobilenumber = System.Console.ReadLine();

        int walletbalance;
        do
        {
         System.Console.WriteLine("Enter initial wallet balance : ");
         walletbalance = int.Parse(Console.ReadLine());
            // walletbalance.All(char.IsDigit) == false;
        } while (Convert.ToInt32(walletbalance) <= 0 );
        
        System.Console.WriteLine("Enter your email id : ");
        var emailid = System.Console.ReadLine();

        return new Customer(customername,city,mobilenumber,walletbalance,emailid);
    }

    

    public object SerializeCustomerData()
    {
        return new { CustomerID, CustomerName, City, MobileNumber, WalletBalance, EmailID };
    }

}
