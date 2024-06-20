using HarryManual.DataAccess;
using HarryManual.DataAccess.HarryCarrier;
using HarryManual.DataAccess.Reps;
using HarryManual.Dependencies;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private string _searchString = "";
        private string _quote = "";
        private List<CheckBox> _checkedPersonsFilter = new List<CheckBox>();
        private List<CheckBox> _checkedFilmsFilter = new List<CheckBox>();
        private RadioButton _radioFavState;

        private readonly IRep<Article> _articleRep;
        private readonly IRep<Quote> _quoteRep;
        private readonly IRep<Person_Quote> _person_QuoteRep;
        private readonly IRepExtend<Person> _personRep;
        private readonly IRep<Film> _filmRep;
        private readonly IRep<Person_Film> _person_FilmRep;
        private readonly IRep<CustomCategory> _customCategoryRep;
        private readonly IRep<Notes> _noteRep;
        private readonly IRep<CustomCategory_Note> _customCategory_NoteRep;

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

        }

        private void InitData()
        {
            _searchString = MainSearchString.Text;

            _quote = Quote.Text;

            var checkBoxes = PersonCheck.Items.OfType<CheckBox>();
            _checkedPersonsFilter = checkBoxes.Where(cb => cb.IsChecked == true).ToList();

            var checkFilterBoxes = FilmCheck.Items.OfType<CheckBox>();
            _checkedFilmsFilter = checkFilterBoxes.Where(cb => cb.IsChecked == true).ToList();

            if (RadioFavAll.IsChecked == true)
            {
                _radioFavState = RadioFavAll;
            }
            else if (RadioFavShow.IsChecked == true)
            {
                _radioFavState = RadioFavShow;
            }
            else if (RadioFavHide.IsChecked == true)
            {
                _radioFavState = RadioFavHide;
            }

        }

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

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(facultyTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(person, films, quotes);

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

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(person, quote);

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

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(article);

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

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(facultyTextBox);
            stackPanel.Children.Add(descriptionTextBox);
            stackPanel.Children.Add(textBox);

            groupBox.Content = stackPanel;
                        
            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(film, persons);

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

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;

            groupBox.MouseDoubleClick += (_sender, _e) => ClickEvent();

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ResultStack.Items.Add(listViewItem);

            void ClickEvent()
            {
                ItemWindow itemWindow = new ItemWindow(note, category);

                itemWindow.Show();
            }
        }


        private void ViewPerson(object sender, RoutedEventArgs e)
        {
            List<Person> persons = _personRep.GetItems();

            foreach (Person person in persons)
            {
                List<Person_Film> person_Films = _person_FilmRep.GetItems().Where(a => a.PersonId == person.PersonId).ToList();

                List<Film> appropriateFilms = _filmRep.GetItems().Where(a => person_Films.Where(b => b.FilmId == a.FilmId).ToList().Count > 0).ToList();

                List<Person_Quote> person_Quotes = _person_QuoteRep.GetItems().Where(a => a.PersonId == person.PersonId).ToList();

                List<Quote> appropriateQuotes = _quoteRep.GetItems().Where(a => person_Quotes.Where(b => b.QuoteId == a.QuoteId).ToList().Count > 0).ToList();

                AddView(sender, e, person, appropriateFilms, appropriateQuotes);
            }
        }


        private void GetFilterValues(object sender, RoutedEventArgs e)
        {
            InitData();

            ResultStack.Items.Clear();

            List<Person> persons = _personRep.GetItems();

            if(_searchString.Length != 0 && _searchString != "")
                persons = persons.Where(a => a.Name.Contains(_searchString)).ToList().Count > 0 ? persons.Where(a => a.Name.Contains(_searchString)).ToList() : persons;

            if (_checkedPersonsFilter.Count != 0 && _checkedPersonsFilter != null)
                persons = persons.Where(a => _checkedPersonsFilter.Where(b => b.Content.ToString() == a.Name).ToList().Count>0).ToList();


            List<Quote> quotes = _quoteRep.GetItems();

            if (_quote.Length != 0 && _quote != "")
                quotes = quotes.Where(a => a.Content.Contains(_quote)).ToList();


            List<Article> articles = _articleRep.GetItems();

            List<Film> films = _filmRep.GetItems();

            if (_searchString.Length != 0 && _searchString != "")
                films = films.Where(a => a.Title.Contains(_searchString)).ToList().Count > 0 ? films.Where(a => a.Title.Contains(_searchString)).ToList() : films;

            if (_checkedFilmsFilter.Count != 0 && _checkedFilmsFilter != null)
                films = films.Where(a => _checkedFilmsFilter.Where(b => b.Content.ToString().Contains(a.Title)).ToList().Count > 0).ToList();


            List<Person_Film> person_films = _person_FilmRep.GetItems();

            List<Person_Quote> person_quotes = _person_QuoteRep.GetItems();

            List<Notes> notes = _noteRep.GetItems();

            List<CustomCategory> customCategories = _customCategoryRep.GetItems();

            List<CustomCategory_Note> customCategory_Notes = _customCategory_NoteRep.GetItems();


            foreach (Person person in persons)
            {
                List<Person_Film> person_Films = person_films.Where(a => a.PersonId == person.PersonId).ToList();

                List<Film> appropriateFilms = films.Where(a => person_Films.Where(b => b.FilmId == a.FilmId).ToList().Count > 0).ToList();

                List<Person_Quote> person_Quotes = person_quotes.Where(a => a.PersonId == person.PersonId).ToList();

                List<Quote> appropriateQuotes = quotes.Where(a => person_Quotes.Where(b => b.QuoteId == a.QuoteId).ToList().Count > 0).ToList();

                AddView(sender, e, person, appropriateFilms, appropriateQuotes);
            }

            foreach (Quote quote in quotes)
            {
                Person_Quote appropriatePerson_Quote = person_quotes.FirstOrDefault(a => a.QuoteId == quote.QuoteId);

                Person appropriatePerson = persons.FirstOrDefault(a => a.PersonId == appropriatePerson_Quote.PersonId);

                AddView(sender, e, appropriatePerson, quote);
            }


            foreach(Article article in articles)
                AddView(sender, e, article);

            foreach (Film film in films)
            {
                Person_Film appropriatePerson_Film = person_films.FirstOrDefault(a => a.FilmId == film.FilmId);

                List<Person> appropriatePersons = persons.Where(a => a.PersonId == appropriatePerson_Film.PersonId).ToList();

                AddView(sender, e, film, appropriatePersons);
            }

            foreach (Notes note in notes)
            {
                CustomCategory_Note appropriateCustomCategory_Note = customCategory_Notes.FirstOrDefault(a => a.NoteId == note.NoteId);

                CustomCategory category = customCategories.FirstOrDefault(a => a.CategoryId == appropriateCustomCategory_Note.CustomCategoryId);

                AddView(sender, e, note, category);
            }


        }


        private void AddCategory_Click_1(object sender, RoutedEventArgs e)
        {
            AdditionWindow additionWindow = new AdditionWindow();

            additionWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddPersonCheckBoxes();

            AddFilmCheckBoxes();
        }

        private void AddPersonCheckBoxes()
        {
            StackPanel stackPanel = new StackPanel();

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

                stackPanel.Children.Add(checkBox);
            }

            PersonCheck.Items.Add(stackPanel);
        }

        public void AddFilmCheckBoxes()
        {
            StackPanel stackPanel = new StackPanel();

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

                stackPanel.Children.Add(checkBox);
            }

            FilmCheck.Items.Add(stackPanel);
        }

}
}

