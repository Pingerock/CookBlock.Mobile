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

namespace CookBlock.Services
{
    public class UserProfileService
    {
        const string Url = "http://192.168.31.211:5137/api/UserProfile/";

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<UserProfile> GetById(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "UserId?id=" + id).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(result);
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
