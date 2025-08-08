using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ecommerce;
using Newtonsoft.Json;

public class Program
{
    public static async Task Main(string[] s)
    {
        //initializers
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Customer customer_obj = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.


        do // first menu printer
        {
            Constants.MenuPrinter(Constants.GetMainMenuarr());
            var choice = System.Console.ReadLine();
            if (choice == "1")
            {// register customer and display the id
                System.Console.WriteLine("Handle user registration");
                customer_obj = Methods.Register();
                if (customer_obj != null)
                {
                    Constants.InsertToFile("./data/users/customers.txt", customer_obj);
                    await Methods.EventLog("Customer", "REGISTRATION", DateTime.Now.ToString(), $"{customer_obj.CustomerID}", "Registration success");
                    Constants.DefaultMessagePrinter($"Your user id  = {customer_obj.CustomerID}");
                }
            }
            else if (choice == "2")
            {// login customer and do second submenu

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string[] Logincredentials = Constants.RequestLoginCredentials();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (Logincredentials != null)
                {
                    if (await Methods.Login(Logincredentials[0], Logincredentials[1]))
                    {
                        //record the login event 
                        await Methods.EventLog("Customer", "LOGIN", DateTime.Now.ToString(), $"{Logincredentials[0]}", "login success");
                        // do while is looged in loop

                        Constants.DefaultMessagePrinter($"😎😎 Welcome {Logincredentials[0]} 😎😎 ");
                    
                        do
                        {
                            Constants.MenuPrinter(Constants.GetIsloggedInMenu());
                            System.Console.WriteLine("Enter your Destination : ");
                            if (char.TryParse(Console.ReadLine()?.ToLower(), out char result))
                            {

                            }

                            if (result == 'f')
                            {
                                Constants.DefaultMessagePrinter("Sad to see you leave 👎🏿👋👋");
                                break;
                            }
                            else if (result == 'a')
                            {

                                // Constants.DefaultMessagePrinter("Initiate purchase 👌👌");
                                await Methods.Get_Print_AllProducts("./data/products/products-list.txt");
                                Constants.DefaultMessagePrinter("Enter product id to purchase ");
                                string? productid = System.Console.ReadLine().ToUpper();
                                string line = await Methods.GetSingleObjectFromTxt("./data/products/products-list.txt", productid);
                                // check if productid is valid
                                if (line == null)
                                {
                                    Constants.DefaultMessagePrinter("Invalid product id entered 😒😒");
                                    await Methods.EventLog("Product", "Product id check", DateTime.Now.ToString(), Logincredentials[0], "Failed validation of product id");
                                    continue;
                                }
                                else
                                {
                                    ProductDetails? product = JsonConvert.DeserializeObject<ProductDetails>(line);
                                    List<ProductDetails> ls_product = [];
                                    ls_product.Add(product);
                                    Constants.DefaultMessagePrinter("Enter product count ");
                                    int Quantity = Convert.ToInt32(System.Console.ReadLine());
                                    var stockQuantity = ls_product[0].StockQuantity;
                                    System.Console.WriteLine($" quantity = {stockQuantity}");
                                    if (stockQuantity >= Quantity)
                                    {
                                        // Find the total amount 
                                        float totalamount = ls_product[0].PricePerItem * Quantity;
                                        // check wallet balance
                                        string user_obj_str = await Methods.GetSingleObjectFromTxt("./data/users/customers.txt", Logincredentials[0]);
                                        List<Customer> customer_list = [];
                                        customer_obj = JsonConvert.DeserializeObject<Customer>(user_obj_str);
                                        customer_list.Add(customer_obj);

                                        if (totalamount > customer_list[0].getWalletBalance())
                                        {
                                            Constants.DefaultMessagePrinter("Insufficient balance 😅😅 ");
                                            continue;
                                        }
                                        else
                                        {
                                            // record  the order purchase
                                            DateTime dateorder = DateTime.Now;
                                            Orders ord = new(customer_list[0].CustomerID, ls_product[0].ProductId, totalamount, dateorder.ToString(), Quantity, "Ordered");
                                            // serialize the object and record 
                                            dateorder.AddMonths(ls_product[0].ShippingDuration);

                                            Constants.DefaultMessagePrinter($"Order palced successfully\nYour order will be delivered on {dateorder}");
                                            Constants.InsertToFile("./data/orders/orders.txt", ord);
                                            await Methods.EventLog( ord.OrderID, "Order Made", DateTime.Now.ToString(),Logincredentials[0],"Order placed successfully","./data/events/orders-history.txt");

                                            ProductDetails new_ = product;
                                            new_.StockQuantity = product.StockQuantity - Quantity;
                                            string lines = File.ReadAllText("./data/products/products-list.txt");
                                            // deduct  quantity 


                                            
                                            string insert_prod = JsonConvert.SerializeObject(new_.SerializeProductData());
                                            System.Console.WriteLine("new = "+ insert_prod);
                                            System.Console.WriteLine("prev = "+ line);
                                            lines = lines.Replace(line,insert_prod);
                                            File.WriteAllText("./data/products/products-list.txt", lines);
                                            await Methods.EventLog(product.ProductId, "Update", DateTime.Now.ToString(), "system", "Product updated", $"previous stock = {ls_product[0].StockQuantity}", $"new stock = {ls_product[0].StockQuantity - Quantity}");
                                            string get_customer = await Methods.GetSingleObjectFromTxt("./data/users/customers.txt", Logincredentials[0]);
                                            string get_customers = await Methods.GetFileContent("./data/users/customers.txt");
                                            customer_obj = JsonConvert.DeserializeObject<Customer>(get_customer);                                
                                            customer_obj.WalletBalance = customer_obj.getWalletBalance() - (int)totalamount;
                                            string new_wallet = JsonConvert.SerializeObject(customer_obj);
                                            get_customers = get_customers.Replace(get_customer,new_wallet);
                                            await File.WriteAllTextAsync("./data/users/customers.txt",get_customers);

                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        Constants.DefaultMessagePrinter($"The required count is not available , \nthe current product stock count is {stockQuantity}");
                                        await Methods.EventLog("Order purchase", "get quntity stock", DateTime.Now.ToString(), Logincredentials[0], $"user requested {Quantity} items but only {stockQuantity} is avalible for productid {ls_product[0].ProductId}");
                                        // continue;
                                    }



                                }

                                break;
                            }

                            else if (result == 'b')
                            {
                                Constants.DefaultMessagePrinter("Orders history 📅");
                                string[] orders = await Methods.GetFileContent("./data/orders/orders.txt", null);
                                Constants.DefaultMessagePrinter(orders, Logincredentials[0]);
                                continue;
                            }
                            else if (result == 'c')
                            {
                                Constants.DefaultMessagePrinter("Cancel order ❌❌");

                                string[]? orders_user = await Methods.GetFileContent("./data/orders/orders.txt", null);
                                string order_lines = await Methods.GetFileContent("./data/orders/orders.txt");
                                Constants.DefaultMessagePrinter(orders_user, Logincredentials[0]);
                                System.Console.WriteLine("Enter order id to cancel ");
                                string _orderidCancel = System.Console.ReadLine();

                                string order_line_tocancel = await Methods.GetSingleObjectFromTxt("./data/orders/orders.txt", _orderidCancel);
                                if (order_line_tocancel == null)
                                {
                                    Constants.DefaultMessagePrinter("Invalid id or order status is already canceled");
                                    continue; 
                                }
                                
                                // stus of the order to Cancelled
                                Orders? ord_line_obj = JsonConvert.DeserializeObject<Orders>(order_line_tocancel);
                                ord_line_obj.OrderStatus = "Canceled";
                                string ord_cid = ord_line_obj.CustomerID;
                                string ord_pid = ord_line_obj.ProductID;
                                string ord_oid = ord_line_obj.OrderID;
                                int ord_quatity = ord_line_obj.Quantity;
                                float ord_price = (float)ord_line_obj.TotalPrice;
                                // update orders lines
                                string order_line_to_update = JsonConvert.SerializeObject(ord_line_obj);
                                // get the orders array and update it and write to files
                                order_lines = order_lines.Replace(order_line_tocancel,order_line_to_update);
                                File.WriteAllText("./data/orders/orders.txt", order_lines);

                                //update the  stock quantity
                                string[]? products_arr = await Methods.GetFileContent("./data/products/products-list.txt", null);
                                string prod_lines = await Methods.GetFileContent("./data/products/products-list.txt");
                                string prod_line_previous = "";
                                string prod_line_update = "";

                                int i = 0;
                                foreach (var item in products_arr)
                                {
                                    // get the product id line 
                                    if (item.Contains(ord_pid))
                                    {
                                        prod_line_previous = item;
                                        ProductDetails? prod_obj = JsonConvert.DeserializeObject<ProductDetails>(item);
                                        prod_obj.StockQuantity += ord_line_obj.Quantity;
                                        prod_line_update = JsonConvert.SerializeObject(prod_obj);
                                        break;
                                    }
                                }
                                prod_lines = prod_lines.Replace(prod_line_previous,prod_line_update);
                                // Write to file 
                                // System.Console.WriteLine($"prev prod = {prod_line_previous}");
                                // System.Console.WriteLine($"Update prod = {prod_line_update}");
                                await File.WriteAllTextAsync("./data/products/products-list.txt", prod_lines);

                                // refund the user wallet

                                string get_customer = await Methods.GetSingleObjectFromTxt("./data/users/customers.txt", Logincredentials[0]);
                                string get_customers = await Methods.GetFileContent("./data/users/customers.txt");
                                customer_obj = JsonConvert.DeserializeObject<Customer>(get_customer);
                                System.Console.WriteLine($"user object = {get_customer}");
                                System.Console.WriteLine($"allet balance {customer_obj.getWalletBalance()}");
                                System.Console.WriteLine($"new wallet balance {customer_obj.getWalletBalance() + ord_price}");
                                customer_obj.WalletBalance = customer_obj.getWalletBalance() + (int)ord_price;

                                string new_wallet = JsonConvert.SerializeObject(customer_obj);

                                get_customers = get_customers.Replace(get_customer,new_wallet);
                                await File.WriteAllTextAsync("./data/users/customers.txt",get_customers);

                                continue;
                            }
                            else if (result == 'd')
                            {
                                Constants.DefaultMessagePrinter("Wallet Ballance 🤑🤑");
                                string get_customer = await Methods.GetSingleObjectFromTxt("./data/users/customers.txt", Logincredentials[0]);
                                customer_obj = JsonConvert.DeserializeObject<Customer>(get_customer);                                
                                System.Console.WriteLine($"allet balance {customer_obj.getWalletBalance()}");

                                continue;
                            }
                            else if (result == 'e')
                            {
                                Constants.DefaultMessagePrinter("Wallet Recharge ➕💰\nEnter amount to recharge");
                                int amount = Convert.ToInt32(System.Console.ReadLine());

                                string get_customer = await Methods.GetSingleObjectFromTxt("./data/users/customers.txt", Logincredentials[0]);
                                string get_customers = await Methods.GetFileContent("./data/users/customers.txt");
                                customer_obj = JsonConvert.DeserializeObject<Customer>(get_customer);
                                System.Console.WriteLine($"user object = {get_customer}");
                                System.Console.WriteLine($"allet balance {customer_obj.getWalletBalance()}");
                                System.Console.WriteLine($"new wallet balance {customer_obj.getWalletBalance() + amount}");
                                customer_obj.WalletBalance = customer_obj.getWalletBalance() + amount;
                                string new_wallet = JsonConvert.SerializeObject(customer_obj);
                                get_customers = get_customers.Replace(get_customer,new_wallet);
                                await File.WriteAllTextAsync("./data/users/customers.txt",get_customers);

                                continue;
                            }
                            else if ( result == 'f')
                            {
                                break;
                            }

                            else
                            {
                                continue;
                            }


                        }
                        while (true);
                    }
                    else
                    {
                        //record the login event 
                        await Methods.EventLog("Customer", "LOGIN", DateTime.Now.ToString(), $"{Logincredentials[0]}", "login failed");
                        Constants.DefaultMessagePrinter("Invalid login Credentials 😒😒😒😒");
                        continue;

                    }
                }

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