using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EternityApp.Models;
using Xamarin.Essentials;

namespace EternityApp.Services
{
    public class ActionItemService
    {
        private const string _url = AppSettings.Url + "api/ActionItems/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        public ActionItemService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Получение списка закладок/закреплений/просмотров
        private async Task<IEnumerable<DataAction>> Get(DataActionDTO dto)
        {
            var response = await _client.PostAsync(_url + "all", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<IEnumerable<DataAction>>(await response.Content.ReadAsStringAsync(), _options);
        }

        // Добавление закладок/закреплений/просмотров
        private async Task Add(DataActionDTO dto)
        {
            await _client.PostAsync(_url + "add", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
        }

        // Удаление закладок/закреплений/просмотров
        private async Task Delete(DataActionDTO dto)
        {
            await _client.PostAsync(_url + "remove", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
        }

        public async Task<IEnumerable<DataAction>> GetAction(int categoryId, int actionId)
        {
            return await Get(new DataActionDTO
            {
                DataActionId = null,
                DataCategoryId = categoryId,
                ActionCategoryId = actionId,
                UserId = Convert.ToInt32(await SecureStorage.GetAsync("ID")),
                ItemId = null
            });
        }

        public async Task AddAction(int categoryId, int actionId, int itemId)
        {
            await Add(new DataActionDTO
            {
                DataActionId = null,
                DataCategoryId = categoryId,
                ActionCategoryId = actionId,
                UserId = Convert.ToInt32(await SecureStorage.GetAsync("ID")),
                ItemId = itemId
            });
        }

        public async Task DeleteAction(int categoryId, int actionId, int itemId)
        {
            await Delete(new DataActionDTO
            {
                DataActionId = null,
                DataCategoryId = categoryId,
                ActionCategoryId = actionId,
                UserId = Convert.ToInt32(await SecureStorage.GetAsync("ID")),
                ItemId = itemId
            });
        }
    }
}
