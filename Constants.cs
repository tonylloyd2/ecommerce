using System;
using System.Text.Json;

namespace ecommerce;

public class Constants
{

    public static string[] Mainmenu = [
        "1. Customer Registration",
            "2. Login",
            "3. Exit"
    ];
    public static string[] IsloggedInMenu = [
        "a. Purchase",
        "b. OrderHistory",
        "c. CancelOrder",
        "d. WalletBalance",
        "e. WalletRecharge",
        "f. Exit"
    ];

    public static void MenuPrinter(string[] menu_arr)
    {
        // Console.Clear();
        System.Console.WriteLine(new string('*', 100));
        foreach (var item in menu_arr)
        {
            System.Console.WriteLine(item);
        }
        System.Console.WriteLine(new string('*', 100));

    }
    public static string[] GetMainMenuarr()
    {
        return Mainmenu;
    }
    public static string[] GetIsloggedInMenu()
    {
        return IsloggedInMenu;
    }
    public static void DefaultMessagePrinter(string message)
    {
        // Console.Clear();
        System.Console.WriteLine(new string('*', 100));
        System.Console.WriteLine(message);
        System.Console.WriteLine(new string('*', 100));
    }
    public static void DefaultMessagePrinter(string[] message)
    {
        Console.Clear();
        System.Console.WriteLine(new string('*', 100));
        foreach (var item in message)
        {
        System.Console.WriteLine(item);            
        }
        System.Console.WriteLine(new string('*', 100));
    }
    public static void DefaultMessagePrinter(string[] message,string searchtext)
    {
        Console.Clear();
        System.Console.WriteLine(new string('*', 100));
        foreach (var item in message)
        {
            if (item.Contains(searchtext))
            {
        System.Console.WriteLine(item);                            
            }
        }
        System.Console.WriteLine(new string('*', 100));
    }
    public static void InsertToFile(string filepath, Customer customer_obj)
    {
        string insert_obj_json = JsonSerializer.Serialize(customer_obj.SerializeCustomerData());
        File.AppendAllText(filepath, insert_obj_json + Environment.NewLine);

    }
public static void InsertToFile(string filepath, ProductDetails product_obj)
    {
        string insert_obj_json = JsonSerializer.Serialize(product_obj.SerializeProductData());
        File.AppendAllText(filepath, insert_obj_json + Environment.NewLine);

    }
    
    public static void InsertToFile(string filepath, Orders orders_obj)
    {
        string insert_obj_json = JsonSerializer.Serialize(orders_obj.Serializeorders());
        File.AppendAllText(filepath, insert_obj_json + Environment.NewLine);

    }
    public static string[]? RequestLoginCredentials()
    {
        string[] Logincredentials = new string[2];

        System.Console.WriteLine("Enter your CustomerId");
        Logincredentials[0] = System.Console.ReadLine();
        System.Console.WriteLine("Enter your MobileNumber");
        Logincredentials[1] = System.Console.ReadLine();

        return Logincredentials ?? null;
    }



}
