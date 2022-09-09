using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CookBlock.Models;
using CookBlock.Services;
using CookBlock.Views;
using CookBlock.Views.MainPage.MenuPages;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using CookBlock.Views.MainPage;
using System.Globalization;

namespace CookBlock.ViewModels
{
    public class RecipeViewModel
    {
        // идет ли загрузка с сервера
        private bool isBusy;

        public User logInUser;

        public Recipe addedRecipe;

        public int position;

        public int recipeId;

        public FullRecipeService recipeService = new FullRecipeService();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand NextCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        public ICommand AddInstructionsCommand { get; protected set; }

        public ICommand CreateRecipeCommand { get; protected set; }
        public ICommand AddingredientCommand { get; protected set; }
        public ICommand AddInstructionCommand { get; protected set; }

        public ObservableCollection<Recipe_Ingredient> Ingredients;
        public ObservableCollection<Recipe_Instruction> Instructions;

        public INavigation Navigation { get; set; }

        public RecipeViewModel(User user)
        {
            logInUser = user;
            position = 0;
            Ingredients = new ObservableCollection<Recipe_Ingredient>();
            Instructions = new ObservableCollection<Recipe_Instruction>();
            NextCommand = new Command(Next);
            BackCommand = new Command(Back);
            CreateRecipeCommand = new Command(CreateRecipe);
            AddingredientCommand = new Command(Addingredient);
            AddInstructionCommand = new Command(AddInstruction);
            AddInstructionsCommand = new Command(AddInstructions);
            isBusy = false;
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private async void Next(object recipe)
        {
            addedRecipe = recipe as Recipe;
            //addedRecipe = await recipeService.AddRecipe(_recipe);
            await Navigation.PushAsync(new AddIngredientsPage(this));
        }

        private async void AddInstructions()
        {
            /*foreach (Recipe_Ingredient ingr in Ingredients)
            {
                Recipe_Ingredient addedIngredient = await recipeService.AddIngredient(ingr);
            }*/
            await Navigation.PushAsync(new AddInstructionPage(this));
        }

        private async void AddInstruction(object i)
        {
            Recipe_Instruction instruction = i as Recipe_Instruction;
            Recipe_Instruction newInstruction = await recipeService.AddInstruction(instruction);
        }


        private async void Addingredient(object ingr)
        {
            Recipe_Ingredient ingredient = ingr as Recipe_Ingredient;
            //ingredient.Recipe_Id = newRecipe.Id;
            Recipe_Ingredient newIngredient = await recipeService.AddIngredient(ingredient);
        }


        private async void CreateRecipe()
        {
            //FullRecipe NewRecipe = new FullRecipe(addedRecipe, Ingredients,Instructions);
            //FullRecipe addedRecipe = await recipeService.A
            Recipe newRecipe = await recipeService.AddRecipe(addedRecipe);
            foreach (Recipe_Ingredient ingr in Ingredients)
            {
                ingr.Recipe_Id = newRecipe.Id;
                Recipe_Ingredient addedIngredient = await recipeService.AddIngredient(ingr);
            }
            foreach (Recipe_Instruction instr in Instructions)
            {
                instr.Recipe_Id = newRecipe.Id;
                Recipe_Instruction addedInstruction = await recipeService.AddInstruction(instr);
            }
           await Navigation.PushAsync(new MainPage(logInUser));
        }

        public void MakeAlert(string message)
        {
            Application.Current.MainPage.DisplayAlert("Ошибка", message, "Ок");
        }


    }

}
