using CookBlock.Models;
using CookBlock.Views.MainPage;
using CookBlock.Views;
using System;
using System.Collections.Generic;
using System.Text;
using CookBlock.Services;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using CookBlock.Views.MainPage.MenuPages;

namespace CookBlock.ViewModels
{
    public class SelectedRecipeViewModel
    {
        public User logInUser;

        public FullRecipe fullRecipe;

        public int ratingsTotal;
        public int RatingsTotal
        {
            get { return ratingsTotal; }
            set 
            { 
                ratingsTotal = value; 
                OnPropertyChanged(nameof(RatingsTotal));
            }
        }

        public int commentsTotal;
        public int CommentsTotal
        {
            get { return commentsTotal; }
            set 
            { 
                commentsTotal = value; 
                OnPropertyChanged(nameof(CommentsTotal)); 
            }
        }

        public string authorName;
        public string AuthorName
        {
            get { return authorName; }
            set
            {
                authorName = value;
                OnPropertyChanged(nameof(AuthorName));
            }
        }

        public double averageRating;
        public double AverageRating
        {
            get { return averageRating; }
            set 
            {
                averageRating = value;
                OnPropertyChanged(nameof(AverageRating));
            }
        }

        public bool isMyRecipe;
        public bool IsMyRecipe
        {
            get { return isMyRecipe; }
            set 
            {
                isMyRecipe = value;
                OnPropertyChanged("IsMyRecipe");
                OnPropertyChanged("NotMyRecipe");
            }
        }

        public bool NotMyRecipe
        {
            get { return !isMyRecipe; }
        }

        public bool isLiked;
        public bool IsLiked
        {
            get { return isLiked; }
            set 
            {
                isLiked = value;
                OnPropertyChanged("IsLiked");
                OnPropertyChanged("NotLiked");
            }
        }

        public bool NotLiked
        {
            get { return !isLiked; }
        }

        public bool isImageExist;
        public bool IsImageExist
        {
            get { return isImageExist; }
            set 
            {
                isImageExist = value;
                OnPropertyChanged("IsImageExist");
            }
        }

        public bool isLoadedIngredients;
        public bool IsLoadedIngredients
        {
            get { return isLoadedIngredients; }
            set
            {
                isLoadedIngredients = value;
                OnPropertyChanged("IsLoadedIngredients");
                OnPropertyChanged("IsEmptyIngredients");
            }
        }

        public bool IsEmptyIngredients
        {
            get { return !isLoadedIngredients; }
        }

        public bool isLoadedInstructions;
        public bool IsLoadedInstructions
        {
            get { return isLoadedInstructions; }
            set 
            {
                isLoadedInstructions = value;
                OnPropertyChanged("IsLoadedInstructions");
                OnPropertyChanged("IsEmptyInstructions");
            }
        }

        public bool IsEmptyInstructions
        {
            get { return !isLoadedInstructions; }
        }

        public bool isLoadedComments;
        public bool IsLoadedComments
        {
            get { return isLoadedComments; }
            set
            {
                isLoadedComments = value;
                OnPropertyChanged("IsLoadedComments");
                OnPropertyChanged("IsEmptyComments");
            }
        }

        public bool IsEmptyComments
        {
            get { return !isLoadedComments; }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public ObservableCollection<Comment> Comments { get; set; }
        public ObservableCollection<Recipe_Ingredient> Ingredients { get; set; }
        public ObservableCollection<Instruction> Instructions { get; set; }

        public FullRecipeService recipeService = new FullRecipeService();
        public UserService userService = new UserService();
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand EditRecipeCommand { get; protected set; }
        public ICommand DeleteRecipeCommand { get; protected set; }
        public ICommand LikeRecipeCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }
        public ICommand AddCommentCommand { get; protected set; }
        public ICommand AddRatingCommand { get; protected set; }

        public INavigation Navigation { get; set; }

