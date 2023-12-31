﻿using LAB10.Data;
using LAB10.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LAB10
{
    internal class Program
    {
        static void Main(string[] args)
        {           
            while (true)
            {
                // Show user options and take input from user
                Console.WriteLine("Menu");
                Console.WriteLine("[1]: Show all customers");
                Console.WriteLine("[2]: Choose specific customer");
                Console.WriteLine("[3]: Add customer");
                Console.WriteLine("[4]: Exit program");
                string input = Console.ReadLine();
           
                // Call applicable method depending on users choice
                switch (input)
                {
                    case "1":
                        string sortingOrder = null;

                        // Re-promts user until sortingOrder is either ASC or DESC
                        while (sortingOrder != "ASC" && sortingOrder != "DESC")
                        {
                            Console.Clear();
                            Console.WriteLine("Acsending or descending order?");
                            Console.Write("[ASC/DESC]:");
                            sortingOrder = Console.ReadLine().ToUpper();
                        }
                        
                        ShowAllCustomers(sortingOrder);
                        break;
                    case "2":
                        ChooseCustomer();
                        break;
                    case "3":
                        AddCustomer();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        Thread.Sleep(500);
                        break;
                }
            }
        }

        // Prints out all customers name, country, region, number and amount of orders
        static void ShowAllCustomers(string sortingOrder)
        {
            using (NorthWindContext context = new NorthWindContext())
            {

                // Define initial query to get sorted later
                var query = context.Customers
                    .Select(c => new { ShippedDate = c.Orders.Select(o => o.ShippedDate), c.CompanyName, c.Country, c.Region, c.Phone, OrderCount = c.Orders.Count() });


                // Chooses how to order the list depending on users choice
                if (sortingOrder == "ASC")
                {
                    query = query.OrderBy(q => q.CompanyName);
                }
                else
                {
                    query = query.OrderByDescending(q => q.CompanyName);
                }

                // Runs query and saves it to customers
                var customers = query.ToList();
              
                // Iterates over each customer and prints out all their info along with how many orders they have shipped and not shipped
                foreach (var customer in customers)
                {
                    int ordersShipped = 0;
                    int ordersNotShipped = 0;

                    // All orders that are shipped have a date and those that don't aren't shipped
                    // So we simply check if date is null or not and and add 1 to respective variable
                    foreach (var shipDate in customer.ShippedDate)
                    {
                        if (shipDate == null)
                        {
                            ordersNotShipped++;
                        }
                        else
                        {
                            ordersShipped++;
                        }
                    }

                    Console.WriteLine($"Name: {customer.CompanyName} \nCountry: {customer.Country} \nRegion: {customer.Region} \nNumber: {customer.Phone} \nNumber of orders: {customer.OrderCount}");
                    Console.WriteLine($"Orders shipped: {ordersShipped}");
                    Console.WriteLine($"Orders not shipped: {ordersNotShipped}");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        // Prints out info on a customer of the users choice
        static void ChooseCustomer()
        {
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine();

            using (NorthWindContext context = new NorthWindContext())
            {
                try
                {
                    var customer = context.Customers
                        .Include(c => c.Orders)
                            .ThenInclude(o => o.OrderDetails)
                                .ThenInclude(od => od.Product)
                        .SingleOrDefault(c => c.CompanyName == name);

                    // If customer was found in the database, continue with printing out info
                    if (customer != null)
                    {
                        // Print out all information about the customer 
                        Console.WriteLine($"Company: {customer.CompanyName}");
                        Console.WriteLine($"Contact: {customer.ContactName}");
                        Console.WriteLine($"Contact title: {customer.ContactTitle}");
                        Console.WriteLine($"Address: {customer.Address}");
                        Console.WriteLine($"City: {customer.City}");
                        Console.WriteLine($"Region: {customer.Region}");
                        Console.WriteLine($"Postal code: {customer.PostalCode}");
                        Console.WriteLine($"Country: {customer.Country}");
                        Console.WriteLine($"Phone: {customer.Phone}");
                        Console.WriteLine($"Fax: {customer.Fax}");
                        Console.WriteLine();
                        Console.WriteLine("Orders:");

                        // Loop through and print out all the customers orders
                        foreach (var order in customer.Orders)
                        {
                            // Print out order id and date ordered
                            Console.WriteLine($" Order: {order.OrderId}");
                            Console.WriteLine($" Date ordered: {order.OrderDate}");

                            // Print out all the products and their price in said order
                            foreach (var orderDetail in order.OrderDetails)
                            {
                                Console.WriteLine($" Product: {orderDetail.Product.ProductName} | Unit price: {orderDetail.Product.UnitPrice}");
                            }
                            Console.WriteLine();
                        }
                    }
                    // If customer wasn't found in the database print out error message
                    else
                    {
                        Console.WriteLine("Could not find customer");
                    }
                }
                // If there was multiple customers found in the database print out error message
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // Add a new customer to database
        static void AddCustomer()
        {

            using (var context = new NorthWindContext())
            {
                // Print out all columns and take input from user on each
                Console.WriteLine("Enter the following (press enter to skip field)");
                Console.Write($"Company name: ");
                string companyName = Console.ReadLine();
                Console.Write($"Contact name: ");
                string contactName = Console.ReadLine();
                Console.Write($"Contact title: ");
                string contactTitle = Console.ReadLine();
                Console.Write($"Address: ");
                string address = Console.ReadLine();
                Console.Write($"City: ");
                string city = Console.ReadLine();
                Console.Write($"Region: ");
                string region = Console.ReadLine();
                Console.Write($"Postal code: ");
                string postalCode = Console.ReadLine();
                Console.Write($"Country: ");
                string country = Console.ReadLine();
                Console.Write($"Phone: ");
                string phone = Console.ReadLine();
                Console.Write($"Fax: ");
                string fax = Console.ReadLine();

                // Create new customer
                Customer customer = new Customer()
                {
                    // Checks if user entered values are null or not and saves
                    CompanyName = CheckIfNullAndSave(companyName),
                    ContactName = CheckIfNullAndSave(contactName),
                    ContactTitle = CheckIfNullAndSave(contactTitle),
                    Address = CheckIfNullAndSave(address),
                    City = CheckIfNullAndSave(city),
                    Region = CheckIfNullAndSave(region),
                    PostalCode = CheckIfNullAndSave(postalCode),
                    Country = CheckIfNullAndSave(country),
                    Phone = CheckIfNullAndSave(phone),
                    Fax = CheckIfNullAndSave(fax),

                    // Sets customer id to the first 5 letters of the company name to be similiar to existing customer id's
                    CustomerId = companyName.Substring(0, Math.Min(5, companyName.Length)).ToUpper()

                };
                
                // Save to database
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            // Checks if string is null or empty. And returns null if it is and the value if it isn't
            static string CheckIfNullAndSave(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }
    }
}



