using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBlock;
using CookBlock.Models;
using Xunit;
using Moq;
using CookBlock.Views.MainPage.MenuPages;
using CookBlock.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using FakeItEasy;

namespace CookBlock.Tests
{
    public class UserTests
    {
        public UserTests()
        {
            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;
        }

        public bool BaseTestMethod(User currentUser, string userTextBox, string newPassword, string newPasswordConfirm, string oldPassword)
        {
            var mockViewModel = new Mock<UserProfileViewModel>(currentUser, "Тест");
            var mockVM = mockViewModel.Object;
            var mockView = new Mock<UpdateUserPage>(currentUser, mockVM);
            var mock = mockView.Object;
            bool check = mock.InputCheck(userTextBox, newPassword, newPasswordConfirm, oldPassword);
            return check;
        }

        [Fact]
        public void UserEditTest1()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "";
            string newPassword = "";
            string newPasswordConfirm = ""; 
            string oldPassword = "";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.False(result);
        }

        [Fact]
        public void UserEditTest2()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "TestName";
            string newPassword = "";
            string newPasswordConfirm = "";
            string oldPassword = "";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.False(result);
        }

        [Fact]
        public void UserEditTest3()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "TestName";
            string newPassword = "NewTestPassword";
            string newPasswordConfirm = "NewTestPasswordWrong";
            string oldPassword = "TestPassword";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.False(result);
        }

        [Fact]
        public void UserEditTest4()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "TestName";
            string newPassword = "TestPassword";
            string newPasswordConfirm = "";
            string oldPassword = "TestPassword";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.False(result);
        }

        [Fact]
        public void UserEditTest5()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "NewTestPassword";
            string newPassword = "NewTestPassword";
            string newPasswordConfirm = "NewTestPassword";
            string oldPassword = "TestPassword";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.False(result);
        }

        [Fact]
        public void UserEditTest6()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "NewTestName";
            string newPassword = "";
            string newPasswordConfirm = "";
            string oldPassword = "TestPassword";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.True(result);
        }

        [Fact]
        public void UserEditTest7()
        {
            User user = new User();
            user.Name = "TestName";
            user.Password = "TestPassword";
            string userTextBox = "NewTestName";
            string newPassword = "NewTestPassword";
            string newPasswordConfirm = "NewTestPassword";
            string oldPassword = "TestPassword";


            bool result = BaseTestMethod(user, userTextBox, newPassword, newPasswordConfirm, oldPassword);
            Assert.True(result);
        }
    }
}