        public SelectedRecipeViewModel(User user, FullRecipe fullRecipe)
        {
            logInUser = user;
            this.fullRecipe = fullRecipe;
            Comments = new ObservableCollection<Comment>();
            Ingredients = new ObservableCollection<Recipe_Ingredient>();
            Instructions = new ObservableCollection<Instruction>();
            GetIngredients();
            GetInstructions();
            GetComments();
            IsItMyRecipe();
            IsRecipeLiked();
            IsMainImageExist();
            GetAuthorName();
            IsIngredientListExist();
            IsInstructionListExist();
            IsCommentListExist();
            GetRatingsTotal();
            GetAverageRating();
            EditRecipeCommand = new Command(EditRecipe);
            DeleteRecipeCommand = new Command(DeleteRecipe);
            LikeRecipeCommand = new Command(LikeRecipe);
            BackCommand = new Command(Back);
            AddCommentCommand = new Command(AddComment);
            AddRatingCommand = new Command(AddRating);
        }

        public async Task GetIngredients()
        {
            IsBusy = true;
            foreach (Recipe_Ingredient ingr in fullRecipe.ingredients)
            {
                Ingredients.Add(ingr);
            }
            IsBusy = false;
        }

        public async Task GetInstructions()
        {
            IsBusy = true;
            fullRecipe.instructions = fullRecipe.instructions.OrderBy(x => x.Position).ToList();
            foreach (Recipe_Instruction instr in fullRecipe.instructions)
            {
                Instructions.Add(new Instruction(instr));
            }
            IsBusy = false;
        }

        public async Task GetComments()
        {
            IsBusy = true;
            IEnumerable<Comment> comments = await recipeService.GetCommentsFromRecipe(fullRecipe.recipe.Id);
            comments.OrderByDescending(x => x.Date);
            foreach (Comment c in comments)
            {
                Comments.Add(c);
            }
            commentsTotal = fullRecipe.comments.Count();
            IsCommentListExist();
            IsBusy = false;
        }

