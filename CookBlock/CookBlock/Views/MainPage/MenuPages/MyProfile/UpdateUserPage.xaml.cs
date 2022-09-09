using CookBlock.Models;
using CookBlock.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CookBlock.Views.MainPage.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateUserPage : ContentPage
    {
        public User User { get; private set; }
        public UserProfileViewModel ViewModel { get; private set; }
        public UpdateUserPage(User user, UserProfileViewModel viewModel)
        {
            InitializeComponent();
            User = user;
            ViewModel = viewModel;
            this.BindingContext = this;
        }

        public UpdateUserPage(User user)
        {
            InitializeComponent();
            User = user;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            bool check = InputCheck(UserName_Entry.Text, NewPassword_Entry.Text, NewPasswordConfirm_Entry.Text, OldPassword_Entry.Text);
            if (check)
            {
                ViewModel.SaveUserCommand.Execute(User);
            }
            else
            {
                ViewModel.MakeAlert("Некорректный ввод.");
            }
        }

        public bool InputCheck(string userNameEntry, string newPasswordEntry, string newPasswordCondirmEntry, string oldPasswordEntry)
        {
            //если поле ввода имени пользователя пустое
            if (!String.IsNullOrWhiteSpace(userNameEntry)) // 1
            {
                //если не введён нынешний пароль
                if (oldPasswordEntry == User.Password) // 2
                {
                    //если новый пароль совпадает с подтверждением пароля
                    if (newPasswordEntry == newPasswordCondirmEntry) // 3
                    {
                        //если новый пароль равен нынешнему
                        if (newPasswordEntry != User.Password) // 4
                        {
                            //если новый пароль совпадает с именем пользователя
                            if (newPasswordEntry != userNameEntry) // 5
                            {
                                // если поля ввода нового пароля и подтверждения пароля пустые
                                if (!String.IsNullOrWhiteSpace(newPasswordEntry) && !String.IsNullOrWhiteSpace(newPasswordCondirmEntry)) // 6
                                {
                                    User.Password = newPasswordEntry; // 7
                                }
                                User.Name = userNameEntry;
                                return true; // 8
                            }
                            else
                            {
                                //ViewModel.MakeAlert("Имя пользователя не должно совпадать с паролем.\nЭто не безопасно!");
                                return false; // 8
                            }
                        }
                        else
                        {
                            //ViewModel.MakeAlert("Новый пароль совпадает со старым паролем.");
                            return false; // 8
                        }
                    }
                    else
                    {
                        //ViewModel.MakeAlert("Новый пароль не совпадает с подтверждением пароля.");
                        return false; // 8
                    }
                }
                else
                {
                    //ViewModel.MakeAlert("Старый пароль введён неправильно.");
                    return false; // 8
                }
            }
            else
            {
                //ViewModel.MakeAlert("Пустое имя пользователя.");
                return false; // 8
            }
        }
    }
}