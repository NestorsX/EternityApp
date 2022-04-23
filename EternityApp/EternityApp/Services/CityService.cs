using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class CityService
    {
        private const string _url = "http://eternity.somee.com/api/Cities/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public CityService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        //Получаем список городов
        public async Task<IEnumerable<City>> Get()
        {
            string result = await _client.GetStringAsync(_url);
            return JsonSerializer.Deserialize<IEnumerable<City>>(result, _options);
        }

        // Получаем город по id
        public async Task<City> Get(int id)
        {
            string result = await _client.GetStringAsync(_url + id);
            return JsonSerializer.Deserialize<City>(result, _options);
        }
    }
}
