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
    public partial class CategoryMenuPage : ContentPage
    {
        public User User { get; private set; }
        public MenuViewModel ViewModel { get; private set; }
        public CategoryMenuPage(User user, MenuViewModel viewModel)
        {
            InitializeComponent();
            User = user;
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RecipeList.SelectedItem = null;
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