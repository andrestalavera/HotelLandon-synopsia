using HotelLandon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace HotelLandon.Demo_Csv
{
    class Program 
    {
        static void Main(string[] args)
        {
            WriteLine("Type your first name:");
            string firstName = ReadLine();
            
            WriteLine("Type your last name:");
            string lastName = ReadLine();

            Customer customer = new Customer(firstName, lastName);

            Save(customer);
        }

        static void Save(Customer customer)
        {
            string fileName = "Customers.json";
            List<Customer> customers = ReadCustomers();
            if (customers == null)
            {
                WriteLine("La liste des clients est vide");
                return;
            }
            customers.Add(customer);
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        static List<Customer> ReadCustomers()
        {
            string fileName = "Customers.json";
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            string json = File.ReadAllText(fileName);
            if (string.IsNullOrWhiteSpace(json))
            {
                json = "[]";
            }
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            return customers;
        }
    }
}
