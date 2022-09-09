using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.ViewModels;
using CookBlock.Models;
using Xamarin.Essentials;

namespace CookBlock.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        UserLoginViewModel viewModel;
        public StartPage(User user)
        {
            InitializeComponent();
            Label_AppName.FadeTo(0, 0, Easing.Linear);
            Label_AppName.FadeTo(1, 2000, Easing.Linear);
            //RotateImageContinously(image);
            viewModel = new UserLoginViewModel()
            {
                Navigation = this.Navigation
            };
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            //await viewModel.GetUsers();
            base.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Login_Entry.Text) && !String.IsNullOrWhiteSpace(Password_Entry.Text))
            {
                User user = new User();
                user.Name = Login_Entry.Text;
                user.Password = Password_Entry.Text;
                viewModel.LogInCommand.Execute(user);
            }
            else
            {
                viewModel.MakeAlert("Неверный ввод имени пользователя или пароля.");
            }
        }

        async Task RotateImageContinously(Image image)
        {
            while (true) // a CancellationToken in real life ;-)
            {
                for (int i = 1; i < 7; i++)
                {
                    if (image.Rotation >= 360f) image.Rotation = 0;
                    await image.RotateTo(i * (360 / 6), 1000, Easing.Linear);
                }
            }
        }
    }
}