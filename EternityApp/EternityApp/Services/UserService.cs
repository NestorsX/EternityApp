using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class UserService
    {
        private const string _url = AppSettings.Url + "api/Users/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public UserService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Получаем пользователя по id
        public async Task<User> Get(int id)
        {
            string result = await _client.GetStringAsync(_url + id);
            return JsonSerializer.Deserialize<User>(result, _options);
        }

        // Получаем пользователя по логину и паролю
        public async Task<User> Get(string login, string password)
        {
            string result = await _client.GetStringAsync(_url + $"auth/{login}/{password}");
            return JsonSerializer.Deserialize<User>(result, _options);
        }

        // Получаем пользователя по логину и паролю
        public async Task RestorePassword(string email)
        {
            await _client.GetStringAsync(_url + $"restorepassword/{email}");
        }

        // Добавление пользователя
        public async Task<User> Add(User user)
        {
            var response = await _client.PostAsync(_url, new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), _options);
        }

        // Обновление пользователя
        public async Task Update(User user)
        {
            await _client.PutAsync(_url, new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
        }
    }
}
