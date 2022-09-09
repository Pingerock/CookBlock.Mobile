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
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        // идет ли загрузка с сервера
        private bool isBusy;

        public User logInUser;

        public UserProfile userProfile;
        public UserProfile UserProfile
        {
            get
            { 
                return userProfile; 
            }
            set
            { 
                userProfile = value;
                OnPropertyChanged(nameof(UserProfile));
            }
        }

        public string userDate;
        public string UserDate
        {
            get
            {
                return userDate;
            }
            set
            {
                userDate = value;
                OnPropertyChanged(nameof(UserDate));
            }
        }

        public int myRecipeCount;
        public int MyRecipeCount
        {
            get
            {
                return myRecipeCount;
            }
            set
            {
                myRecipeCount = value;
                OnPropertyChanged(nameof(MyRecipeCount));
            }
        }

        public int myRecipeRatingsCount;
        public int MyRecipeRatingsCount
        {
            get
            {
                return myRecipeRatingsCount;
            }
            set
            {
                myRecipeRatingsCount = value;
                OnPropertyChanged(nameof(MyRecipeRatingsCount));
            }
        }

        public int myRecipeCommentsCount;
        public int MyRecipeCommentsCount
        {
            get
            {
                return myRecipeCommentsCount;
            }
            set
            {
                myRecipeCommentsCount = value;
                OnPropertyChanged(nameof(MyRecipeCommentsCount));
            }
        }

        public int myRecipeFavouritesCount;
        public int MyRecipeFavouritesCount
        {
            get
            {
                return myRecipeFavouritesCount;
            }
            set
            {
                myRecipeFavouritesCount = value;
                OnPropertyChanged(nameof(MyRecipeFavouritesCount));
            }
        }

        public FullRecipe BestRecipe;

        public string bestRecipeName;
        public string BestRecipeName
        {
            get
            {
                return bestRecipeName;
            }
            set
            {
                bestRecipeName = value;
                OnPropertyChanged(nameof(BestRecipeName));
            }
        }

        public double bestRecipeAverageRatingCount;
        public double BestRecipeAverageRatingCount
        {
            get
            {
                return bestRecipeAverageRatingCount;
            }
            set
            {
                bestRecipeAverageRatingCount = value;
                OnPropertyChanged(nameof(BestRecipeAverageRatingCount));
            }
        }

        public int bestRecipeRatingsCount;
        public int BestRecipeRatingsCount
        {
            get
            {
                return bestRecipeRatingsCount;
            }
            set
            {
                bestRecipeRatingsCount = value;
                OnPropertyChanged(nameof(BestRecipeRatingsCount));
            }
        }

        public int bestRecipeCommentsCount;
        public int BestRecipeCommentsCount
        {
            get
            {
                return bestRecipeCommentsCount;
            }
            set
            {
                bestRecipeCommentsCount = value;
                OnPropertyChanged(nameof(BestRecipeCommentsCount));
            }
        }

        public bool bestRecipeExist;
        public bool BestRecipeExist
        {
            get
            {
                return bestRecipeExist;
            }
            set
            {
                bestRecipeExist = value;
                OnPropertyChanged(nameof(BestRecipeExist));
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

        UserProfileService userService = new UserProfileService();
        FullRecipeService recipeService = new FullRecipeService();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand UpdateUserCommand { get; protected set; }
        public ICommand DeleteUserCommand { get; protected set; }
        public ICommand SaveUserCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        public INavigation Navigation { get; set; }

        public UserProfileViewModel(User user)
        {
            logInUser = user;
            userProfile = GetUserProfile();
            userDate = SetUserDataStringFormat();
            myRecipeCount = GetRecipeCount();
            myRecipeRatingsCount = GetRecipeRatingsCount();
            myRecipeCommentsCount = GetRecipeCommentsCount();
            myRecipeFavouritesCount = GetRecipeFavouriteCount();
            GetBestRecipeStats();
            UpdateUserCommand = new Command(UpdateUser);
            DeleteUserCommand = new Command(DeleteUser);
            SaveUserCommand = new Command(SaveUser);
            BackCommand = new Command(Back);
            IsBusy = false;
        }

        //этот конструктор для тестов
        public UserProfileViewModel(User user, string test)
        {
            logInUser = user;
            UpdateUserCommand = new Command(UpdateUser);
            DeleteUserCommand = new Command(DeleteUser);
            SaveUserCommand = new Command(SaveUser);
            BackCommand = new Command(Back);
            IsBusy = false;
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void UpdateUser()
        {
            await Navigation.PushAsync(new UpdateUserPage(logInUser, this));
        }

        private async void DeleteUser()
        {
            await userService.Delete(logInUser.Id);
            await Navigation.PushAsync(new StartPage(logInUser));
        }

        private async void SaveUser(object userObject)
        {
            User user = userObject as User;
            if (user != null)
            {
                IsBusy = true;
                // редактирование
                if (user.Id > 0)
                {
                    User updatedUser = await userService.Update(user);
                    // заменяем объекты в представлении на обновленные
                    if (updatedUser != null)
                    {
                        //logInUser = updatedUser;
                        //userProfile = GetUserProfile();
                        await Navigation.PushAsync(new MyProfilePage(updatedUser));
                    }
                }
                else
                {
                    MakeAlert("Эмм...вас как бы не должно существовать.\nВыйдите из приложения по добру, по здорову.");
                    await Navigation.PushAsync(new StartPage(logInUser));
                }
                IsBusy = false;
            }
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        public UserProfile GetUserProfile()
        {
            UserProfile userProfile = userService.GetById(logInUser.Id).Result;
            return userProfile;
        }

        public string SetUserDataStringFormat()
        {
            string date = logInUser.Date.ToString("dd MMMM, yyyy", CultureInfo.CreateSpecificCulture("ru-RU")) + " г.";
            return date;
        }

        public int GetRecipeCount()
        {
            int recipeCount = recipeService.GetRecipesByUser(logInUser.Id).Result.Count();
            return recipeCount;
        }

        public int GetRecipeRatingsCount()
        {
            int recipeRatingsCount = recipeService.GetAllRatingsFromMyRecipes(logInUser.Id).Result.Count();
            return recipeRatingsCount;
        }

        public int GetRecipeCommentsCount()
        {
            int recipeCommentsCount = recipeService.GetAllCommentsFromMyRecipes(logInUser.Id).Result.Count();
            return recipeCommentsCount;
        }

        public int GetRecipeFavouriteCount()
        {
            int recipeFavouritesCount = recipeService.GetAllFavouritesFromMyRecipes(logInUser.Id).Result.Count();
            return recipeFavouritesCount;
        }

        public void GetBestRecipeStats()
        {
            BestRecipe = recipeService.GetBestFullRecipe(logInUser.Id).Result;
            if (BestRecipe.comments.Count == 0 && BestRecipe.ratings.Count == 0)
            {
                bestRecipeName = "Пусто. Похоже ещё не нашлось ценителей твоих стараний. :(";
                bestRecipeExist = false;
            }
            else
            {
                bestRecipeName = BestRecipe.recipe.Name;
                bestRecipeExist = true;
                bestRecipeAverageRatingCount = recipeService.GetAverageRatingFromRecipe(BestRecipe.recipe.Id).Result;
                bestRecipeRatingsCount = BestRecipe.ratings.Count;
                bestRecipeCommentsCount = BestRecipe.comments.Count;
            }
        }

        public void MakeAlert(string message)
        {
            Application.Current.MainPage.DisplayAlert("Ошибка", message, "Ок");
        }
    }
}
