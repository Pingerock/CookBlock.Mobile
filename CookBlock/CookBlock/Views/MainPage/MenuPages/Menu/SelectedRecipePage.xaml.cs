using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CookBlock.Models;
using CookBlock.ViewModels;
using System.IO;
using System.Collections.ObjectModel;
using CookBlock.Services;
using System.Globalization;
using CookBlock.Tools;

namespace CookBlock.Views.MainPage.MenuPages
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedRecipePage : ContentPage
    {
        public SelectedRecipeViewModel ViewModel { get; set; }
        public User logInUser { get; set; }
        public FullRecipe FullRecipe { get; set; }
        public Recipe Recipe { get; set; }

        public ObservableCollection<CommentView> Comments;
        public ObservableCollection<Recipe_Rating> Ratings;
        public FullRecipeService fullRecipeService = new FullRecipeService();
        public Random random = new Random();

        public SelectedRecipePage(User user)
        {
            logInUser = user;
            List<Recipe> recipes = fullRecipeService.GetAllRecipes().Result.ToList();
            int index = random.Next(recipes.Count);
            Recipe recipe = recipes[index];
            FullRecipe fullRecipe = fullRecipeService.GetFullRecipe(recipe.Id).Result;
            Recipe = fullRecipe.recipe;
            Ratings = new ObservableCollection<Recipe_Rating>();
            Comments = new ObservableCollection<CommentView>();
            foreach (Recipe_Rating rating in FullRecipe.ratings)
            {
                Ratings.Add(rating);
            }
            foreach (Comment comment in FullRecipe.comments)
            {
                Comments.Add(new CommentView(comment));
            }
            Comments.OrderBy(x => x.Date);

            ViewModel = new SelectedRecipeViewModel(user, fullRecipe)
            {
                Navigation = this.Navigation,
            };
            this.BindingContext = this;
            InitializeComponent();

            if (ViewModel.IsLiked == true)
            {
                likeImage.Source = "heart_white.png";
            }
            else
            {
                likeImage.Source = "heart_outline_white.png";
            }

            if (ViewModel.IsImageExist == true)
            {
                mainImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(FullRecipe.recipe.Picture_base64)));
            }
            else
            {
                mainImage.Source = "no_image.jpg";
                mainImage.IsVisible = true;
            }
            commentCount.Text = Comments.Count.ToString();
            GetAverage();
            rTotal.Text = Ratings.Count.ToString();
            if (Comments.Count > 0)
            {
                commentList.IsVisible = true;
                commentsEmpty.IsVisible = false;
            }
            commentList.ItemsSource = Comments;
            creationDate.Text = FullRecipe.recipe.Date.ToString("dd MMMM, yyyy", CultureInfo.CreateSpecificCulture("ru-RU")) + " г.";
        }

        public SelectedRecipePage(User user, FullRecipe fR)
        {
            
            logInUser = user;
            FullRecipe = fR;
            Recipe = fR.recipe;
            Ratings = new ObservableCollection<Recipe_Rating>();
            Comments = new ObservableCollection<CommentView>();
            foreach (Recipe_Rating rating in FullRecipe.ratings)
            {
                Ratings.Add(rating);
            }
            foreach (Comment comment in FullRecipe.comments)
            {
                Comments.Add(new CommentView(comment));
            }
            Comments.OrderBy(x => x.Date);

            ViewModel = new SelectedRecipeViewModel(user, fR)
            {
                Navigation = this.Navigation,
            };
            this.BindingContext = this;
            InitializeComponent();

            if (ViewModel.IsLiked == true)
            {
                likeImage.Source = "heart_white.png";
            }
            else
            {
                likeImage.Source = "heart_outline_white.png";
            }

            if (ViewModel.IsImageExist == true)
            {
                mainImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(FullRecipe.recipe.Picture_base64)));
            }
            else
            {
                mainImage.Source = "no_image.jpg";
                mainImage.IsVisible = true;
            }
            commentCount.Text = Comments.Count.ToString();
            GetAverage();
            rTotal.Text = Ratings.Count.ToString();
            if (Comments.Count > 0)
            {
                commentList.IsVisible = true;
                commentsEmpty.IsVisible = false;
            }
            commentList.ItemsSource = Comments;
            creationDate.Text = FullRecipe.recipe.Date.ToString("dd MMMM, yyyy", CultureInfo.CreateSpecificCulture("ru-RU")) + " г.";
        }

        public void EditRecipeTapped(object sender, EventArgs args)
        {

        }

        public async void DeleteRecipeTapped(object sender, EventArgs args)
        {
            bool check = await DisplayAlert("Погоди!", "Ты уверен, что хочешь удалить свой рецепт?", "Да", "Нет");
            if (check)
            {
                ViewModel.DeleteRecipeCommand.Execute(null);
            }
        }

        public async void ReportRecipeTapped(object sender, EventArgs args)
        {
            string reportText = "";
            string reason = await DisplayActionSheet("Выберите причину для отправки жалобы", "Отмена", null, 
                "Ложная информация", "Опасные действия", "Оскорбления", "Непристойное имя автора", "Непристойный комментарий");
            if (reason != "Отмена")
            {
                string description = await DisplayPromptAsync("Опрос", "Пожалуйста, введите подробную причну жалобы.");
                if (String.IsNullOrEmpty(description))
                {
                    await DisplayAlert("Ошибка", "Пожалуйста, введите причину жалобы. Это поможет нам быстрее анализировать ситуацию.", "ОК");
                    return;
                }
                else
                {
                    reportText =
                        "Причина жалобы: " + reason + "\n" +
                        "Описание: " + description;
                    MailSender reporter = new MailSender();
                    reporter.sendReport(logInUser, Recipe, reportText);
                    await DisplayAlert("Готово", "Жалоба успешно отправлена!", "ОК");
                }
            }    
        }

        public void LikeRecipeTapped(object sender, EventArgs args)
        {
            ViewModel.LikeRecipeCommand.Execute(null);
            if (ViewModel.IsLiked == true)
            {
                likeImage.Source = "heart_white.png";
            }
            else
            {
                likeImage.Source = "heart_outline_white.png";
            }
        }

        public void SendCommentTapped(object sender, EventArgs args)
        {
            if (!String.IsNullOrWhiteSpace(commentText_Entry.Text))
            {
                ViewModel.AddCommentCommand.Execute(commentText_Entry.Text);
                CommentView comment = new CommentView();
                comment.Text = commentText_Entry.Text;
                comment.Name = logInUser.Name;
                comment.Date = DateTime.Now;
                //Comments.Add(comment);
                Comments.Insert(0, comment);
                commentCount.Text = Comments.Count.ToString();
                commentList.IsVisible = true;
                commentsEmpty.IsVisible = false;
            }
            else
            {
                DisplayAlert("Ошибка", "Комментарий не может быть пустым.", "Ок");
            }
            commentText_Entry.Text = null;
            Comments.OrderBy(x => x.Date);
            commentList.ItemsSource = Comments;
        }

        private async void ratingButton_Clicked(object sender, EventArgs e)
        {
            int rating = ratingBar.SelectedStarValue;
            ViewModel.AddRatingCommand.Execute(rating);
            Recipe_Rating r = new Recipe_Rating();
            r.Rating_Score = rating;
            bool exist = Ratings.Any(x => x.User_Id == logInUser.Id);
            if (exist)
            {
                Recipe_Rating rating1 = Ratings.FirstOrDefault(x => x.User_Id == logInUser.Id);
                Ratings.Remove(rating1);
                Ratings.Add(r);
            }
            else
            {
                Ratings.Add(r);
            }
            GetAverage();
            
            rTotal.Text = Ratings.Count.ToString();
        }

        public void GetAverage()
        {
            double average = 0.0;
            foreach (Recipe_Rating rating in Ratings)
            {
                average += rating.Rating_Score;
            }
            if (Ratings.Count == 0)
            {
                average = 0.0;
            }
            else
            {
                average /= Ratings.Count;
            }
            rAverage.Text = average.ToString();
        }
    }

    public class Instruction
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public int Position { get; set; }
        public string Text { get; set; }
        public ImageSource ImageSource { get; set; }

        public bool IsVisibleImage { get; set; }

        public Instruction(Recipe_Instruction instr)
        {
            this.Id = instr.Id;
            this.Recipe_Id = instr.Recipe_Id;
            this.Position = instr.Position;
            this.Text = instr.Text;
            if (instr.Picture_base64 != null)
            {
                IsVisibleImage = true;
                ImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(instr.Picture_base64)));
            }
            else
            {
                ImageSource = null;
                IsVisibleImage = false;
            }
            
        }
    }

    public class CommentView
    {
        public int Id { get; set; }
        public int Recipe_Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public CommentView(Comment comment)
        {
            UserService userService = new UserService();
            Id = comment.Id;
            Recipe_Id = comment.Recipe_Id;
            Name = userService.GetById(comment.User_Id).Result.Name;
            Text = comment.Text;
            Date = comment.Date;
        }

        public CommentView()
        {
            
        }
    }
}