using HotelLandon.Data;
using HotelLandon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelLandon.Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly HotelLandonContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(HotelLandonContext context,
            ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Customer> GetAll(string search)
        {
            IEnumerable<Customer> query = _context.Customers; // SELECT * FROM Customers
            if(!string.IsNullOrWhiteSpace(search))
            {
                // WHERE FirstName LIKE '%(search)%' 
                // OR LastName LIKE '%(search)%'
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }
            return query.ToList();
        }

        [HttpPost]
        public Customer Post(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        [HttpPut]
        public Customer Put(int id, Customer customer)
        {
            if(id != customer.Id)
            {
                throw new Exception("V�rifiez l'ID");
            }
            _context.Customers.Update(customer);
            _context.SaveChanges();

            return customer;
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            Customer customer = _context.Customers.Find(id);
            _context.Remove(customer);
            return _context.SaveChanges() == 1;
        }

        [HttpGet("Toto")]
        public IEnumerable<Customer> Get2(){
        IEnumerable<Customer> query = _context.Customers.ToList();

        // filtrer les résultats déjà obtenus
        IEnumerable<Customer> query2 = query.Where(c => c.FirstName.Contains("A"));

        // renvoyer
        return query2.ToList();
        }
    }
}
