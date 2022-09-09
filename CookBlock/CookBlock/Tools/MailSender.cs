using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using CookBlock.Models;

namespace CookBlock.Tools
{
    public class MailSender
    {
        private static string mailAdress = "kiselev.igor.student@mail.ru";
        private static string mailPassword = "VfM8L2zXjriZB9v8FZHh";
        private static string smtpLink = "smtp.mail.ru";
        private static int smtpPort = 2525;

        private SmtpClient smtpClient;
        private MailMessage mail;

        public MailSender()
        {
            smtpClient = new SmtpClient(smtpLink, smtpPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(mailAdress, mailPassword);
            smtpClient.EnableSsl = true;
            mail = new MailMessage();
        }

        public void sendReport(User user, Recipe recipe, string reportText)
        {
            mail.From = new MailAddress(mailAdress);
            mail.To.Add(new MailAddress(mailAdress));
            mail.Subject = "Жалоба по рецепту " + "\"" + recipe.Name + "\"";
            mail.Body =
                "Жалоба от пользователя " + user.Name + "\n" +
                "Дата жалобы: " + DateTime.Now + "\n" +
                "\n" +
                "Данные о рецепте: " + "\n" +
                "ID: " + recipe.Id + "\n" +
                "User_Id: " + recipe.User_Id + "\n" +
                "Name: " + recipe.Name + "\n" +
                "Food_Type_Id: " + recipe.Food_Type_Id + "\n" +
                "Date: " + recipe.Date + "\n" +
                "\n" +
                reportText;
            smtpClient.Send(mail);
        }
    }
}
