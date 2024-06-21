using HarryManual.DataAccess;
using HarryManual.DataAccess.HarryCarrier;
using HarryManual.DataAccess.Reps;
using HarryManual.Dependencies;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Linq;
using Label = System.Windows.Controls.Label;
using HarryManual.Helpers;
using Microsoft.Office.Interop.Word;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : System.Windows.Window
    {
        private User _user;

        private IRep<Article> _articleRep;
        private IRep<Quote> _quoteRep;
        private IRep<Person_Quote> _person_QuoteRep;
        private IRepExtendTitle<Person> _personRep;
        private IRep<Film> _filmRep;
        private IRep<Person_Film> _person_FilmRep;
        private IRep<CustomCategory> _customCategoryRep;
        private IRep<Notes> _noteRep;
        private IRep<CustomCategory_Note> _customCategory_NoteRep;
        private IRepExtendId<Favorite> _favoriteRep;

        private void InitHeaders(User user)
        {
            _user = user;

            if (user.Role != "admin")
            {
                Edit.Visibility = Visibility.Collapsed;
                Delete.Visibility = Visibility.Collapsed;
            }

            _articleRep = new ArticleRep(new DataBaseContext());
            _quoteRep = new QuoteRep(new DataBaseContext());
            _person_QuoteRep = new Person_QuoteRep(new DataBaseContext());
            _personRep = new PersonRep(new DataBaseContext());
            _filmRep = new FilmRep(new DataBaseContext());
            _person_FilmRep = new Person_FilmRep(new DataBaseContext());
            _customCategoryRep = new CustomCategoryRep(new DataBaseContext());
            _noteRep = new NoteRep(new DataBaseContext());
            _customCategory_NoteRep = new CustomCategory_NoteRep(new DataBaseContext());
            _favoriteRep = new FavoriteRep(new DataBaseContext());
        }

        public ItemWindow(User user, Person person, List<Film> films, List<Quote> quotes)
        {
            InitializeComponent();

            InitHeaders(user);

            AddView(person, films, quotes);
        }

        public ItemWindow(User user, Film film, List<Person> persons)
        {
            InitializeComponent();

            InitHeaders(user);

            AddView(film, persons);
        }

        public ItemWindow(User user, Person person, Quote quote)
        {
            InitializeComponent();

            InitHeaders(user);

            AddView(person, quote);
        }

        public ItemWindow(User user, Article article)
        {
            InitializeComponent();

            InitHeaders(user);

            AddView(article);
        }

        public ItemWindow(User user, Notes note, CustomCategory category)
        {
            InitializeComponent();

            InitHeaders(user);

            AddView(note, category);
        }

        private void AddView(Person person, List<Film> films, List<Quote> quotes)
        {

            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Персонажи";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = person.Name;
            nameTextBox.BorderThickness = new Thickness(0);

            Label nameLabel = new Label();

            nameLabel.Content = "Имя: ";

            StackPanel nameStack = new StackPanel();

            nameStack.Orientation = Orientation.Horizontal;

            nameStack.Children.Add(nameLabel);
            nameStack.Children.Add(nameTextBox);


            TextBox facultyTextBox = new TextBox();
            facultyTextBox.Background = Brushes.Transparent;
            facultyTextBox.Text = person.Sex;
            facultyTextBox.BorderThickness = new Thickness(0);

            Label facultyLabel = new Label();

            facultyLabel.Content = "Пол: ";

            StackPanel facultyStack = new StackPanel();

            facultyStack.Orientation = Orientation.Horizontal;

            facultyStack.Children.Add(facultyLabel);
            facultyStack.Children.Add(facultyTextBox);

            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = person.Description;
            descriptionTextBox.BorderThickness = new Thickness(0);

            Label descriptionLabel = new Label();

            descriptionLabel.Content = "Описание: ";

            StackPanel descriptionStack = new StackPanel();

            descriptionStack.Orientation = Orientation.Horizontal;

            descriptionStack.Children.Add(descriptionLabel);
            descriptionStack.Children.Add(descriptionTextBox);

            stackPanel.Children.Add(nameStack);
            stackPanel.Children.Add(facultyStack);
            stackPanel.Children.Add(descriptionStack);

            groupBox.Content = stackPanel;


            GroupBox filmGroupBox = new GroupBox();
            filmGroupBox.Header = "Учaстие в фильмах";
            filmGroupBox.Width = 454;

            StackPanel filmStackPanel = new StackPanel();

            foreach (Film film in films)
            {
                TextBox textBox = new TextBox();
                textBox.Background = Brushes.Transparent;
                textBox.Text = film.Title + " - " + film.Part;
                textBox.BorderThickness = new Thickness(0);

                filmStackPanel.Children.Add(textBox);
            }

            filmGroupBox.Content = filmStackPanel;

            GroupBox quoteGroupBox = new GroupBox();
            quoteGroupBox.Header = "Цитаты";
            quoteGroupBox.Width = 454;

            StackPanel quoteStackPanel = new StackPanel();

            foreach (Quote quote in quotes)
            {
                TextBox textBox = new TextBox();
                textBox.Background = Brushes.Transparent;
                textBox.Text = "<<" + quote.Content + ">>";
                textBox.FontStyle = FontStyles.Italic;
                textBox.BorderThickness = new Thickness(0);

                quoteStackPanel.Children.Add(textBox);
            }

            quoteGroupBox.Content = quoteStackPanel;


            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(groupBox);
            ((StackPanel)listViewItem.Content).Children.Add(filmGroupBox);
            ((StackPanel)listViewItem.Content).Children.Add(quoteGroupBox);

            ContentView.Items.Add(listViewItem);

            Edit.Click += (sender, e) => EditAction();
            Delete.Click += (sender, e) => DeleteAction();
            Export.Click += (sender, e) => ExportAction();

            void ExportAction()
            {
                Exporter.ExportPerson(person, films, quotes);

                MessageBox.Show("Данные экспортированы успешно!");
            }

            void EditAction()
            {
                int updatedPersonId = _personRep.UpdateItem(new Person
                {
                    PersonId = person.PersonId,
                    Name = nameTextBox.Text,
                    Sex = facultyTextBox.Text,
                    Description = descriptionTextBox.Text
                });

                MessageBox.Show("Объект успешно обновлен");
            }

            void DeleteAction()
            {
                int deletedPersonId = _personRep.DeleteItem(person.PersonId);

                MessageBox.Show("Объект успешно удален");

            }
        }

        private void AddView(Person person, Quote quote)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Цитата";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = person.Name;
            nameTextBox.BorderThickness = new Thickness(0);

            Label nameLabel = new Label();

            nameLabel.Content = "Имя персонажа: ";

            StackPanel nameStack = new StackPanel();

            nameStack.Orientation = Orientation.Horizontal;

            nameStack.Children.Add(nameLabel);
            nameStack.Children.Add(nameTextBox);

            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = quote.Content;
            descriptionTextBox.BorderThickness = new Thickness(0);

            Label descriptionLabel = new Label();

            descriptionLabel.Content = "Цитата: ";

            StackPanel descriptionStack = new StackPanel();

            descriptionStack.Orientation = Orientation.Horizontal;

            descriptionStack.Children.Add(descriptionLabel);
            descriptionStack.Children.Add(descriptionTextBox);

            stackPanel.Children.Add(nameStack);
            stackPanel.Children.Add(descriptionStack);

            groupBox.Content = stackPanel;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ContentView.Items.Add(listViewItem);

            Edit.Click += (sender, e) => EditAction();
            Delete.Click += (sender, e) => DeleteAction();
            Export.Click += (sender, e) => ExportAction();

            void ExportAction()
            {
                Exporter.ExportQuote(person, quote);

                MessageBox.Show("Данные экспортированы успешно!");
            }

            void EditAction()
            {
                int updatedPersonId = _personRep.UpdateItem(new Person
                {
                    PersonId = person.PersonId,
                    Name = nameTextBox.Text,
                    Sex = person.Sex,
                    Description = person.Description
                });

                int updatedQuoteId = _quoteRep.UpdateItem(new Quote
                {
                    QuoteId = quote.QuoteId,
                    Content = descriptionTextBox.Text
                });

                MessageBox.Show("Объект успешно обновлен");
            }

            void DeleteAction()
            {
                int deletedPersonId = _personRep.DeleteItem(person.PersonId);

                int deletedQuoteId = _quoteRep.DeleteItem(quote.QuoteId);

                List<Person_Quote> quote_Persons = _person_QuoteRep.GetItems().Where(a => a.QuoteId == quote.QuoteId).ToList();

                int deletedId;

                foreach (Person_Quote person_quote in quote_Persons)
                {
                    deletedId = _person_QuoteRep.DeleteItem(person_quote.Person_QuoteId);
                }

                MessageBox.Show("Объект успешно удален");

            }
        }

        private void AddView(Article article)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Статья";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = article.Title;
            nameTextBox.FontWeight = FontWeights.Bold;

            nameTextBox.BorderThickness = new Thickness(0);


            
            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = article.Description;
            descriptionTextBox.Margin = new Thickness(0, 10, 0, 0);
            descriptionTextBox.BorderThickness = new Thickness(0);

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ContentView.Items.Add(listViewItem);

            Edit.Click += (sender, e) => EditAction();
            Delete.Click += (sender, e) => DeleteAction();
            Export.Click += (sender, e) => ExportAction();

            void ExportAction()
            {
                Exporter.ExportArticle(article);

                MessageBox.Show("Данные экспортированы успешно!");
            }

            void EditAction()
            {
                int updatedArticleId = _articleRep.UpdateItem(new Article
                {
                    ArticleId = article.ArticleId,
                    Title = nameTextBox.Text,
                    Description = descriptionTextBox.Text
                });

                
                MessageBox.Show("Объект успешно обновлен");
            }

            void DeleteAction()
            {
                int deletedArticleId = _articleRep.DeleteItem(article.ArticleId);

                MessageBox.Show("Объект успешно удален");

            }
        }

        private void AddView(Film film, List<Person> persons)
        {
            List<int> actorsIds = new List<int>();

            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Фильм";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = film.Title;
            nameTextBox.BorderThickness = new Thickness(0);

            Label nameLabel = new Label();

            nameLabel.Content = "Название: ";

            StackPanel nameStack = new StackPanel();

            nameStack.Orientation = Orientation.Horizontal;

            nameStack.Children.Add(nameLabel);
            nameStack.Children.Add(nameTextBox);



            TextBox dateTextBox = new TextBox();
            dateTextBox.Background = Brushes.Transparent;
            dateTextBox.Text = film.DateOfPublication.ToString();
            dateTextBox.BorderThickness = new Thickness(0);

            Label dateLabel = new Label();

            nameLabel.Content = "Дата публикации: ";

            StackPanel dateStack = new StackPanel();

            dateStack.Orientation = Orientation.Horizontal;

            dateStack.Children.Add(dateLabel);
            dateStack.Children.Add(dateTextBox);



            TextBox facultyTextBox = new TextBox();
            facultyTextBox.Background = Brushes.Transparent;
            facultyTextBox.Text = film.Part.ToString();
            facultyTextBox.BorderThickness = new Thickness(0);

            Label facultyLabel = new Label();

            facultyLabel.Content = "Часть: ";

            StackPanel facultyStack = new StackPanel();

            facultyStack.Orientation = Orientation.Horizontal;

            facultyStack.Children.Add(facultyLabel);
            facultyStack.Children.Add(facultyTextBox);

            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = film.Description;
            descriptionTextBox.BorderThickness = new Thickness(0);

            Label descriptionLabel = new Label();

            descriptionLabel.Content = "Описание: ";

            StackPanel descriptionStack = new StackPanel();

            descriptionStack.Orientation = Orientation.Horizontal;

            descriptionStack.Children.Add(descriptionLabel);
            descriptionStack.Children.Add(descriptionTextBox);

            stackPanel.Children.Add(nameStack);
            stackPanel.Children.Add(dateStack);
            stackPanel.Children.Add(facultyStack);
            stackPanel.Children.Add(descriptionStack);

            groupBox.Content = stackPanel;


            GroupBox filmGroupBox = new GroupBox();
            filmGroupBox.Header = "Актеры";
            filmGroupBox.Width = 454;

            StackPanel filmStackPanel = new StackPanel();

            Label actorLabel = new Label();
            actorLabel.Content = "";
            actorLabel.HorizontalAlignment = HorizontalAlignment.Left;

            foreach (Person person in persons)
            {
                actorLabel.Content += person.Name + " ";
            }

            TextBox textBoxActor = new TextBox();
            textBoxActor.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxActor.TextWrapping = TextWrapping.Wrap;
            textBoxActor.Width = 630;

            Button actorAdditionbutton = new Button();
            actorAdditionbutton.Content = "Добавить персонажа";
            actorAdditionbutton.Width = 100;
            actorAdditionbutton.Click += (_sender, _e) => AddActor();

            filmStackPanel.Children.Add(actorLabel);

            if (_user.Role == "admin")
            {
                filmStackPanel.Children.Add(textBoxActor);
                filmStackPanel.Children.Add(actorAdditionbutton);
            }

            filmGroupBox.Content = filmStackPanel;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(groupBox);
            ((StackPanel)listViewItem.Content).Children.Add(filmGroupBox);

            ContentView.Items.Add(listViewItem);

            Edit.Click += (sender, e) => EditAction();
            Delete.Click += (sender, e) => DeleteAction();
            Export.Click += (sender, e) => ExportAction();

            void AddActor()
            {
                string nameText = textBoxActor.Text;

                List<Person> existPerson = _personRep.GetItems(nameText);

                
                if (existPerson == null || existPerson.Count() == 0)
                {
                    MessageBox.Show("Ошибка. Такого персонажа нет!");
                    return;
                }

                List<Person_Film> person_Film = _person_FilmRep.GetItems();

                if (person_Film.Count != 0 && person_Film.FirstOrDefault(a => a.PersonId == existPerson[0].PersonId) != null)
                {
                    MessageBox.Show("Ошибка. Такой персонаж уже есть!");
                    return;
                }

                if (nameText.Length == 0)
                {
                    MessageBox.Show("Ошибка. Не все поля заполнены!");
                    return;
                }

                actorLabel.Content += nameText + " ";

                actorsIds.Add(existPerson[0].PersonId);
            }

            void ExportAction()
            {
                Exporter.ExportFilm(film, persons);

                MessageBox.Show("Данные экспортированы успешно!");
            }

            void EditAction()
            {
                int updatedFilmId = _filmRep.UpdateItem(new Film
                {
                    FilmId = film.FilmId,
                    Title = nameTextBox.Text,
                    Part = int.Parse(facultyTextBox.Text),
                    DateOfPublication = film.DateOfPublication,
                    Description = descriptionTextBox.Text
                });

                List<string> actors = actorLabel.Content.ToString().Split(' ').ToList();

                List<string> oldActors = persons.Select(a => a.Name).ToList();

                List<string> newActors = actors.Except(oldActors).ToList();

                if(newActors.Any())
                {
                    int newPerson_FilmId;

                    foreach(string item in newActors)
                    {
                        if (item.Length > 1)
                        {
                            newPerson_FilmId = _person_FilmRep.AddItem(new Person_Film
                            {
                                FilmId = film.FilmId,
                                PersonId = _personRep.GetItems(item)[0].PersonId
                            });
                        }
                    }
                }


                MessageBox.Show("Объект успешно обновлен");
            }

            void DeleteAction()
            {
                int deletedFilmId = _filmRep.DeleteItem(film.FilmId);

                List<Person_Film> film_Persons = _person_FilmRep.GetItems().Where(a => a.FilmId == film.FilmId).ToList();

                int deletedId;

                foreach (Person_Film person_film in film_Persons)
                {
                    deletedId = _person_FilmRep.DeleteItem(person_film.Person_FilmId);
                }

                MessageBox.Show("Объект успешно удален");

            }
        }

        private void AddView(Notes note, CustomCategory category)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = category.CategoryName;
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = note.NoteTitle;
            nameTextBox.FontWeight = FontWeights.Bold;

            nameTextBox.BorderThickness = new Thickness(0);


            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = note.NoteContent;
            descriptionTextBox.Margin = new Thickness(0, 10, 0, 0);
            descriptionTextBox.BorderThickness = new Thickness(0);


            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ContentView.Items.Add(listViewItem);

            Edit.Click += (sender, e) => EditAction();
            Delete.Click += (sender, e) => DeleteAction();
            Export.Click += (sender, e) => ExportAction();

            void ExportAction()
            {
                Exporter.ExportNote(note, category);

                MessageBox.Show("Данные экспортированы успешно!");
            }

            void EditAction()
            {
                int updatedNoteId = _noteRep.UpdateItem(new Notes
                {
                    NoteId = note.NoteId,
                    NoteTitle = nameTextBox.Text,
                    NoteContent = descriptionTextBox.Text
                });


                MessageBox.Show("Объект успешно обновлен");
            }

            void DeleteAction()
            {
                int deletedNoteId = _noteRep.DeleteItem(note.NoteId);

                MessageBox.Show("Объект успешно удален");

            }
        }

    }
}
