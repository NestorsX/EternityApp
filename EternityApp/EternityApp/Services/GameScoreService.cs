using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class GameScoreService
    {
        private const string _url = "http://eternity.somee.com/api/GameScores/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public GameScoreService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Получаем результат игры по id игры
        public async Task<GameScore> Get(int id)
        {
            string result = await _client.GetStringAsync(_url + id);
            return JsonSerializer.Deserialize<GameScore>(result, _options);
        }

        // Добавление результата игры
        public async Task Add(AddGameScore gameScore)
        {
            await _client.PostAsync(_url, new StringContent(JsonSerializer.Serialize(gameScore), Encoding.UTF8, "application/json"));
        }
    }
}
