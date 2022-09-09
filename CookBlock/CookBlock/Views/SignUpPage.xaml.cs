using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;
using CookBlock.ViewModels;

namespace CookBlock.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public User Model { get; private set; }
        public UserLoginViewModel ViewModel { get; private set; }
        public SignUpPage(User model, UserLoginViewModel viewModel)
        {
            InitializeComponent();
            Model = model;
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Login_Entry.Text) && !String.IsNullOrWhiteSpace(Password_Entry.Text) &&
                !String.IsNullOrWhiteSpace(PasswordConfirm_Entry.Text) && Password_Entry.Text == PasswordConfirm_Entry.Text)
            {
                Model.Name = Login_Entry.Text;
                Model.Password = Password_Entry.Text;
                Model.Date = DateTime.Now;
                ViewModel.SaveUserCommand.Execute(Model);
            }
            else 
            {
                ViewModel.MakeAlert("Неверный ввод имени пользователя или пароля.");
            }
        }
    }
}