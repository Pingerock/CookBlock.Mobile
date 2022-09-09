using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;
using CookBlock.ViewModels;

namespace CookBlock.Views.MainPage.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRecipePage : ContentPage
    {
        public User User { get; private set; }
        public MenuViewModel ViewModel { get; private set; }
        public MyRecipePage(User user)
        {
            InitializeComponent();
            ViewModel = new MenuViewModel(user)
            {
                Navigation = this.Navigation,
            };
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            await ViewModel.GetMyRecipes();
            base.OnAppearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                RecipeList.ItemsSource = ViewModel.Recipes;
            }
            else
            {
                RecipeList.ItemsSource = ViewModel.Recipes.Where(r => r.Name.Contains(e.NewTextValue));
            }
        }
    }
}