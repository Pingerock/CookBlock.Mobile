using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CookBlock.Models;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace CookBlock.Services
{
    public class FullRecipeService
    {
        const string Url = "http://192.168.31.40:5137/api/Recipes/";

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

        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe>>(result, options);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByUser(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "byUserId?userId=" + userId).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe>>(result, options);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByFoodType(int foodTypeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "byFoodType?foodTypeId=" + foodTypeId);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe>>(result, options);
        }


        public async Task<IEnumerable<Recipe>> GetFavouriteRecipes(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + userId + "/Favourites");
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe>>(result, options);
        }

        public async Task<IEnumerable<Favourite>> GetFavouritesByUser(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "Favourites/byUserId?userId=" + userId).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Favourite>>(result, options);
        }

        public async Task<IEnumerable<Favourite>> GetAllFavouritesFromMyRecipes(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "Favourites/all?userId=" + userId).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Favourite>>(result, options);
        }

        public async Task<IEnumerable<Comment>> GetCommentsFromRecipe(int recipeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + recipeId + "/Comments");
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Comment>>(result, options);
        }

        public async Task<IEnumerable<Recipe_Rating>> GetAllCommentsFromMyRecipes(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "Comments/all?userId=" + userId).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe_Rating>>(result, options);
        }

        public async Task<IEnumerable<Recipe_Rating>> GetRatingsFromRecipe(int recipeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + recipeId + "/Ratings").ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe_Rating>>(result, options);
        }

        public async Task<double> GetAverageRatingFromRecipe(int recipeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + recipeId + "/Ratings/average").ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<double>(result, options);
        }

        public async Task<IEnumerable<Recipe_Rating>> GetAllRatingsFromMyRecipes(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "Ratings/all?userId=" + userId).ConfigureAwait(false);
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe_Rating>>(result, options);
        }

        public async Task<IEnumerable<Recipe_Ingredient>> GetIngredientsFromRecipe(int recipeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + recipeId + "/Ingredients");
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe_Ingredient>>(result, options);
        }

        public async Task<IEnumerable<Recipe_Instruction>> GetInstructionsFromRecipe(int recipeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + recipeId + "/Instructions");
            return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Recipe_Instruction>>(result, options);
        }

        public async Task<FullRecipe> GetFullRecipe(int recipeId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + recipeId + "/Full").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<FullRecipe>(result);
        }

        public async Task<FullRecipe> GetBestFullRecipe(int userId)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + userId + "/Best").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<FullRecipe>(result);
        }

        public async Task<Favourite> AddFavouriteRecipe(Favourite favourite)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url + "Add/Favourite",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(favourite),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Favourite>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe_Rating> AddRating(Recipe_Rating rating)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url + "Add/Rating",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(rating),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Recipe_Rating>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url + "Add/Recipe/Main",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(recipe),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Recipe>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe_Ingredient> AddIngredient(Recipe_Ingredient ingredient)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url + "Add/Recipe/Ingredient",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(ingredient),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Recipe_Ingredient>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe_Instruction> AddInstruction(Recipe_Instruction instruction)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url + "Add/Recipe/Instruction",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(instruction),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Recipe_Instruction>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            HttpClient httpClient = GetClient();
            var response = await httpClient.PostAsync(Url + "Add/Comment",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(comment),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Comment>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<FullRecipe> UpdateRecipe(FullRecipe fullRecipe)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url,
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(fullRecipe),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<FullRecipe>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Comment> UpdateComment(Comment comment)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "Update/Comment",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(comment),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Comment>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe_Rating> UpdateRating(Recipe_Rating rating)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "Update/Rating",
                new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(rating),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<Recipe_Rating>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe> DeleteRecipe(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "id?id=" + id);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }


            return System.Text.Json.JsonSerializer.Deserialize<Recipe>(
               await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Comment> DeleteComment(int recipeId, int commentId)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + recipeId + "/Comments?commentId=" + commentId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }


            return System.Text.Json.JsonSerializer.Deserialize<Comment>(
               await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Recipe_Rating> DeleteRating(int recipeId, int ratingId)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + recipeId + "/Ratings?ratingId=" + ratingId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }


            return System.Text.Json.JsonSerializer.Deserialize<Recipe_Rating>(
               await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Favourite> DeleteFavourite(int userId, int recipeId)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + userId + "/Favourites/" + recipeId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }


            return System.Text.Json.JsonSerializer.Deserialize<Favourite>(
               await response.Content.ReadAsStringAsync(), options);
        }
    }
}
