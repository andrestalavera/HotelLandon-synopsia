using HotelLandon.Models;
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
            string fileName = "Customers.csv";
            List<string> lines = new List<string>();

            string newLine = $"{customer.FirstName};{customer.LastName}";
            lines.Add(newLine);

            File.AppendAllLines(fileName, lines);
        }

        static List<Customer> ReadCustomers()
        {
            string fileName = "Customers.csv";
            List<Customer> customers = new List<Customer>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                string[] array = line.Split(';');
                string firstName = array[0];
                string lastName = array[1];
                Customer customer = new Customer(firstName, lastName);
                customers.Add(customer);
            }
            return customers;
        }
    }
}
