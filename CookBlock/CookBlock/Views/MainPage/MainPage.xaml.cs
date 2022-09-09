using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;
using CookBlock.Views.MainPage.MenuPages;

namespace CookBlock.Views.MainPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public User currentUser;

        public MainPage(User logInUser)
        {
            InitializeComponent();
            currentUser = logInUser;
            Detail = new NavigationPage(new MainMenuPage(currentUser));
            FlyoutPage.ListView1.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageFlyoutMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType, currentUser);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            FlyoutPage.ListView1.SelectedItem = null;
        }
    }
}