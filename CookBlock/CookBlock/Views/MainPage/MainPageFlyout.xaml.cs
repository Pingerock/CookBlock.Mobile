using CookBlock.Views.MainPage.MenuPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;

namespace CookBlock.Views.MainPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageFlyout : ContentPage
    {
        public ListView ListView1;

        public MainPageFlyout()
        {
            InitializeComponent();

            BindingContext = new MainPageFlyoutViewModel();
            ListView1 = MenuItemsListView;
        }

        private class MainPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageFlyoutMenuItem> MenuItems { get; set; }

            public MainPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<MainPageFlyoutMenuItem>(new[]
                {
                    new MainPageFlyoutMenuItem { Id = 0, Title = "Мой профиль", IconSource="account.png", TargetType=typeof(MyProfilePage)},
                    new MainPageFlyoutMenuItem { Id = 1, Title = "Меню", IconSource="silverware.png", TargetType=typeof(MainMenuPage)},
                    //new MainPageFlyoutMenuItem { Id = 2, Title = "\"Мне повезёт\"", IconSource="dice.png", TargetType=typeof(SelectedRecipePage)},
                    new MainPageFlyoutMenuItem { Id = 2, Title = "Мои рецепты", IconSource="list.png", TargetType=typeof(MyRecipePage)},
                    new MainPageFlyoutMenuItem { Id = 3, Title = "Избранные", IconSource="heart_black.png", TargetType=typeof(MyFavouritesPage)},
                    new MainPageFlyoutMenuItem { Id = 4, Title = "Создать рецепт", IconSource="plus.png", TargetType=typeof(AddRecipePage)},
                    new MainPageFlyoutMenuItem { Id = 5, Title = "О приложении", IconSource = "info.png", TargetType=typeof(AboutPage)},
                    new MainPageFlyoutMenuItem { Id = 6, Title = "Выйти", IconSource = "exit.png", TargetType=typeof(StartPage)}
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}