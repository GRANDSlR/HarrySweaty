﻿using HarryManual.DataAccess;
using HarryManual.DataAccess.HarryCarrier;
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
        private DataBaseContext dbContext;

        private User _user;

        private string _searchString = "";
        private string _quote = "";
        private List<CheckBox> _checkedPersonsFilter = new List<CheckBox>();
        private List<CheckBox> _checkedFilmsFilter = new List<CheckBox>();
        private RadioButton _radioFavState;

        public MainWindow(User user)
        {
            InitializeComponent();
            dbContext = new DataBaseContext();

            _user = user;

            if(user.Role != "admin")
            {
                AddCategory.Visibility = Visibility.Collapsed;
            }

        }

        private void InitData()
        {
            _searchString = MainSearchString.Text;

            _quote = Quote.Text;

            var checkBoxes = Persons.Children.OfType<CheckBox>();
            _checkedPersonsFilter = checkBoxes.Where(cb => cb.IsChecked == true).ToList();

            var checkFilterBoxes = Films.Children.OfType<CheckBox>();
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

        private void AddPersonView(object sender, RoutedEventArgs e, Person person, List<Film> films, List<Quote> quotes)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Персонажи";
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

        private void AddQuoteView(object sender, RoutedEventArgs e, Person person, Quote quote)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Цитаты";
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

        private void AddArticleView(object sender, RoutedEventArgs e, Article article)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Header = "Статьи";
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




        private void GetFilterValues(object sender, RoutedEventArgs e)
        {
            InitData();

        }

        private void AddCategory_Click_1(object sender, RoutedEventArgs e)
        {
            AdditionWindow additionWindow = new AdditionWindow();

            additionWindow.Show();
        }
    }
}

