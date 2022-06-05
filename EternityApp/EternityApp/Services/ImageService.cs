using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class ImageService
    {
        private const string _url = AppSettings.Url + "api/images";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public ImageService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        //Получаем список картинок городов
        public async Task<IEnumerable<Image>> Get(string category, int id)
        {
            string result = await _client.GetStringAsync($"{_url}/{category}/{id}");
            IEnumerable<Image> images = JsonSerializer.Deserialize<IEnumerable<Image>>(result, _options);
            foreach (Image image in images)
            {
                image.Path = $"http://eternity.somee.com/images/{category}/{id}/{image.Path}";
            }

            return images;
        }

        // Получаем главную картинку
        public async Task<string> GetTitleImage(string category, int id)
        {
            string result = await _client.GetStringAsync($"{_url}/{category}/{id}");
            return JsonSerializer.Deserialize<IEnumerable<Image>>(result, _options).First().Path;
        }

        // Отправляем картинку пользователя на сервер
        public async Task PostUserImage(int userId, MultipartFormDataContent content)
        {
            await _client.PostAsync($"{_url}/{userId}", content);
        }
    }
}
