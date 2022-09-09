using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;
using CookBlock.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.IO;
using CookBlock.Tools;

namespace CookBlock.Views.MainPage.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddInstructionPage : ContentPage
    {
        public RecipeViewModel ViewModel;
        public int currentPosition;
        public string base64Image;

        public ImageToBase64Formatter imageToBase64Formatter = new ImageToBase64Formatter();
        public AddInstructionPage(RecipeViewModel viewModel)
        {
            InitializeComponent();
            currentPosition = 0;
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
        }

        async void GetPhotoAsync(object sender, EventArgs e)
        {
            try
            {
                base64Image = "";
                // выбираем фото
                var photo = await MediaPicker.PickPhotoAsync();
                // загружаем в ImageView
                img.Source = ImageSource.FromFile(photo.FullPath);
                byte[] bytes = imageToBase64Formatter.ByteArrayFromImage(photo.FullPath);
                base64Image = Convert.ToBase64String(bytes);
                //Recipe.Picture_base64 = base64Image;
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        async void TakePhotoAsync(object sender, EventArgs e)
        {
            //try
            //{
            base64Image = "";
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"{ViewModel.addedRecipe.Name}_{ViewModel.position}.png"
                });

                // для примера сохраняем файл в локальном хранилище
                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                // загружаем в ImageView
                img.Source = ImageSource.FromFile(photo.FullPath);
                byte[] b = imageToBase64Formatter.ByteArrayFromImage(photo.FullPath);
                base64Image = Convert.ToBase64String(b);
            //}
            /*catch (Exception ex)
            {
                //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }*/
        }

        private void addBtn_Click(object sender, EventArgs args)
        {
            Recipe_Instruction instruction = new Recipe_Instruction();
            instruction.Text = textEntry.Text;
            instruction.Picture_base64 = base64Image;
            instruction.Position = currentPosition + 1;
            bool exist = ViewModel.Instructions.Any(x => x.Position == currentPosition + 1);
            if (exist)
            {
                Recipe_Instruction oldInstruction = ViewModel.Instructions.FirstOrDefault(x => x.Position == currentPosition + 1);
                ViewModel.Instructions.Remove(oldInstruction);
            }
            ViewModel.Instructions.Add(instruction);
            currentPosition++;

            bool nextInstrExist = ViewModel.Instructions.Any(x => x.Position == currentPosition + 1);
            if (nextInstrExist)
            {
                Recipe_Instruction instr = ViewModel.Instructions.FirstOrDefault(x => x.Position == currentPosition + 1);
                textEntry.Text = instr.Text;
                try
                {
                    byte[] fromBase64String = Convert.FromBase64String(instr.Picture_base64);
                    img.Source = ImageSource.FromStream(() => new MemoryStream(fromBase64String));
                }
                catch
                {
                    img.Source = null;
                }
            }
            else
            {
                textEntry.Text = "";
                img.Source = null;
                base64Image = "";
            }
        }

        private void backBtn_Click(object sender, EventArgs args)
        {
            if (currentPosition == 0)
            {

            }
            else 
            {
                currentPosition--;
                Recipe_Instruction instr = ViewModel.Instructions.FirstOrDefault(x => x.Position == currentPosition + 1);
                textEntry.Text = instr.Text;
                base64Image = instr.Picture_base64;
                try
                {
                    byte[] fromBase64String = Convert.FromBase64String(instr.Picture_base64);
                    img.Source = ImageSource.FromStream(() => new MemoryStream(fromBase64String));
                }
                catch
                {
                    img.Source = null;
                }
            }
        }

        private void finishBtn_Click(object sender, EventArgs e)
        {
            ViewModel.CreateRecipeCommand.Execute(null);
        }
    }
}