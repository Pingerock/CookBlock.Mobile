using System;
using System.Collections.Generic;
using System.Text;
using CookBlock.Views;
using CookBlock.Models;
using CookBlock.ViewModels;
using CookBlock.Services;
using System.Linq;
using Xamarin.Forms;

namespace CookBlock.Views.MainPage.MenuPages
{
    public class FeelingLucky
    {
        public User logInUser;
        public List<Recipe> recipes;

        public FullRecipeService fullRecipeService = new FullRecipeService();
        public Random random = new Random();

        public INavigation Navigation { get; set; }
        public FeelingLucky(User user)
        {
            logInUser = user;
            recipes = fullRecipeService.GetAllRecipes().Result.ToList();
            int index = random.Next(recipes.Count);
            Recipe recipe = recipes[index];
            FullRecipe fullRecipe = fullRecipeService.GetFullRecipe(recipe.Id).Result;
            Navigation.PushAsync(new SelectedRecipePage(logInUser, fullRecipe));
        }
    }
}
