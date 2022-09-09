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

namespace CookBlock.ViewModels
{
    public class UserLoginViewModel : INotifyPropertyChanged
    {
        // была ли начальная инициализация
        bool initialized = false;
        // выбранный пользователь
        User selectedUser;
        // идет ли загрузка с сервера
        private bool isBusy;

        public ObservableCollection<User> Users { get; set; }
        UserService userService = new UserService();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateUserCommand { get; protected set; }
        public ICommand DeleteUserCommand { get; protected set; }
        public ICommand SaveUserCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        public ICommand LogInCommand { get; protected set; }

        public INavigation Navigation { get; set; }

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

        public UserLoginViewModel()
        {
            Users = new ObservableCollection<User>();
            IsBusy = false;
            CreateUserCommand = new Command(CreateUser);
            DeleteUserCommand = new Command(DeleteUser);
            SaveUserCommand = new Command(SaveUser);
            LogInCommand = new Command(LogIn);
            BackCommand = new Command(Back);
        }

        // Сейчас это бесполезно. Пригодится, когда сделаю "режим админа"
        /*public User SelectedUser
        {
            get { return selectedUser; }
            set 
            {
                if (selectedUser != value)
                {
                    User tempUser = new User()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Password = value.Password
                    };
                    selectedUser = null;
                    OnPropertyChanged("SelectedUser");
                    //Navigation.PushAsync(new UserPage(tempUser, this));
                }
            }
        }*/

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void CreateUser()
        {
            await Navigation.PushAsync(new SignUpPage(new User(), this));
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        /*public async Task GetUsers()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<User> users = await userService.Get();

            // очищаем список
            while (Users.Any())
                Users.RemoveAt(Users.Count - 1);

            // добавляем загруженные данные
            foreach (User u in users)
                Users.Add(u);
            IsBusy = false;
            initialized = true;
        }*/

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
                    // заменяем объект в списке на новый
                    if (updatedUser != null)
                    {
                        int pos = Users.IndexOf(updatedUser);
                        Users.RemoveAt(pos);
                        Users.Insert(pos, updatedUser);
                    }
                }
                // добавление
                else
                {
                    IEnumerable<User> users = await userService.Get();
                    bool exist = users.Any(x => x.Name == user.Name);
                    if (exist)
                    {
                        MakeAlert("Данный пользователь уже существует.");
                    }
                    else
                    {
                        User addedUser = await userService.Add(user);
                        if (addedUser != null)
                        {
                            Users.Add(addedUser);
                        }      
                    }
                }
                IsBusy = false;
            }
            Back();
        }

        private async void LogIn(object userObject)
        {
            User user = userObject as User;
            if (user != null)
            {
                IsBusy = true;
                IEnumerable<User> users = await userService.Get();
                bool exist = users.Any(x => x.Name == user.Name && x.Password == user.Password);
                if (exist)
                {
                    User logInUser = users.FirstOrDefault(x => x.Name == user.Name && x.Password == user.Password);
                    await Navigation.PushAsync(new MainPage(logInUser));
                }
                else 
                {
                    MakeAlert("Неверный ввод имени пользователя или пароля.");
                }
            }
            IsBusy = false;
        }

        private async void DeleteUser(object userObject)
        {
            User user = userObject as User;
            if (user != null)
            {
                IsBusy = true;
                User deletedFriend = await userService.Delete(user.Id);
                if (deletedFriend != null)
                {
                    Users.Remove(deletedFriend);
                }
                IsBusy = false;
            }
            Back();
        }

        public void MakeAlert(string message)
        {
            Application.Current.MainPage.DisplayAlert("Ошибка", message, "Ок");
        }
    }
}
