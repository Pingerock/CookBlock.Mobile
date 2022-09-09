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
    public partial class MyProfilePage : ContentPage
    {
        UserProfileViewModel viewModel;
        UserProfile UserProfile;

        public MyProfilePage(User currentUser)
        {
            InitializeComponent();
            viewModel = new UserProfileViewModel(currentUser)
            {
                Navigation = this.Navigation,
            };
            UserProfile = viewModel.userProfile;
            BindingContext = viewModel;
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool check1 = await DisplayAlert("Погоди!", "Ты уверен, что хочешь удалить свой профиль?", "Да", "Нет");
            if (check1)
            {
                bool check2 = await DisplayAlert("Точно?", "Ты все ещё уверен, что хочешь удалить свой профиль?", "Точно", "Нет");
                if (check2)
                {
                    await DisplayAlert("Хорошо.", "Но для удостоверения мне понадобится твой пароль.", "Ок");
                    string check3 = await DisplayPromptAsync("Подтверждение.", "Введи пароль от своего профиля, чтобы удалить данный аккаунт.");
                    if (check3 == UserProfile.user.Password)
                    {
                        await DisplayAlert("Удаление подтверждено.", "See You Space Cowboy...", "Ок");
                        viewModel.DeleteUserCommand.Execute(null);
                    }
                }
            }
        }
    }
}