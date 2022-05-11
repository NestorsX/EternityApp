using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;

namespace EternityApp.Services
{
    public class CityBookmarkService
    {
        private const string _url = "http://eternity.somee.com/api/bookmarks/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public CityBookmarkService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        //Получаем список закладок городов
        public async Task<IEnumerable<CityBookmark>> GetCityBookmarkList(int userId)
        {
            string result = await _client.GetStringAsync(_url + "citybookmark/" + userId);
            return JsonSerializer.Deserialize<IEnumerable<CityBookmark>>(result, _options);
        }

        // Добавить закладку города
        public async Task AddCityBookmark(CityBookmark bookmark)
        {
            await _client.PostAsync(_url + "citybookmark", new StringContent(JsonSerializer.Serialize(bookmark), Encoding.UTF8, "application/json"));
        }

        // Удаление закладки города
        public async Task DeleteCityBookmark(int id)
        {
            await _client.DeleteAsync(_url + "citybookmark/" + id);
        }
    }
}