        public void IsItMyRecipe()
        {
            IsBusy = true;
            if (logInUser.Id == fullRecipe.recipe.User_Id)
            {
                IsMyRecipe = true;
            }
            else
            {
                IsMyRecipe = false;
            }
            IsBusy = false;
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void IsRecipeLiked()
        {
            IsBusy = true;
            IsLiked = false;
            IEnumerable<Favourite> favourite = recipeService.GetFavouritesByUser(logInUser.Id).Result;
            foreach (Favourite fav in favourite)
            {
                if (fav.Recipe_Id == fullRecipe.recipe.Id)
                {
                    IsLiked = true;
                    break;
                }
            }
            IsBusy = false;
        }

        public void GetAverageRating()
        {
            IsBusy = true;
            List<Recipe_Rating> ratings = recipeService.GetRatingsFromRecipe(fullRecipe.recipe.Id).Result.ToList();
            double average = 0.0;
            foreach (Recipe_Rating rating in ratings)
            {
                average += rating.Rating_Score;
            }
            if (ratings.Count == 0)
            {
                average = 0.0;
            }
            else
            {
                average /= ratings.Count;
            }
            //double rating = recipeService.GetAverageRatingFromRecipe(fullRecipe.recipe.Id).Result;
            /*double average = 0.0;
            foreach (Recipe_Rating r in fullRecipe.ratings)
            {
                average += (double)r.Rating_Score;
            }
            averageRating = average / (double)fullRecipe.ratings.Count;*/
            averageRating = average;
            IsBusy = false;
        }

        public async void GetRatingsTotal()
        {
            IsBusy = true;
            List<Recipe_Rating> ratings = recipeService.GetRatingsFromRecipe(fullRecipe.recipe.Id).Result.ToList();
            ratingsTotal = ratings.Count;
            IsBusy = false;
        }

        public async void GetAuthorName()
        {
            IsBusy = true;
            User user = userService.GetById(fullRecipe.recipe.User_Id).Result;
            authorName = user.Name;
            IsBusy = false;
        }

        public async void IsIngredientListExist()
        {
            IsBusy = true;
            if (fullRecipe.ingredients.Count > 0)
            {
                IsLoadedIngredients = true;
            }
            else
            {
                IsLoadedIngredients = false;
            }
            IsBusy = false;
        }

        public async void IsInstructionListExist()
        {
            IsBusy = true;
            if (fullRecipe.instructions.Count > 0)
            {
                IsLoadedInstructions = true;
            }
            else
            {
                IsLoadedInstructions = false;
            }
            IsBusy = false;
        }

        public async void IsMainImageExist()
        {
            IsBusy = true;
            if (fullRecipe.recipe.Picture_base64 != null)
            {
                isImageExist = true;
            }
            else
            {
                isImageExist=false;
            }
            IsBusy = false;
        }

        public async void IsCommentListExist()
        {
            IsBusy = true;
            if (Comments.Count > 0)
            {
                isLoadedComments = true;
            }
            else
            {
                isLoadedComments = false;
            }
            IsBusy = false;
        }

        private async void EditRecipe()
        {
            
        }

        private async void DeleteRecipe()
        {
            await recipeService.DeleteRecipe(fullRecipe.recipe.Id);
            await Navigation.PushAsync(new MainPage(logInUser));
        }

        private async void LikeRecipe()
        {
            IsBusy = true;
            if (IsLiked == true)
            {
                IsLiked = false;
                Favourite deletedFavourite = await recipeService.DeleteFavourite(logInUser.Id, fullRecipe.recipe.Id);
            }
            else
            {
                Favourite favourite = new Favourite();
                favourite.Recipe_Id = fullRecipe.recipe.Id;
                favourite.User_Id = logInUser.Id;
                IsLiked = true;
                Favourite newFavourite = await recipeService.AddFavouriteRecipe(favourite);
            }
            IsBusy=false;
        }

        private async void AddRating(object ratingScore)
        {
            IsBusy = true;
            int score = (int)ratingScore;
            Recipe_Rating rating = new Recipe_Rating();
            rating.Rating_Score = score;
            rating.User_Id= logInUser.Id;
            rating.Recipe_Id= fullRecipe.recipe.Id;
            List<Recipe_Rating> ratings = recipeService.GetRatingsFromRecipe(fullRecipe.recipe.Id).Result.ToList();
            
            bool exist = ratings.Any(x => x.User_Id == rating.User_Id && x.Recipe_Id == rating.Recipe_Id);
            if (exist)
            {
                Recipe_Rating rating1 = ratings.FirstOrDefault(x => x.User_Id == rating.User_Id && x.Recipe_Id == rating.Recipe_Id);
                Recipe_Rating deletedRating = await recipeService.DeleteRating(rating.Recipe_Id, rating1.Id);
                Recipe_Rating newRating = await recipeService.AddRating(rating);
                //Recipe_Rating updatedRecipe = await recipeService.UpdateRating(rating1);
                await Application.Current.MainPage.DisplayAlert("Спасибо за ваш отзыв!", "Ваш старый отзыв успешно изменён.", "Ок");

            }
            else
            {
                Recipe_Rating newRating = await recipeService.AddRating(rating);
                await Application.Current.MainPage.DisplayAlert("Спасибо за ваш отзыв!", "Создан новый отзыв.", "Ок");
            }
            GetRatingsTotal();
            GetAverageRating();

            IsBusy = false;

        }

        private async void AddComment(object commentText)
        {
            IsBusy = true;
            string text = commentText.ToString();
            Comment comment = new Comment();
            comment.Text = text;
            comment.Date = DateTime.Now;
            comment.User_Id = logInUser.Id;
            comment.Recipe_Id = fullRecipe.recipe.Id;
            Comment newComment = await recipeService.AddComment(comment);
            GetComments();
            IsBusy=false;
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        public void MakeAlert(string message)
        {
            Application.Current.MainPage.DisplayAlert("Ошибка", message, "Ок");
        }
    }

    public class Instruction
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int Position { get; set; }
        public string Text { get; set; }
        public ImageSource ImageSource { get; set; }

        public Instruction(Recipe_Instruction instr)
        {
            this.Id = instr.Id;
            this.Recipe_Id = instr.Recipe_Id;
            this.Position = instr.Position;
            this.Text = instr.Text;
            ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(instr.Picture_base64)));
        }
    }
}
