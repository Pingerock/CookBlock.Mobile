using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Views;
using CookBlock.Models;
using CookBlock.Views.MainPage;

namespace CookBlock
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            User user = new User();
            MainPage = new NavigationPage(new StartPage(user));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
