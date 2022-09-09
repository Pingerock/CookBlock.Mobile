using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;
using CookBlock.ViewModels;
using CookBlock.Services;

namespace CookBlock.Views.MainPage.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        MenuViewModel viewModel;
        public User logInUser;

        public MainMenuPage(User user)
        {
            InitializeComponent();
            logInUser = user;
            viewModel = new MenuViewModel(user)
            {
                Navigation = this.Navigation,
            };
            BindingContext = viewModel;
        }

        public void FoodTypeFirstTapped(object sender, EventArgs args)
        {
            viewModel.FoodTypeFirstCommand.Execute(null);
        }

        public void FoodTypeSecondTapped(object sender, EventArgs args)
        {
            viewModel.FoodTypeSecondCommand.Execute(null);
        }

        public void FoodTypeThirdTapped(object sender, EventArgs args)
        {
            viewModel.FoodTypeThirdCommand.Execute(null);
        }

        public void FoodTypeFourthTapped(object sender, EventArgs args)
        {
            viewModel.FoodTypeFourthCommand.Execute(null);
        }

        public void FoodTypeFifthTapped(object sender, EventArgs args)
        {
            viewModel.FoodTypeFifthCommand.Execute(null);
        }

        public void FoodTypeSixthTapped(object sender, EventArgs args)
        {
            viewModel.FoodTypeSixthCommand.Execute(null);
        }

        public async void RandomRecipeTapped(object sender, EventArgs args)
        {
            FullRecipeService fullRecipeService = new FullRecipeService();
            Random random = new Random();
            List<Recipe> recipes = fullRecipeService.GetAllRecipes().Result.ToList();
            int index = random.Next(recipes.Count);
            Recipe recipe = recipes[index];
            FullRecipe fullRecipe = fullRecipeService.GetFullRecipe(recipe.Id).Result;
            viewModel.Navigation.PushAsync(new SelectedRecipePage(logInUser, fullRecipe));
        }
    }
}