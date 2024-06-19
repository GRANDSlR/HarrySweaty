using HarryManual.DataAccess.HarryCarrier;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        public ItemWindow(Person person, List<Film> films, List<Quote> quotes)
        {
            InitializeComponent();

            AddView(person, films, quotes);
        }

        public ItemWindow(Film film, List<Person> persons)
        {
            InitializeComponent();

            AddView(film, persons);
        }

        public ItemWindow(Person person, Quote quote)
        {
            InitializeComponent();

            AddView(person, quote);
        }

        public ItemWindow(Article article)
        {
            InitializeComponent();

            AddView(article);
        }

        public ItemWindow(Notes note, CustomCategory category)
        {
            InitializeComponent();

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
            nameTextBox.Text = "Имя: " + person.Name;
            nameTextBox.BorderThickness = new Thickness(0);

            TextBox facultyTextBox = new TextBox();
            facultyTextBox.Background = Brushes.Transparent;
            facultyTextBox.Text = "Пол: " + person.Sex;
            facultyTextBox.BorderThickness = new Thickness(0);

            TextBox descriptionTextBox = new TextBox();
            descriptionTextBox.Background = Brushes.Transparent;
            descriptionTextBox.Text = "Описание: " + person.Description;
            descriptionTextBox.BorderThickness = new Thickness(0);

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(facultyTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;


            GroupBox filmGroupBox = new GroupBox();
            filmGroupBox.Header = "Учaстие в фильмах";
            filmGroupBox.Width = 454;

            StackPanel filmStackPanel = new StackPanel();

            foreach(Film film in films)
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
        }

        private void AddView(Person person, Quote quote)
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

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = groupBox;

            ContentView.Items.Add(listViewItem);
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

        }

        private void AddView(Film film, List<Person> persons)
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

            stackPanel.Children.Add(nameTextBox);
            stackPanel.Children.Add(facultyTextBox);
            stackPanel.Children.Add(descriptionTextBox);

            groupBox.Content = stackPanel;


            GroupBox filmGroupBox = new GroupBox();
            filmGroupBox.Header = "Актеры";
            filmGroupBox.Width = 454;

            StackPanel filmStackPanel = new StackPanel();

            foreach (Person person in persons)
            {
                TextBox textBox = new TextBox();
                textBox.Background = Brushes.Transparent;
                textBox.Text = person.Name + " ";
                textBox.BorderThickness = new Thickness(0);

                filmStackPanel.Children.Add(textBox);
            }

            filmGroupBox.Content = filmStackPanel;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(groupBox);
            ((StackPanel)listViewItem.Content).Children.Add(filmGroupBox);

            ContentView.Items.Add(listViewItem);
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

        }


    }
}
