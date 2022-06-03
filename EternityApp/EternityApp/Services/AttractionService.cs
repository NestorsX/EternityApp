using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class AttractionService
    {
        private const string _url = AppSettings.Url + "api/Attractions/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public AttractionService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        //Получаем список достопримечательностей
        public async Task<IEnumerable<Attraction>> Get()
        {
            string result = await _client.GetStringAsync(_url);
            return JsonSerializer.Deserialize<IEnumerable<Attraction>>(result, _options);
        }

        // Получаем достопримечательность по id
        public async Task<Attraction> Get(int id)
        {
            string result = await _client.GetStringAsync(_url + id);
            return JsonSerializer.Deserialize<Attraction>(result, _options);
        }
    }
}
