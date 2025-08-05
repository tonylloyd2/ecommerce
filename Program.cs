using System;
using ecommerce;

public class Program
{
    public static void Main(string[] s)
    {
        //initializers

        Customer customer_obj = null;
        do // first menu printer
        {

            Constants.MenuPrinter(Constants.GetMainMenuarr());
            var choice = System.Console.ReadLine();

            if (choice == "1")
            {// register customer and display the id
                System.Console.WriteLine("Handle user registration");
                customer_obj = Customer.Register();
                if (customer_obj != null)
                {
                    Constants.InsertToFile("./data/users/customers.txt", customer_obj);
                    
                    System.Console.WriteLine($"Your user id  = {customer_obj.CustomerID}");
                }
                continue;
            }
            else if (choice == "2")
            {// login customer and do second submenu

                Constants.MenuPrinter(Constants.GetIsloggedInMenu());
                continue;
            }
            else if (choice == "3")
            {
                Constants.DefaultMessagePrinter("byeeee  👌👌👌👌");
                break;
            }
            else
            {
                Constants.DefaultMessagePrinter("Invalid input 😒😒😒😒");
                continue;
            }



        } while (true);
    }
}