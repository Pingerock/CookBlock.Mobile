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

namespace CookBlock.Views.MainPage.MenuPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddIngredientsPage : ContentPage
    {
        public RecipeViewModel ViewModel { get; set; }
        public ObservableCollection<Ingredient> IngredientList;

        public AddIngredientsPage(RecipeViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            IngredientList = new ObservableCollection<Ingredient>();
            ingredientList.ItemsSource = IngredientList;
            this.BindingContext = ViewModel;
        }

        private void addBtn_Click(object sender, EventArgs a)
        {
            if ((!String.IsNullOrWhiteSpace(nameEntry.Text) && !String.IsNullOrWhiteSpace(countEntry.Text)) || (Convert.ToInt16(countEntry.Text) != 0))
            {
                Ingredient ingredient = new Ingredient();
                ingredient.Name = nameEntry.Text;
                ingredient.Count = Convert.ToInt16(countEntry.Text);
                ingredient.Recipe_Id = ViewModel.addedRecipe.Id;
                IngredientList.Add(ingredient);
                ingredientList.ItemsSource = IngredientList;
                nameEntry.Text = "";
                countEntry.Text = "";
            }
            else 
            {
                ViewModel.MakeAlert("Неправильный ввод.");
            }
        }

        private void deleteBtn_Click(object sender, EventArgs args)
        {
            var item = (Xamarin.Forms.Button)sender;
            Recipe_Ingredient ingredient = ViewModel.Ingredients.FirstOrDefault(x => x.Name == item.CommandParameter.ToString());
            ViewModel.Ingredients.Remove(ingredient);
            ingredientList.ItemsSource = IngredientList;
        }

        private void OnLabelTapped_Delete(object sender, EventArgs args)
        {
            var label = (Label)sender;
            var item = (TapGestureRecognizer)label.GestureRecognizers[0];
            var name = item.CommandParameter.ToString();
            Ingredient ingredient = IngredientList.FirstOrDefault(x => x.Name == name);
            IngredientList.Remove(ingredient);
            ingredientList.ItemsSource = IngredientList;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            foreach(Ingredient ingr in IngredientList)
            {
                Recipe_Ingredient recipe_Ingredient = new Recipe_Ingredient();
                recipe_Ingredient.Name = ingr.Name;
                recipe_Ingredient.Count = ingr.Count;
                recipe_Ingredient.Recipe_Id = ingr.Id;
                ViewModel.Ingredients.Add(recipe_Ingredient);
            }
            ViewModel.AddInstructionsCommand.Execute(null);
        }
    }

    public class Ingredient
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}