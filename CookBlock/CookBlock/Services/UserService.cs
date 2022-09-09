using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CookBlock.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CookBlock.Services
{
    public class UserService
    {
        const string Url = "http://192.168.31.211:5137/api/Users/";

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromMinutes(2); 
            return client;
        }

        public async Task<IEnumerable<User>> Get()
        {
            HttpClient client = GetClient();
            string result = "";
            try
            {
                result = await client.GetStringAsync(Url);
            }
            catch 
            {
                string message = "Отсутствует подключение к серверу. Пожалуйста, проверьте соединение с Интернетом.";
                await Application.Current.MainPage.DisplayAlert("Ошибка", message, "Ок");
                System.Environment.Exit(0);
            }
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<User>>(result, options);
        }

        public void CheckConnection()
        {
            
        }

        public async Task<User> GetById(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "byId?id=" + id).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<User>(result);
        }

        public async Task<User> Add(User user)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url,
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<User>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<User> Update(User user)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url,
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<User>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<User> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "delete?id=" + id);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            
            return System.Text.Json.JsonSerializer.Deserialize<User>(
               await response.Content.ReadAsStringAsync(), options);
        }
    }
}
