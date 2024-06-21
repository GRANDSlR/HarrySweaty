using HarryManual.DataAccess;
using HarryManual.DataAccess.HarryCarrier;
using HarryManual.DataAccess.Reps;
using HarryManual.Dependencies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user;

        //Фильтрационные параметры
        private string _searchString = "";
        private string _quote = "";
        private List<string> _checkedPersonsFilter = new List<string>();
        private List<string> _checkedFilmsFilter = new List<string>();
        private string _radioFavState;

        //Репозитории для взаимодейтвия с БД
        private readonly IRep<Article> _articleRep;
        private readonly IRep<Quote> _quoteRep;
        private readonly IRep<Person_Quote> _person_QuoteRep;
        private readonly IRepExtendTitle<Person> _personRep;
        private readonly IRep<Film> _filmRep;
        private readonly IRep<Person_Film> _person_FilmRep;
        private readonly IRep<CustomCategory> _customCategoryRep;
        private readonly IRep<Notes> _noteRep;
        private readonly IRep<CustomCategory_Note> _customCategory_NoteRep;
        private readonly IRepExtendId<Favorite> _favoriteRep;

        public MainWindow(User user)
        {
            InitializeComponent();

            _user = user;

            if(user.Role != "admin")
            {
                AddCategory.Visibility = Visibility.Collapsed;
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

        //Поиск ближайшего child у элемента
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T result)
                {
                    return result;
                }

                var descendant = FindVisualChild<T>(child);

                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }

        //считывание фильтрационных параметров
        private void InitData()
        {
            _searchString = MainSearchString.Text;

            _quote = Quote.Text;

            _checkedPersonsFilter.Clear();

            _checkedFilmsFilter.Clear();

            foreach (ListViewItem listViewItem in PersonCheck.Items)
            {
                CheckBox checkBox = FindVisualChild<CheckBox>(listViewItem);

                if (checkBox != null && checkBox.IsChecked == true)
                {
                    _checkedPersonsFilter.Add(checkBox.Content.ToString());
                }
            }


            foreach (ListViewItem listViewItem in FilmCheck.Items)
            {
                CheckBox checkBox = FindVisualChild<CheckBox>(listViewItem);

                if (checkBox != null && checkBox.IsChecked == true)
                {
                    _checkedFilmsFilter.Add(checkBox.Content.ToString());
                }
            }


            if (RadioFavAll.IsChecked == true)
            {
                _radioFavState = RadioFavAll.Content.ToString();
            }
            else if (RadioFavShow.IsChecked == true)
            {
                _radioFavState = RadioFavShow.Content.ToString();
            }
            else if (RadioFavHide.IsChecked == true)
            {
                _radioFavState = RadioFavHide.Content.ToString();
            }

        }


        //отображение результатов поиска (динамическая вставка кода)
        private void AddView(object sender, RoutedEventArgs e, Person person, List<Film> films, List<Quote> quotes)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Персонаж";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = "Имя: "+person.Name;
            nameTextBox.BorderThickness = new Thickness(0);

            TextBox facultyTextBox = new TextBox();
            facultyTextBox.Background = Brushes.Transparent;
            facultyTextBox.Text = "Пол: "+person.Sex;
            facultyTextBox.BorderThickness = new Thickness(0);

            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = "Описание: "+person.Description;
            descriptionTextBox.BorderThickness = new Thickness(0);

            CheckBox checkBox = new CheckBox()
            {
                Content = new TextBlock()
                {
                    Text = "Избранное",
                    FontWeight = FontWeights.Bold

                },
                IsChecked = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == person.PersonId && a.Classification == "person") != null ? true : false
            };

            checkBox.Checked += (_sender, _e) => CheckBox_Checked();

            checkBox.Unchecked += (_sender, _e) => CheckBox_Unchecked();

            void CheckBox_Checked()
            {
                int favDataId = _favoriteRep.AddItem(new Favorite 
                { 
                    UserId = _user.UserId,
                    NoteId = person.PersonId,
                    Classification = "person"
                });
            }

            void CheckBox_Unchecked()
            {
                Favorite favToDelete = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == person.PersonId && a.Classification == "person");

                if (favToDelete != null)
                {
                    int favDataId = _favoriteRep.DeleteItem(favToDelete.FavoriteId);
                }
                else
                    MessageBox.Show("Ошибка. Элемент для удаления не найден!");

            }

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(facultyTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            if(_user.Role == "admin")
                stackPanel.Children.Add(checkBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);
                        
            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(_user, person, films, quotes);

                itemWindow.Show();
            }
        }

        private void AddView(object sender, RoutedEventArgs e, Person person, Quote quote)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Цитата";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = "Имя персонажа: " + person.Name;
            nameTextBox.BorderThickness = new Thickness(0);


            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = "Цитата: " + quote.Content;
            descriptionTextBox.BorderThickness = new Thickness(0);

            CheckBox checkBox = new CheckBox()
            {
                Content = new TextBlock()
                {
                    Text = "Избранное",
                    FontWeight = FontWeights.Bold

                },
                IsChecked = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == quote.QuoteId && a.Classification == "quote") != null ? true : false
            };

            checkBox.Checked += (_sender, _e) => CheckBox_Checked();

            checkBox.Unchecked += (_sender, _e) => CheckBox_Unchecked();

            void CheckBox_Checked()
            {
                int favDataId = _favoriteRep.AddItem(new Favorite
                {
                    UserId = _user.UserId,
                    NoteId = quote.QuoteId,
                    Classification = "quote"
                });
            }

            void CheckBox_Unchecked()
            {
                Favorite favToDelete = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == quote.QuoteId && a.Classification == "quote");

                if (favToDelete != null)
                {
                    int favDataId = _favoriteRep.DeleteItem(favToDelete.FavoriteId);
                }
                else
                    MessageBox.Show("Ошибка. Элемент для удаления не найден!");

            }

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            if (_user.Role == "admin")
                stackPanel.Children.Add(checkBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(_user, person, quote);

                itemWindow.Show();
            }
        }

        private void AddView(object sender, RoutedEventArgs e, Article article)
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

            CheckBox checkBox = new CheckBox()
            {
                Content = new TextBlock()
                {
                    Text = "Избранное",
                    FontWeight = FontWeights.Bold

                },
                IsChecked = _favoriteRep.GetItems()
        .FirstOrDefault(a => a.NoteId == article.ArticleId && a.Classification == "article") != null ? true : false
            };

            checkBox.Checked += (_sender, _e) => CheckBox_Checked();

            checkBox.Unchecked += (_sender, _e) => CheckBox_Unchecked();

            void CheckBox_Checked()
            {
                int favDataId = _favoriteRep.AddItem(new Favorite
                {
                    UserId = _user.UserId,
                    NoteId = article.ArticleId,
                    Classification = "article"
                });
            }

            void CheckBox_Unchecked()
            {
                Favorite favToDelete = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == article.ArticleId && a.Classification == "article");

                if (favToDelete != null)
                {
                    int favDataId = _favoriteRep.DeleteItem(favToDelete.FavoriteId);
                }
                else
                    MessageBox.Show("Ошибка. Элемент для удаления не найден!");

            }

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);


            if (_user.Role == "admin")
                stackPanel.Children.Add(checkBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(_user, article);

                itemWindow.Show();
            }
        }

        private void AddView(object sender, RoutedEventArgs e, Film film, List<Person> persons)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Фильм";
            groupBox.Width = 454;

            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Background = Brushes.Transparent;
            nameTextBox.Text = "Название: " + film.Title;
            nameTextBox.BorderThickness = new Thickness(0);

            TextBox facultyTextBox = new TextBox();
            facultyTextBox.Background = Brushes.Transparent;
            facultyTextBox.Text = "Часть: " + film.Part;
            facultyTextBox.BorderThickness = new Thickness(0);

            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = "Описание: " + film.Description;
            descriptionTextBox.BorderThickness = new Thickness(0);

            string artistNames = "";

            foreach (Person person in persons)
            {
                artistNames += person.Name + " ";
            }

            TextBox textBox = new TextBox();
            textBox.Background = Brushes.Transparent;
            textBox.Text = artistNames;
            textBox.Margin = new Thickness(0, 10, 0, 0);
            textBox.BorderThickness = new Thickness(0);

            CheckBox checkBox = new CheckBox()
            {
                Content = new TextBlock()
                {
                    Text = "Избранное",
                    FontWeight = FontWeights.Bold

                },
                IsChecked = _favoriteRep.GetItems()
        .FirstOrDefault(a => a.NoteId == film.FilmId && a.Classification == "film") != null ? true : false
            };

            checkBox.Checked += (_sender, _e) => CheckBox_Checked();

            checkBox.Unchecked += (_sender, _e) => CheckBox_Unchecked();

            void CheckBox_Checked()
            {
                int favDataId = _favoriteRep.AddItem(new Favorite
                {
                    UserId = _user.UserId,
                    NoteId = film.FilmId,
                    Classification = "film"
                });
            }

            void CheckBox_Unchecked()
            {
                Favorite favToDelete = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == film.FilmId && a.Classification == "film");

                if (favToDelete != null)
                {
                    int favDataId = _favoriteRep.DeleteItem(favToDelete.FavoriteId);
                }
                else
                    MessageBox.Show("Ошибка. Элемент для удаления не найден!");

            }

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(facultyTextBox);
            stackPanel.Children.Add(descriptionTextBox);
            stackPanel.Children.Add(textBox);

            if (_user.Role == "admin")
                stackPanel.Children.Add(checkBox);


            groupBox.Content = stackPanel;
                        
            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(_user, film, persons);

                itemWindow.Show();
            }
        }

        private void AddView(object sender, RoutedEventArgs e, Notes note, CustomCategory category)
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

            CheckBox checkBox = new CheckBox()
            {
                Content = new TextBlock()
                {
                    Text = "Избранное",
                    FontWeight = FontWeights.Bold

                },
                IsChecked = _favoriteRep.GetItems()
        .FirstOrDefault(a => a.NoteId == note.NoteId && a.Classification == "note") != null ? true : false
            };

            checkBox.Checked += (_sender, _e) => CheckBox_Checked();

            checkBox.Unchecked += (_sender, _e) => CheckBox_Unchecked();

            void CheckBox_Checked()
            {
                int favDataId = _favoriteRep.AddItem(new Favorite
                {
                    UserId = _user.UserId,
                    NoteId = note.NoteId,
                    Classification = "note"
                });
            }

            void CheckBox_Unchecked()
            {
                Favorite favToDelete = _favoriteRep.GetItems()
                    .FirstOrDefault(a => a.NoteId == note.NoteId && a.Classification == "note");

                if (favToDelete != null)
                {
                    int favDataId = _favoriteRep.DeleteItem(favToDelete.FavoriteId);
                }
                else
                    MessageBox.Show("Ошибка. Элемент для удаления не найден!");

            }

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);


            if (_user.Role == "admin")
                stackPanel.Children.Add(checkBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(_user, note, category);

                itemWindow.Show();
            }
        }

        //отображение результатов по фильтрам
        private void ViewAllResults(object sender, RoutedEventArgs e)
        {
            List<Person> persons = _personRep.GetItems();

            if (_searchString.Length != 0 && _searchString != "")
                persons = persons.Where(a => a.Name.Contains(_searchString)).ToList();

            if (_checkedPersonsFilter.Count != 0 && _checkedPersonsFilter != null)
                persons = persons.Where(person => _checkedPersonsFilter.Contains(person.Name)).ToList();

            List<Quote> quotes = _quoteRep.GetItems();

            if (_quote.Length != 0 && _quote != "")
                quotes = quotes.Where(a => a.Content.Contains(_quote)).ToList();


            List<Article> articles = _articleRep.GetItems();

            List<Film> films = _filmRep.GetItems();

            if (_searchString.Length != 0 && _searchString != "")
                films = films.Where(a => a.Title.Contains(_searchString)).ToList();

            if (_checkedFilmsFilter.Count != 0 && _checkedFilmsFilter != null)
                films = films.Where(film => _checkedFilmsFilter.Contains(film.Title)).ToList();


            List<Person_Film> person_films = _person_FilmRep.GetItems();

            List<Person_Quote> person_quotes = _person_QuoteRep.GetItems();

            List<Notes> notes = _noteRep.GetItems();

            List<CustomCategory> customCategories = _customCategoryRep.GetItems();

            List<CustomCategory_Note> customCategory_Notes = _customCategory_NoteRep.GetItems();


            ProcessPersons(sender, e, films, quotes, persons, person_films, person_quotes);
            ProcessQuotes(sender, e, persons, quotes, person_quotes);
            ProcessArticles(sender, e, articles);
            ProcessFilms(sender, e, films, person_films, persons);

            foreach (CustomCategory category in customCategories)
            {
                Button button = new Button() { Content = category.CategoryName, Height = 26, Width = 116 };

                List<CustomCategory_Note> appropriateCategoryNotes = customCategory_Notes
                    .Where(a => a.CustomCategoryId == category.CategoryId).ToList();

                List<Notes> appropriateNotes = notes.Where(a => appropriateCategoryNotes.Any(b => b.NoteId == a.NoteId)).ToList();

            ProcessNotes(sender, e, appropriateNotes, category);
            }
        }


        //генерация результатов с установлеными критериями категорий
        private void ProcessPersons(object sender, RoutedEventArgs e, List<Film> films, List<Quote> quotes, List<Person> persons, List<Person_Film> person_films, List<Person_Quote> person_quotes)
        {
            foreach (Person person in persons)
            {
                bool isExist = _favoriteRep.GetItems().FirstOrDefault(a => a.NoteId == person.PersonId && a.Classification == "person") != null;
                
                if (_radioFavState == "Только избранное")
                {
                    if (!isExist)
                        continue;
                }
                if (_radioFavState == "Скрыть избранное")
                {
                    if (isExist)
                        continue;
                }

                List<Person_Film> person_Films = person_films.Where(a => a.PersonId == person.PersonId).ToList();
                List<Film> appropriateFilms = films.Where(a => person_Films.Where(b => b.FilmId == a.FilmId).ToList().Count > 0).ToList();
                List<Person_Quote> person_Quotes = person_quotes.Where(a => a.PersonId == person.PersonId).ToList();
                List<Quote> appropriateQuotes = quotes.Where(a => person_Quotes.Where(b => b.QuoteId == a.QuoteId).ToList().Count > 0).ToList();

                if (person != null && appropriateFilms != null && appropriateQuotes != null)
                    AddView(sender, e, person, appropriateFilms, appropriateQuotes);
            }
        }

        private void ProcessQuotes(object sender, RoutedEventArgs e, List<Person> persons, List<Quote> quotes, List<Person_Quote> person_quotes)
        {
            foreach (Quote quote in quotes)
            {
                bool isExist = _favoriteRep.GetItems().FirstOrDefault(a => a.NoteId == quote.QuoteId && a.Classification == "quote") != null;

                if (_radioFavState == "Только избранное")
                {
                    if (!isExist)
                        continue;
                }
                if (_radioFavState == "Скрыть избранное")
                {
                    if (isExist)
                        continue;
                }

                Person_Quote appropriatePerson_Quote = person_quotes.FirstOrDefault(a => a.QuoteId == quote.QuoteId);
                Person appropriatePerson = persons.FirstOrDefault(a => a.PersonId == appropriatePerson_Quote.PersonId);

                if (appropriatePerson != null && quote != null)
                    AddView(sender, e, appropriatePerson, quote);
            }
        }

        private void ProcessArticles(object sender, RoutedEventArgs e, List<Article> articles)
        {
            foreach (Article article in articles)
            {
                bool isExist = _favoriteRep.GetItems().FirstOrDefault(a => a.NoteId == article.ArticleId && a.Classification == "article") != null;
                if (_radioFavState == "Только избранное")
                {
                    if (!isExist)
                        continue;
                }
                if (_radioFavState == "Скрыть избранное")
                {
                    if (isExist)
                        continue;
                }

                AddView(sender, e, article);
            }
        }

        private void ProcessFilms(object sender, RoutedEventArgs e, List<Film> films, List<Person_Film> person_films, List<Person> persons)
        {
            foreach (Film film in films)
            {
                bool isExist = _favoriteRep.GetItems().FirstOrDefault(a => a.NoteId == film.FilmId && a.Classification == "film") != null;
                if (_radioFavState == "Только избранное")
                {
                    if (!isExist)
                        continue;
                }
                if (_radioFavState == "Скрыть избранное")
                {
                    if (isExist)
                        continue;
                }

                Person_Film appropriatePerson_Film = person_films.FirstOrDefault(a => a.FilmId == film.FilmId);
                List<Person> appropriatePersons = persons.Where(a => a.PersonId == appropriatePerson_Film.PersonId).ToList();

                if (appropriatePersons != null && film != null)
                    AddView(sender, e, film, appropriatePersons);
            }
        }

        private void ProcessNotes(object sender, RoutedEventArgs e, List<Notes> notes, CustomCategory customCategory)
        {
            foreach (Notes note in notes)
            {
                bool isExist = _favoriteRep.GetItems().FirstOrDefault(a => a.NoteId == note.NoteId && a.Classification == "note") != null;
                if (_radioFavState == "Только избранное")
                {
                    if (!isExist)
                        continue;
                }
                if (_radioFavState == "Скрыть избранное")
                {
                    if (isExist)
                        continue;
                }

                AddView(sender, e, note, customCategory);
            }
        }


        //фильтрация и отображение результатов
        private void GetFilterValues(object sender, RoutedEventArgs e)
        {
            InitData();

            ResultStack.Items.Clear();

            ViewAllResults(sender, e);

        }


        //Открытие окна добавления записей
        private void AddCategory_Click_1(object sender, RoutedEventArgs e)
        {
            AdditionWindow additionWindow = new AdditionWindow();

            additionWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddPersonCheckBoxes();

            AddFilmCheckBoxes();

            ViewAllResults(sender, e);

            LoadCategoryFilter();

        }

        //отображение фильтров по Персонажам и Фильмам
        private void AddPersonCheckBoxes()
        {
            List<Person> persons = _personRep.GetItems();

            foreach(Person person in persons)
            {
                string checkBoxContent = person.Name;
                string checkBoxHorizontalAlignment = "Left";
                string checkBoxVerticalAlignment = "Top";
                string checkBoxHeight = "16";
                string checkBoxWidth = "71";

                CheckBox checkBox = new CheckBox()
                {
                    Content = checkBoxContent,
                    HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), checkBoxHorizontalAlignment),
                    VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), checkBoxVerticalAlignment),
                    Height = double.Parse(checkBoxHeight),
                    Width = double.Parse(checkBoxWidth)
                };

                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Content = checkBox;

                PersonCheck.Items.Add(listViewItem);

            }

        }

        private void AddFilmCheckBoxes()
        {
            List<Film> films = _filmRep.GetItems();

            foreach (Film film in films)
            {
                string checkBoxContent = film.Title + " - " + film.Part;
                string checkBoxHorizontalAlignment = "Left";
                string checkBoxVerticalAlignment = "Top";
                string checkBoxHeight = "16";
                string checkBoxWidth = "71";

                CheckBox checkBox = new CheckBox()
                {
                    Content = checkBoxContent,
                    HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), checkBoxHorizontalAlignment),
                    VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), checkBoxVerticalAlignment),
                    Height = double.Parse(checkBoxHeight),
                    Width = double.Parse(checkBoxWidth)
                };

                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Content = checkBox;

                FilmCheck.Items.Add(listViewItem);
            }
        }

        //загрузка категорий
        private void LoadCategoryFilter()
        {
            List<Person> persons = _personRep.GetItems();

            if (_searchString.Length != 0 && _searchString != "")
                persons = persons.Where(a => a.Name.Contains(_searchString)).ToList();

            if (_checkedPersonsFilter.Count != 0 && _checkedPersonsFilter != null)
                persons = persons.Where(person => _checkedPersonsFilter.Contains(person.Name)).ToList();

            List<Quote> quotes = _quoteRep.GetItems();

            if (_quote.Length != 0 && _quote != "")
                quotes = quotes.Where(a => a.Content.Contains(_quote)).ToList();


            List<Article> articles = _articleRep.GetItems();

            List<Film> films = _filmRep.GetItems();

            if (_searchString.Length != 0 && _searchString != "")
                films = films.Where(a => a.Title.Contains(_searchString)).ToList();

            if (_checkedFilmsFilter.Count != 0 && _checkedFilmsFilter != null)
                films = films.Where(film => _checkedFilmsFilter.Contains(film.Title)).ToList();


            List<Person_Film> person_films = _person_FilmRep.GetItems();

            List<Person_Quote> person_quotes = _person_QuoteRep.GetItems();

            List<Notes> notes = _noteRep.GetItems();

            List<CustomCategory> customCategories = _customCategoryRep.GetItems();

            List<CustomCategory_Note> customCategory_Notes = _customCategory_NoteRep.GetItems();

            Button personsButton = new Button() { Content = "Персонажи", Height = 26, Width = 116 };
            Button quotesButton = new Button() { Content = "Цитаты", Height = 26, Width = 116 };
            Button articlesButton = new Button() { Content = "Статьи", Height = 26, Width = 116 };
            Button filmsButton = new Button() { Content = "Фильмы", Height = 26, Width = 116 };
            Button allButton = new Button() { Content = "Все", Height = 26, Width = 116 };

            personsButton.Click += (sender, e) => {ResultStack.Items.Clear(); ProcessPersons(sender, e, films, quotes, persons, person_films, person_quotes); };
            quotesButton.Click += (sender, e) => {ResultStack.Items.Clear(); ProcessQuotes(sender, e, persons, quotes, person_quotes); };
            articlesButton.Click += (sender, e) => {ResultStack.Items.Clear(); ProcessArticles(sender, e, articles); };
            filmsButton.Click += (sender, e) =>  {ResultStack.Items.Clear(); ProcessFilms(sender, e, films, person_films, persons); };
            allButton.Click += (sender, e) => { ResultStack.Items.Clear(); ViewAllResults(sender, e); } ;

            CategoryFilter.Items.Add(new ListViewItem() { Content = personsButton });
            CategoryFilter.Items.Add(new ListViewItem() { Content = quotesButton });
            CategoryFilter.Items.Add(new ListViewItem() { Content = articlesButton });
            CategoryFilter.Items.Add(new ListViewItem() { Content = filmsButton });

            foreach (CustomCategory category in customCategories)
            {
                Button button = new Button() { Content = category.CategoryName, Height = 26, Width = 116 };

                List<CustomCategory_Note> appropriateCategoryNotes = customCategory_Notes
                    .Where(a => a.CustomCategoryId == category.CategoryId).ToList();

                List<Notes> appropriateNotes = notes.Where(a => appropriateCategoryNotes.Any(b => b.NoteId == a.NoteId)).ToList();

                button.Click += (sender, e) => { ResultStack.Items.Clear(); ProcessNotes(sender, e, appropriateNotes, category); };
            
                CategoryFilter.Items.Add(new ListViewItem() { Content = button });
            }

            CategoryFilter.Items.Add(new ListViewItem() { Content = allButton });

        }

        //обнуление фильрационных параметров
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultStack.Items.Clear();

            CategoryFilter.Items.Clear();

            LoadCategoryFilter();

            _searchString = "";
            _quote = "";
            _checkedPersonsFilter.Clear();
            _checkedFilmsFilter.Clear();
            _radioFavState = "Все";

            MainSearchString.Text = "";

            Quote.Text = "";

            foreach (ListViewItem listViewItem in PersonCheck.Items)
            {
                CheckBox checkBox = FindVisualChild<CheckBox>(listViewItem);

                checkBox.IsChecked = false;
            }


            foreach (ListViewItem listViewItem in FilmCheck.Items)
            {
                CheckBox checkBox = FindVisualChild<CheckBox>(listViewItem);

                checkBox.IsChecked = false;

            }

            RadioFavAll.IsChecked = true;

            ViewAllResults(sender, e);
        }

        //открытие справки
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectPath, $"Test.chm");

            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при открытии файла справки: " + ex.Message);
            }
        }
    }
}

