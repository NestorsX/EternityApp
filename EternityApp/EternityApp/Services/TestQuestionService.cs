using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class TestQuestionService
    {
        private const string _url = AppSettings.Url + "api/TestQuestions/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public TestQuestionService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        //Получаем список городов
        public async Task<IEnumerable<TestQuestion>> Get()
        {
            string result = await _client.GetStringAsync(_url);
            return JsonSerializer.Deserialize<IEnumerable<TestQuestion>>(result, _options);
        }
    }
}
