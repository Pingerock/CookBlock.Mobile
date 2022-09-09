using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using CookBlock.Models;
using CookBlock.ViewModels;
using Xamarin.Forms.Xaml;
using System.Runtime.Serialization.Formatters.Binary;
using CookBlock.Tools;

using System.Threading.Tasks;


namespace CookBlock.Views.MainPage.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecipePage : ContentPage
    {
        public int foodType;
        public Recipe Recipe { get; private set; }
        public RecipeViewModel viewModel { get; private set; }

        public ImageToBase64Formatter imageToBase64Formatter = new ImageToBase64Formatter();
        public AddRecipePage(User user)
        {
            InitializeComponent();
            
            viewModel = new RecipeViewModel(user)
            {
                Navigation = this.Navigation,
            };
            Recipe = new Recipe();
            BindingContext = viewModel;
        }

        async void GetPhotoAsync(object sender, EventArgs e)
        {
            try
            {
                // выбираем фото
                var photo = await MediaPicker.PickPhotoAsync();
                /*var phototest = await CrossMedia.Current.PickPhotoAsync(
                    new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
                    });*/
                // загружаем в ImageView
                byte[] b = imageToBase64Formatter.ByteArrayFromImage(photo.FullPath);
                img.Source = ImageSource.FromFile(photo.FullPath);
                //byte[] bytes = imageToBase64Formatter.ObjectToByteArray(photo);
                string base64Image = Convert.ToBase64String(b);
                Recipe.Picture_base64 = base64Image;
                /*BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, photo);
                    byte[] bytes = ms.ToArray();
                    string base64Image = Convert.ToBase64String(ms.ToArray());
                    Recipe.Picture_base64 = base64Image;
                }*/
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        async void TakePhotoAsync(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(nameEntry.Text))
            {
                await DisplayAlert("Сообщение об ошибке", "Введите сначала название рецепта.", "OK");
            }
            else
            {
                try
                {
                    var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                    {
                        Title = $"{nameEntry.Text}_main.png"
                    });

                    /*var phototest = await CrossMedia.Current.TakePhotoAsync(
                        new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            SaveToAlbum = true,
                            Name = $"{nameEntry.Text}_main.png",
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
                        });*/

                    // для примера сохраняем файл в локальном хранилище
                    /*var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                    using (var stream = await photo.OpenReadAsync())
                    using (var newStream = File.OpenWrite(newFile))
                        await stream.CopyToAsync(newStream);*/

                    // загружаем в ImageView

                    byte[] b = imageToBase64Formatter.ByteArrayFromImage(photo.FullPath);

                    /*using (var image = await CrossImageEdit.Current.CreateImageAsync(b))
                    {
                        var croped = await Task.Run(() =>
                                image.Resize(500, 500)
                                     .ToPng()
                        );
                    }*/


                    string base64Image = Convert.ToBase64String(b);
                    img.Source = ImageSource.FromFile(photo.FullPath);
                    Recipe.Picture_base64 = base64Image;
                }
                catch (Exception ex)
                {
                    //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
                }
            }
        }

        void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            foodType = picker.SelectedIndex+1;
        }

        private void nextBtn_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(nameEntry.Text) && foodType > 0)
            {
                Recipe.Name = nameEntry.Text;
                Recipe.Food_Type_Id = foodType;
                Recipe.User_Id = viewModel.logInUser.Id;
                Recipe.Date = DateTime.Now;
                viewModel.NextCommand.Execute(Recipe);
            }
            else
            {
                viewModel.MakeAlert("Неправильный ввод данных.");
            }
        }

        private void cancelBtn_Clicked(object sender, EventArgs e)
        {
            viewModel.BackCommand.Execute(null);
        }
    }
}