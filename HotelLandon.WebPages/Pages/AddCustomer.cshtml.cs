using HotelLandon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotelLandon.WebPages.Pages
{
    public class AddCustomerModel : PageModel
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Customer Customer { get; set; }

        public AddCustomerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("ApiClient");
            Customer customer = new Customer()
            {
                FirstName = Customer.FirstName,
                LastName = Customer.LastName
            };
            string json = JsonConvert.SerializeObject(customer);
            await httpClient.PostAsync("Customers", new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}
