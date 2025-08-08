using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ecommerce;
#pragma warning disable CS8604 // Possible null reference argument.
public class Methods
{



    public static async Task<string> GetSingleObjectFromTxt(string filePath, string searchtext,string searchtext2)
    {

        string[]? lines = await GetFileContent(filePath, null);
        // search for the line object
        int counter = 0;
        foreach (var item in lines)
        {
            if (item.Contains(searchtext) && lines.Contains(searchtext2))
            {
                return item;
            }
            counter++;
        }
        return default;
    }

    public static async Task<string> GetSingleObjectFromTxt(string filePath, string searchtext)
    {

        string[]? lines = await GetFileContent(filePath, null);
        // search for the line object
        int counter = 0;
        foreach (var item in lines)
        {
            if (item.Contains(searchtext))
            {
                return item;
            }
            counter++;
        }
        return default;
    }
    public static async Task Get_Print_AllProducts(string filePath)
    {
        string[]? lines = await GetFileContent(filePath, null);
        List<ProductDetails> prod_list = [];
        foreach (var item in lines)
        {
            ProductDetails product = JsonSerializer.Deserialize<ProductDetails>(item);
            prod_list.Add(product);
        }
        Console.Clear();
        Console.WriteLine(new string('-', 80));
        Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-25} {4,-30}",
            "Product-Id", "Product-Name", "Stock-Quantity", "Price-Per-item", "Shipping-duration");
        Console.WriteLine(new string('-', 80));
        foreach (var item in prod_list)
        {
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-25} {4,-30} ",
                item.ProductId, item.ProductName, item.StockQuantity, item.PricePerItem, item.ShippingDuration);
        }
        Console.WriteLine(new string('-', 80));
    }
    public static string GetIdIndexCount(string filepath, string string_to_append)
    {
        int counter = Convert.ToInt32(File.ReadAllText(filepath));

        return string_to_append + counter.ToString();
    }
    public static void UpdateIdIndexCount(string filepath)
    {
        int counter = Convert.ToInt32(File.ReadAllText(filepath));
        counter += 1;
        File.WriteAllText(filepath, counter.ToString());
    }
    //login method

    public static string Array_searcher()
    {
        return new NotImplementedException().ToString();
    }
    public static async Task<bool> Login(string customerId, string mobilenumber)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string[] lines = await GetFileContent("./data/users/customers.txt", null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        int counter = 0;
        foreach (var item in lines)
        {
            if (lines[counter].Contains(customerId) && lines[counter].Contains(mobilenumber))
            {
                return true;
            }
            counter++;
        }
        return false;
    }

    // Registration Method
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
        } while (Convert.ToInt32(walletbalance) <= 0);

        System.Console.WriteLine("Enter your email id : ");
        var emailid = System.Console.ReadLine();

        return new Customer(customername, city, mobilenumber, walletbalance, emailid, Methods.GetIdIndexCount("./data/counters/customer-id-counter.txt", "CID"));
    }

    public static async Task<string> GetFileContent(string filepath)
    {
        string lines = await File.ReadAllTextAsync(filepath);
        return lines;
    }
    //overload the method
    public static async Task<string[]?> GetFileContent(string filePath, string[] lines = null)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }
        lines = await File.ReadAllLinesAsync(filePath);
        return lines;
    }
    public static async Task WriteTofile(string filepath, string content)
    {
        await File.WriteAllTextAsync(filepath, content);
    }
    public static async Task WriteTofile(string filepath, string content, bool append = true)
    {
        await File.AppendAllLinesAsync(filepath, content.Split(','));
    }
    //overloaded method for writing to file with append  option
    public static async Task WriteTofile(string filepath, string[] content, bool append = true)
    {
        await File.AppendAllLinesAsync(filepath, content);
    }

    //overload the write method
    public static async Task WriteTofile(string filepath, string[] content)
    {
        await File.WriteAllLinesAsync(filepath, content);
    }

    // base event record method read
    public static async Task EventLog(string object_entity, string action, string timestamp, string actor_user, string comment_reason)
    {
        string[] event_to_record = [$"object = '{object_entity}' ; action = '{action}' ;  time = '{timestamp}' ; actor/user = '{actor_user}'  ; Comment/Reason = '{comment_reason}' ;"];
        bool append = true;
        await WriteTofile("./data/events/event.txt", event_to_record, append);
    }
    // overloaded crud event log method  partch update 
    public static async Task EventLog(string object_entity, string action, string timestamp, string actor_user, string comment_reason, string previous_State, string new_state)
    {
        string[] event_to_record = [$"object = '{object_entity}' ; action = '{action}' ;  time = '{timestamp}' ; actor/user = '{actor_user}'  ; Previous State = '{previous_State}' ;  New state = '{new_state}'  ; Comment/Reason = {comment_reason} ;"];
        bool append = true;
        await WriteTofile("./data/events/event.txt", event_to_record, append);
    }
    public static async Task EventLog(string object_entity, string action, string timestamp, string actor_user, string comment_reason, string previous_State, string new_state,string filepath)
    {
        string[] event_to_record = [$"object = '{object_entity}' ; action = '{action}' ;  time = '{timestamp}' ; actor/user = '{actor_user}'  ; Previous State = '{previous_State}' ;  New state = '{new_state}'  ; Comment/Reason = {comment_reason} ;"];
        bool append = true;
        await WriteTofile(filepath, event_to_record, append);
    }
// cutom history
    public static async Task EventLog(string orderid, string action, string timestamp, string actor_user, string status, string filepath)
    {
        string[] event_to_record = [$"object = '{orderid}' ; action = '{action}' ;  time = '{timestamp}' ; actor/user = '{actor_user}'  ; Status = '{status}' ;"];
        bool append = true;
        await WriteTofile(filepath, event_to_record, append);
    }
       
#pragma warning restore CS8604 // Possible null reference argument.
}
