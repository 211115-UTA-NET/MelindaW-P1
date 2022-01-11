using PlainOldStoreApp.Ui.Dots;
using PlainOldStoreApp.Ui.Dtos;
using System.Text.Json;

namespace PlainOldStoreApp.Ui
{
    internal class PlainOldShop
    {
        internal async static Task PlaceOrder(Uri server)
        {
            ICustomerService customerService = new CustomerService(server);
            IStoreService storeService = new StoreService(server);
            IProductService productService = new ProductService(server);
            IOrderService orderService = new OrderService(server);
            CustomerHandler customerHandler = new CustomerHandler(customerService);
            ProductsList productsList = new ProductsList(productService);
            StoreList storeList = new StoreList(storeService);
            bool isOrdering = true;
            while (isOrdering)
            {
                string email = "";
                Console.WriteLine("Please enter in the customer's name or email.");
                string? nameOrEmail = Console.ReadLine()?.Trim();
                Console.WriteLine();
                string? tryAgain;
                Tuple<string, string> nameOrEmailTuple = ValidateInput.ValidateNameOrEmail(nameOrEmail);
                bool isValidateingNameOrEmail = true;
                while (isValidateingNameOrEmail)
                {
                    if (nameOrEmailTuple.Item1 == "false")
                    {
                        Console.WriteLine("You did not enter in a full name or email.");
                        Console.WriteLine("Would you like to try again?");
                        Console.WriteLine("Yes(Y) or No(N)?");
                        tryAgain = Console.ReadLine()?.Trim().ToLower();
                        Console.WriteLine();
                        if (tryAgain == "yes" || tryAgain == "y")
                        {
                            Console.WriteLine("Please enter full name or email.");
                            nameOrEmail = Console.ReadLine()?.Trim();
                            Console.WriteLine();
                            nameOrEmailTuple = ValidateInput.ValidateNameOrEmail(nameOrEmail);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        isValidateingNameOrEmail = false;
                    }
                }
                if (nameOrEmailTuple.Item1 == "false")
                {
                    break;
                }
                if (nameOrEmailTuple.Item1 == "email")
                {
                    bool foundEmail;
                    try
                    {
                        foundEmail = await customerHandler.IsEmailFound(nameOrEmailTuple.Item2);
                    }
                    catch (ServerException)
                    {
                        Console.WriteLine("Unable to connect to server.");
                        break;
                    }

                    if (foundEmail)
                    {
                        email = nameOrEmailTuple.Item2.ToUpper();
                        Console.WriteLine("You have an account.");
                        Console.WriteLine("Please continue with your order.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No account has that email please lookup by name or create an account.");
                        Console.WriteLine();
                        break;
                    }
                }
                else
                {
                    List<Customer> foundCustomers;
                    try
                    {
                        foundCustomers = await customerHandler.GetCustomerByName(nameOrEmailTuple.Item1, nameOrEmailTuple.Item2);
                    }
                    catch(ServerException)
                    {
                        Console.WriteLine("Unable to connect to server");
                        break;
                    }
                    string? emailLookUP;
                    Tuple<string, string> emailTuple;
                    if (foundCustomers.Count >= 1)
                    {
                        Console.WriteLine("We have found an account under that name.");
                        Console.WriteLine("Please enter email to verify your account.");
                        emailLookUP = Console.ReadLine();
                        Console.WriteLine();
                        emailTuple = ValidateInput.ValidateEmail(emailLookUP);
                        bool isValidateingEmail = true;
                        while (isValidateingEmail)
                        {
                            if (emailTuple.Item1 == "false")
                            {
                                Console.WriteLine("Your did not enter in an email.");
                                Console.WriteLine("Would you like to try again?");
                                Console.WriteLine("Yes(Y) or No(N)?");
                                tryAgain = Console.ReadLine()?.Trim().ToLower();
                                Console.WriteLine();
                                if (tryAgain == "yes" || tryAgain == "y")
                                {
                                    Console.WriteLine("Please enter your email.");
                                    emailLookUP = Console.ReadLine()?.Trim();
                                    Console.WriteLine();
                                    emailTuple = ValidateInput.ValidateEmail(emailLookUP);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                isValidateingEmail = false;
                            }
                        }
                        if (emailTuple.Item1 == "false")
                        {
                            break;
                        }
                        foreach (Customer customer in foundCustomers)
                        {
                            if (customer.Email == emailTuple.Item2)
                            {
                                Console.WriteLine("You have an account please continue with your order.");
                                Console.WriteLine();
                                email = "anEmailWasFound";
                            }
                        }
                        if (email != "anEmailWasFound")
                        {
                            Console.WriteLine("No account was found with that email.");
                            break;
                        }
                        email = emailTuple.Item2;
                    }
                    else
                    {
                        Console.WriteLine("No account was found with this name.");
                        Console.WriteLine();
                        break;
                    }
                }

                Guid customerId;
                try
                {
                    customerId = await customerHandler.GetCustomerIdByEmail(email);
                }
                catch (ServerException)
                {
                    Console.WriteLine("Unable to connect to server");
                    break;
                }
                
                Dictionary<int, string> stores = await storeList.GetAllStores();

                if(stores.ContainsKey(-1))
                {
                    break;
                }

                foreach (var s in stores)
                {
                    Console.WriteLine(s);
                }
                int numberOfStores = stores.Count;
                int storeLocation = 0;
                bool isInt = false;
                while (!isInt)
                {
                    isInt = int.TryParse(Console.ReadLine(), out storeLocation);
                    if (isInt == false || numberOfStores < 0 || storeLocation > numberOfStores || storeLocation <= 0)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine("Please choose a store location.");
                        Console.WriteLine();
                        isInt = false;
                    }
                }
                List<Product> allStoreProducts;
                try
                {
                    allStoreProducts = await productsList.GetAllStoreProducts(storeLocation);
                }
                catch(ServerException)
                {
                    Console.WriteLine("Unable to connect to server");
                    break;
                }
                bool isOrderingProducts = true;
                List<Product> amountOfProductsOrdered = new List<Product>();
                while (isOrderingProducts)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Please select the item number for the item you would like to buy from our {stores[storeLocation]} location.");
                    Console.WriteLine();
                    Console.WriteLine(string.Format("   | {0,3} {1,28} {2,-30} | {3,-13} | {4,8}", "Product Name", "|", "Product Description", "Product Price", "Quantity"));

                    foreach (Product product in allStoreProducts)
                    {
                        Console.WriteLine(string.Format("{0,-2} | {1,-39} | {2,-30} | {3:C2} {4,8} {5,8}"
                            , product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice, "|", product.ProductQuantiy));
                    }
                    int numberOfProducts = allStoreProducts.Count;
                    int itemSelection = 0;
                    isInt = false;
                    while (!isInt)
                    {
                        isInt = int.TryParse(Console.ReadLine(), out itemSelection);
                        if (isInt == false || itemSelection < 1 || itemSelection > numberOfProducts)
                        {
                            Console.WriteLine("Invalid input.");
                            Console.WriteLine("Please choose an item number.");
                            Console.WriteLine();
                            isInt = false;
                        }
                    }
                    string? productName = "";
                    foreach (Product product in allStoreProducts)
                    {
                        if (product.ProductId == itemSelection)
                        {
                            productName = product.ProductName;
                        }
                    }
                    int productAmount = 0;
                    Console.WriteLine($"How many {productName} would you like?");
                    isInt = false;
                    while (!isInt)
                    {
                        isInt = int.TryParse(Console.ReadLine(), out productAmount);
                        if (isInt == false || productAmount < 1 || productAmount > 5)
                        {
                            Console.WriteLine("Invalid input.");
                            Console.WriteLine("Please enter a amount between 1 and 5.");
                            Console.WriteLine();
                            isInt = false;
                        }
                    }
                    int? sum = 0;
                    if (amountOfProductsOrdered.Count > 0)
                    {
                        foreach (Product product in amountOfProductsOrdered)
                        {
                            if (product.ProductId == itemSelection)
                            {
                                sum += product.ProductQuantiy;
                            }
                        }
                    }
                    if ((sum + productAmount) > 5)
                    {
                        Console.WriteLine("You can not order more than 5 of one item.");
                        Console.WriteLine();
                    }
                    else
                    {
                        foreach (Product product in allStoreProducts)
                        {
                            if (product.ProductId == itemSelection)
                            {
                                if (product.ProductQuantiy == 0)
                                {
                                    Console.WriteLine("Out of stock.");
                                    Console.WriteLine();
                                    break;
                                }
                                while (product.ProductQuantiy < productAmount)
                                {
                                    Console.WriteLine("Not enough inventory to fulfill order.");
                                    Console.WriteLine("Please reduce order amount.");
                                    Console.WriteLine();
                                    isInt = int.TryParse(Console.ReadLine(), out productAmount);
                                    if (isInt == false || productAmount < 1 || productAmount > 5)
                                    {
                                        Console.WriteLine("Invalid input.");
                                        Console.WriteLine("Please enter a amount between 1 and 5.");
                                        Console.WriteLine();
                                        isInt = false;
                                    }
                                }
                                amountOfProductsOrdered.Add(new(itemSelection, productName, product.ProductDescription, product.ProductPrice, productAmount, product.StoreID));
                                product.ProductQuantiy -= productAmount;
                            }
                        }
                    }
                    if (amountOfProductsOrdered.Count > 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("You have selected to order:");
                        foreach (Product product in amountOfProductsOrdered)
                        {
                            Console.WriteLine(string.Format("{0,-2} | {1,-39} | {2,-30} | {3:C2} {4,8} {5,8}",
                                product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice, "|", product.ProductQuantiy));
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Would you like to select another item?");
                    Console.WriteLine("Yes(Y) or NO(N)");
                    string? yesOrNO = Console.ReadLine()?.ToLower();
                    if (yesOrNO == "n" || yesOrNO == "no")
                    {
                        isOrderingProducts = false;
                    }
                }
                List<Order> ordersMade = new List<Order>();
                foreach (Product product in amountOfProductsOrdered)
                {
                    ordersMade.Add(new(customerId, storeLocation, product.ProductId, product.ProductPrice, product.ProductName, product.ProductQuantiy));
                }
                Tuple<List<Order>, string> getOrders;
                try
                {
                    getOrders = await orderService.PostAllOrders(ordersMade);
                }
                catch (ServerException)
                {
                    Console.WriteLine("Unable to connect to server");
                    break;
                }
                Console.WriteLine("The order has been submitted.");
                Console.WriteLine("Order Summery:");
                Console.WriteLine();
                foreach (Order order in getOrders.Item1)
                {
                    Console.WriteLine(string.Format("{0,-39} | {1,-30} | {2:C2}", order.ProductName, order.ProductQuantiy, order.ProductPrice));
                }
                Console.WriteLine(getOrders.Item2);
                Console.WriteLine();
                Console.WriteLine("Thanks for placing an order.");
                Console.WriteLine();
                break;
            }
        }

        internal async static Task AddCustomer(Uri server)
        {
            ICustomerService plainOldStoreService = new CustomerService(server);
            bool isAdding = true;
            while (isAdding)
            {
                string firstName;
                string lastName;
                string email = "";
                Console.WriteLine("Please enter in the customer's name or email.");
                string? nameOrEmail = Console.ReadLine()?.Trim();
                Console.WriteLine();
                string? tryAgain;
                Tuple<string, string> nameOrEmailTuple = ValidateInput.ValidateNameOrEmail(nameOrEmail);
                bool isValidateingNameOrEmail = true;
                while (isValidateingNameOrEmail)
                {
                    if (nameOrEmailTuple.Item1 == "false")
                    {
                        Console.WriteLine("You did not enter in a full name or email.");
                        Console.WriteLine("Would you like to try again?");
                        Console.WriteLine("Yes(Y) or No(N)?");
                        tryAgain = Console.ReadLine()?.Trim().ToLower();
                        Console.WriteLine();
                        if (tryAgain == "yes" || tryAgain == "y")
                        {
                            Console.WriteLine("Please enter full name or email.");
                            nameOrEmail = Console.ReadLine()?.Trim();
                            Console.WriteLine();
                            nameOrEmailTuple = ValidateInput.ValidateNameOrEmail(nameOrEmail);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        isValidateingNameOrEmail = false;
                    }
                }
                CustomerHandler customerHandler = new CustomerHandler(plainOldStoreService);
                if (nameOrEmailTuple.Item1 == "email")
                {
                    bool foundEmail;
                        try
                        {
                            foundEmail = await customerHandler.IsEmailFound(nameOrEmailTuple.Item2);
                        }
                        catch (ServerException)
                        {
                            Console.WriteLine("Unable to connect to server.");
                            break;
                        }
                    if (foundEmail)
                    {
                        Console.WriteLine("The email you entered is already associated with an account.");
                        Console.WriteLine("Please use that account to order");
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        email = nameOrEmailTuple.Item2;
                        Console.WriteLine("Please enter first name and last name.");
                        string? name = Console.ReadLine();
                        Console.WriteLine();
                        Tuple<string, string> fullname = ValidateInput.VaildateName(name);
                        bool isValidateingName = true;
                        while (isValidateingName)
                        {
                            if (string.IsNullOrWhiteSpace(fullname.Item1) || string.IsNullOrWhiteSpace(fullname.Item2))
                            {
                                Console.WriteLine("You did not enter you fist and last name.");
                                Console.WriteLine("Would you like to try again?");
                                Console.WriteLine("Yes(Y) or No(N)?");
                                tryAgain = Console.ReadLine()?.Trim();
                                Console.WriteLine();
                                if (tryAgain == "yes" || tryAgain == "y")
                                {
                                    Console.WriteLine("Please enter your first name and last name.");
                                    name = Console.ReadLine()?.Trim();
                                    Console.WriteLine();
                                    fullname = ValidateInput.VaildateName(name);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                isValidateingName = false;
                            }
                        }
                        firstName = fullname.Item1;
                        lastName = fullname.Item2;
                    }
                }
                else
                {
                    List<Customer> foundCustomers;
                    try
                    {
                        foundCustomers = await plainOldStoreService.GetAllCustomersByFullName(nameOrEmailTuple.Item1, nameOrEmailTuple.Item2);
                    }
                    catch (ServerException)
                    {
                        Console.WriteLine("Unable to connect to server");
                        break;
                    }
                    string? emailLookUP;
                    Tuple<string, string> emailTuple;
                    if (foundCustomers.Count >= 1)
                    {
                        Console.WriteLine("The name you entered is already associated with an account.");
                        Console.WriteLine("Please enter email to see if account exists.");
                        emailLookUP = Console.ReadLine();
                        Console.WriteLine();
                        emailTuple = ValidateInput.ValidateEmail(emailLookUP);
                        bool isValidateingEmail = true;
                        while (isValidateingEmail)
                        {
                            if (emailTuple.Item1 == "false")
                            {
                                Console.WriteLine("Your did not enter in an email.");
                                Console.WriteLine("Would you like to try again?");
                                Console.WriteLine("Yes(Y) or No(N)?");
                                tryAgain = Console.ReadLine()?.Trim().ToLower();
                                Console.WriteLine();
                                if (tryAgain == "yes" || tryAgain == "y")
                                {
                                    Console.WriteLine("Please enter your email.");
                                    emailLookUP = Console.ReadLine()?.Trim();
                                    Console.WriteLine();
                                    emailTuple = ValidateInput.ValidateEmail(emailLookUP);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                isValidateingEmail = false;
                            }
                        }
                        foreach (Customer customer in foundCustomers)
                        {
                            if (customer.Email == emailTuple.Item2)
                            {
                                Console.WriteLine("Please use account associated with email to order.");
                                Console.WriteLine();
                                email = "anEmailWasFound";
                                break;
                            }
                        }
                        if (email == "anEmailWasFound")
                        {
                            break;
                        }
                        email = emailTuple.Item2;
                    }
                    firstName = nameOrEmailTuple.Item1;
                    lastName = nameOrEmailTuple.Item2;
                    if (email == "")
                    {
                        Console.WriteLine("Please enter an email.");
                        emailLookUP = Console.ReadLine();
                        Console.WriteLine();
                        emailTuple = ValidateInput.ValidateEmail(emailLookUP);
                        bool isValidateingEmail = true;
                        while (isValidateingEmail)
                        {
                            if (emailTuple.Item1 == "false")
                            {
                                Console.WriteLine("Your did not enter in an email.");
                                Console.WriteLine("Would you like to try again?");
                                Console.WriteLine("Yes(Y) or No(N)?");
                                tryAgain = Console.ReadLine()?.Trim().ToLower();
                                Console.WriteLine();
                                if (tryAgain == "yes" || tryAgain == "y")
                                {
                                    Console.WriteLine("Please enter your email.");
                                    emailLookUP = Console.ReadLine()?.Trim();
                                    Console.WriteLine();
                                    emailTuple = ValidateInput.ValidateEmail(emailLookUP);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                isValidateingEmail = false;
                            }
                        }
                        email = emailTuple.Item2;
                    }
                }
                Console.WriteLine("Please enter address 1.");
                string? address1 = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter city.");
                string? city = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter state.");
                string? state = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter zip code.");
                string? zip = Console.ReadLine();
                Console.WriteLine();

                string address = await ValidateInput.ValidateAddress(address1, city, state, zip);

                if (string.IsNullOrWhiteSpace(address)) { break; }

                address1 = address.Split("\n")[0];
                city = address.Split("\n")[1];
                state = address.Split("\n")[2];
                zip = address.Split("\n")[3] + "-" + address.Split("\n")[4];

                Console.WriteLine("Please verify that the name, email, and address were entered correctly.");
                Console.WriteLine();
                Console.WriteLine($"{firstName} {lastName}\n{address1}\n{city}\n{state}\n{zip}\n{email}");
                Console.WriteLine();
                Console.WriteLine("Correct Yes(Y) or No(N)");
                string? input = Console.ReadLine()?.ToLower().Trim();
                Console.WriteLine();
                if (input == "n" || input == "no")
                {
                    break;
                }
                Customer newCustomer = new Customer(
                        firstName,
                        lastName,
                        address1,
                        city,
                        state,
                        zip,
                        email);

                bool isAdded = await plainOldStoreService.PostNewCustomer(newCustomer);
                if (isAdded)
                {
                    Console.WriteLine("The customer was successfully registered.");
                    Console.WriteLine();
                }
                break;
        }
    }
        internal async static Task LookupOrder(Uri server)
        {
            IStoreService storeService = new StoreService(server);
            IOrderService orderService = new OrderService(server);
            Console.WriteLine("Would you like to lookup all orders of a customer or store?");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Store");
            string? selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.WriteLine("Please enter first name and last name.");
                    string? name = Console.ReadLine();
                    Console.WriteLine();
                    Tuple<string, string> fullname = ValidateInput.VaildateName(name);
                    string? tryAgain;
                    bool isValidateingName = true;
                    while (isValidateingName)
                    {
                        if (string.IsNullOrWhiteSpace(fullname.Item1) || string.IsNullOrWhiteSpace(fullname.Item2))
                        {
                            Console.WriteLine("You did not enter you fist and last name.");
                            Console.WriteLine("Would you like to try again?");
                            Console.WriteLine("Yes(Y) or No(N)?");
                            tryAgain = Console.ReadLine()?.Trim();
                            Console.WriteLine();
                            if (tryAgain == "yes" || tryAgain == "y")
                            {
                                Console.WriteLine("Please enter your first name and last name.");
                                name = Console.ReadLine()?.Trim();
                                Console.WriteLine();
                                fullname = ValidateInput.VaildateName(name);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            isValidateingName = false;
                        }
                    }
                    string firstName = fullname.Item1.ToUpper();
                    string lastName = fullname.Item2.ToUpper();

                    List<Order> orders = await orderService.GetAllOrdersByName(firstName, lastName);
                    if (orders.Count == 0)
                    {
                        Console.WriteLine("No orders have been made by this customer.");
                    }
                    else
                    {
                        foreach (Order order in orders)
                        {
                            Console.WriteLine(order.ProductName + " " + order.ProductPrice + " " + order.ProductQuantiy + " " + order.DateTime);
                        }
                    }
                    break;
                case "2":
                    Dictionary<int, string> stores = await storeService.GetStoreListAsync();
                    Console.WriteLine("Please choose a store location.");
                    foreach (var s in stores)
                    {
                        Console.WriteLine(s);
                    }
                    int numberOfStores = stores.Count;
                    int storeLocation = 0;
                    bool isInt = false;
                    while (!isInt)
                    {
                        isInt = int.TryParse(Console.ReadLine(), out storeLocation);
                        if (isInt == false || numberOfStores < 0 || storeLocation > numberOfStores)
                        {
                            Console.WriteLine("Invalid input.");
                            Console.WriteLine("Please choose a store location.");
                            Console.WriteLine();
                            isInt = false;
                        }
                    }
                    List<Order> ordersFromStore = await orderService.GetAllOrdersByStoreId(storeLocation);
                    if (ordersFromStore.Count == 0)
                    {
                        Console.WriteLine("No orders have been made at this store.");
                    }
                    else
                    {
                        foreach (Order order in ordersFromStore)
                        {
                            Console.WriteLine(order.ProductName + " " + order.ProductPrice + " " + order.StoreLocation + " " + order.DateTime);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("You did not make a valid selection");
                    break;
            }
        }
    }
}
