using HarryManual.DataAccess.HarryCarrier;
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Windows.Controls;

namespace HarryManual.Helpers
{
    public class Exporter
    {
        public static void ExportPerson(Person person, List<Film> films, List<Quote> quotes)
        {
            Application wordApp = new Application();
            Document doc = wordApp.Documents.Add();

            doc.Content.Text += "Имя персонажа: " + person.Name + Environment.NewLine;
            doc.Content.Text += "Пол персонажа: " + person.Sex + Environment.NewLine;
            doc.Content.Text += "Описание персонажа: " + person.Description + Environment.NewLine;

            doc.Content.Text += "Цитаты: " + Environment.NewLine;

            foreach(Quote quote in quotes)
            {
                doc.Content.Text += "<<" + quote.Content + ">>" + Environment.NewLine;

            }

            doc.Content.Text += "Фильмы: " + Environment.NewLine;

            foreach (Film film in films)
            {
                doc.Content.Text += film.Title + " - " + film.Part + Environment.NewLine;

            }

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectPath, $"Person_{ person.Name}.docx");

            doc.SaveAs2(filePath);
            doc.Close();
            wordApp.Quit();

        }

        public static void ExportQuote(Person person, Quote quote)
        {
            Application wordApp = new Application();
            Document doc = wordApp.Documents.Add();

            doc.Content.Text += "Имя персонажа: " + person.Name + Environment.NewLine;
            doc.Content.Text += "Пол персонажа: " + person.Sex + Environment.NewLine;
            doc.Content.Text += "Описание персонажа: " + person.Description + Environment.NewLine;

            doc.Content.Text += "Цитата: " + quote.Content + Environment.NewLine;

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectPath, $"Quote_{person.Name}.docx");

            doc.SaveAs2(filePath);
            doc.Close();
            wordApp.Quit();

        }

        public static void ExportArticle(Article article)
        {
            Application wordApp = new Application();
            Document doc = wordApp.Documents.Add();

            doc.Content.Text += "Название статьи: " + article.Title + Environment.NewLine;
            doc.Content.Text += "Дата публикации: " + article.DateOfPublication + Environment.NewLine;
            doc.Content.Text += "Содержимое: " + article.Description + Environment.NewLine;

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectPath, $"Article_{article.Title}.docx");

            doc.SaveAs2(filePath);
            doc.Close();
            wordApp.Quit();

        }

        public static void ExportFilm(Film film, List<Person> persons)
        {
            Application wordApp = new Application();
            Document doc = wordApp.Documents.Add();

            doc.Content.Text += "Название фильма: " + film.Title + Environment.NewLine;
            doc.Content.Text += "Часть фильма: " + film.Part + Environment.NewLine;
            doc.Content.Text += "Описнаие фильма: " + film.Description + Environment.NewLine;

            doc.Content.Text += "Актеры: " + Environment.NewLine;

            foreach (Person person in persons)
            {
                doc.Content.Text += person.Name + " ";

            }

            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectPath, $"Film_{film.Title}.docx");

            doc.SaveAs2(filePath);
            doc.Close();
            wordApp.Quit();

        }

        public static void ExportNote(Notes note, CustomCategory category)
        {
            Application wordApp = new Application();
            Document doc = wordApp.Documents.Add();

            doc.Content.Text += "Категория: " + category.CategoryName + Environment.NewLine;
            doc.Content.Text += "Название записи: " + note.NoteTitle + Environment.NewLine;
            doc.Content.Text += "Контент записи: " + note.NoteContent + Environment.NewLine;
            
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectPath, $"Note_{note.NoteTitle}.docx");

            doc.SaveAs2(filePath);
            doc.Close();
            wordApp.Quit();

        }



    }
}
