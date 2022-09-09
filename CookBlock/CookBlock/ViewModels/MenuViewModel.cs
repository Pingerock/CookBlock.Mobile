using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using CookBlock.Services;
using CookBlock.Models;
using CookBlock.Views;
using CookBlock.Views.MainPage;
using System;
using System.Globalization;
using CookBlock.Views.MainPage.MenuPages;

namespace CookBlock.ViewModels
{
    public class MenuViewModel
    {
        // была ли начальная инициализация
        bool initialized = false;
        // идет ли загрузка с сервера
        private bool isBusy;

        public User logInUser;

        public FullRecipe fR;

        public string menuTitle;
        public string MenuTitle
        {
            get
            {
                return menuTitle;
            }
            set
            {
                menuTitle = value;
                OnPropertyChanged(nameof(MenuTitle));
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }

        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public bool isEmpty;
        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                isEmpty = value;
                OnPropertyChanged("IsEmpty");
            }
        }

        public bool NotEmpty
        {
            get
            {
                return !isEmpty;
            }
        }

        public ObservableCollection<Recipe_Ingredient> FullRecipe_Ingredients {get; set;}
        public ObservableCollection<Recipe_Instruction> FullRecipe_Instructions { get; set; }
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Recipe_Rating> Ratings { get; set; }
        public ObservableCollection<Recipe> Favourites { get; set; }

        public FullRecipeService recipeService = new FullRecipeService();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand FoodTypeFirstCommand { get; protected set; }
        public ICommand FoodTypeSecondCommand { get; protected set; }
        public ICommand FoodTypeThirdCommand { get; protected set; }
        public ICommand FoodTypeFourthCommand { get; protected set; }
        public ICommand FoodTypeFifthCommand { get; protected set; }
        public ICommand FoodTypeSixthCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        public Recipe selectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                if (selectedRecipe != value)
                {
                    fR = recipeService.GetFullRecipe(value.Id).Result;
                    selectedRecipe = null;
                    OnPropertyChanged("SelectedRecipe");
                    Navigation.PushAsync(new SelectedRecipePage(logInUser, fR));
                }
            }
        }

        public INavigation Navigation { get; set; }

        public MenuViewModel(User user)
        {
            logInUser = user;
            menuTitle = "Главное меню";
            Recipes = new ObservableCollection<Recipe>();
            Ratings = new ObservableCollection<Recipe_Rating>();
            Favourites = new ObservableCollection<Recipe>();
            FullRecipe_Ingredients = new ObservableCollection<Recipe_Ingredient>();
            FullRecipe_Instructions = new ObservableCollection<Recipe_Instruction>();
            FoodTypeFirstCommand = new Command(FoodTypeFirst);
            FoodTypeSecondCommand = new Command(FoodTypeSecond);
            FoodTypeThirdCommand = new Command(FoodTypeThird);
            FoodTypeFourthCommand = new Command(FoodTypeFourth);
            FoodTypeFifthCommand = new Command(FoodTypeFifth);
            FoodTypeSixthCommand = new Command(FoodTypeSixth);
            BackCommand = new Command(Back);
        }

        public async Task GetFavourites()
        {
            IsBusy = true;
            IEnumerable<Recipe> favourites = await recipeService.GetFavouriteRecipes(logInUser.Id);

            // очищаем список
            while (Favourites.Any())
                Favourites.RemoveAt(Favourites.Count - 1);

            // добавляем загруженные данные
            foreach (Recipe r in favourites)
                Favourites.Add(r);
            IsBusy = false;
        }

        public async Task GetRecipes(int foodTypeId)
        {
            IsBusy = true;
            IEnumerable<Recipe> recipes = await recipeService.GetRecipesByFoodType(foodTypeId);

            // очищаем список
            while (Recipes.Any())
                Recipes.RemoveAt(Recipes.Count - 1);

            // добавляем загруженные данные
            foreach (Recipe r in recipes)
                Recipes.Add(r);
            IsBusy = false;
        }

/*        public async Task GetSelectedRecipe(int recipeId)
        {
            IsBusy = true;
            FullRecipe fullRecipe = await recipeService.GetFullRecipe(recipeId).Result;
            IsBusy = false;
            return fullRecipe;
        }*/

        public async Task GetMyRecipes()
        {
            IsBusy = true;
            IEnumerable<Recipe> recipes = await recipeService.GetRecipesByUser(logInUser.Id);

            // очищаем список
            while (Recipes.Any())
                Recipes.RemoveAt(Recipes.Count - 1);

            // добавляем загруженные данные
            foreach (Recipe r in recipes)
                Recipes.Add(r);
            IsBusy = false;
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void FoodTypeFirst()
        {
            menuTitle = "Первые блюда";
            await GetRecipes(1);
            await Navigation.PushAsync(new CategoryMenuPage(logInUser, this));
        }

        private async void FoodTypeSecond()
        {
            menuTitle = "Вторые блюда";
            await GetRecipes(2);
            await Navigation.PushAsync(new CategoryMenuPage(logInUser, this));
        }

        private async void FoodTypeThird()
        {
            menuTitle = "Салаты";
            await GetRecipes(3);
            await Navigation.PushAsync(new CategoryMenuPage(logInUser, this));
        }

        private async void FoodTypeFourth()
        {
            menuTitle = "Закуски";
            await GetRecipes(4);
            await Navigation.PushAsync(new CategoryMenuPage(logInUser, this));
        }

        private async void FoodTypeFifth()
        {
            menuTitle = "Десерты";
            await GetRecipes(5);
            await Navigation.PushAsync(new CategoryMenuPage(logInUser, this));
        }

        private async void FoodTypeSixth()
        {
            menuTitle = "Напитки";
            await GetRecipes(6);
            await Navigation.PushAsync(new CategoryMenuPage(logInUser, this));
        }

        private async void MyFavourite()
        {
            await GetFavourites();
            await Navigation.PushAsync(new MyFavouritesPage(logInUser));
        }

        private void Back()
        {
            selectedRecipe = null;
            Navigation.PopAsync();
        }
    }
}
