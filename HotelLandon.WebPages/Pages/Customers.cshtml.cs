using HotelLandon.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelLandon.WebPages.Pages
{
    public class CustomersModel : PageModel
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        public IEnumerable<Customer> Customers { get; set; }

        public CustomersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("ApiClient");
            string json = await httpClient.GetStringAsync("Customers");
            Customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(json);
        }
    }
}
