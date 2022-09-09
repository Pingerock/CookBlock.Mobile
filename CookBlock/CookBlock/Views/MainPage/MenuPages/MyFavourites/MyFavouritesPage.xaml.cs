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
    public partial class MyFavouritesPage : ContentPage
    {
        public MenuViewModel ViewModel;
        public User user;

        public MyFavouritesPage(User user)
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
            await ViewModel.GetFavourites();
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