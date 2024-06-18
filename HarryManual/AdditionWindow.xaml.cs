using HarryManual.DataAccess.HarryCarrier;
using HarryManual.DataAccess.Reps;
using HarryManual.Dependencies;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Label = System.Windows.Controls.Label;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для AdditionWindow.xaml
    /// </summary>
    public partial class AdditionWindow : Window
    {
        private readonly IRep<Article> _articleRep;
        private readonly IRep<Quote> _quoteRep;
        private readonly IRep<Person_Quote> _person_QuoteRep;
        private readonly IRepExtend<Person> _personRep;
        private readonly IRep<Film> _filmRep;
        private readonly IRep<Person_Film> _person_Film;


        public AdditionWindow()
        {
            InitializeComponent();

            _articleRep = new ArticleRep(new DataBaseContext());
            _quoteRep = new QuoteRep(new DataBaseContext());
            _person_QuoteRep = new Person_QuoteRep(new DataBaseContext());
            _personRep = new PersonRep(new DataBaseContext());
            _filmRep = new FilmRep(new DataBaseContext());
            _person_Film = new Person_FilmRep(new DataBaseContext());
        }

        private void CategoryAddition(object sender, RoutedEventArgs e)
        {
            
        }



        private void QuoteRadio(object sender, RoutedEventArgs e)
        {

            Label label = new Label();
            label.Content = "Цитата";
            label.Width = 48;

            GroupBox contentGroupBox = new GroupBox();
            contentGroupBox.Height = 40;
            contentGroupBox.Header = "Контент";
            TextBox contentTextBox = new TextBox();
            contentTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            contentTextBox.TextWrapping = TextWrapping.Wrap;
            contentTextBox.Width = 630;
            contentGroupBox.Content = contentTextBox;

            GroupBox authorGroupBox = new GroupBox();
            authorGroupBox.Height = 40;
            authorGroupBox.Header = "Автор";
            TextBox authorTextBox = new TextBox();
            authorTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            authorTextBox.TextWrapping = TextWrapping.Wrap;
            authorTextBox.Width = 630;
            authorGroupBox.Content = authorTextBox;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(label);
            ((StackPanel)listViewItem.Content).Children.Add(contentGroupBox);
            ((StackPanel)listViewItem.Content).Children.Add(authorGroupBox);

            Button button = new Button();
            button.Content = "Запись";
            button.Width = 100;
            button.Click += (_sender, _e) => ReadFormData();
            ((StackPanel)listViewItem.Content).Children.Add(button);

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);

            void ReadFormData()
            {
                string contentText = contentTextBox.Text;
                string authorText = authorTextBox.Text;

                if(contentText.Length == 0 || authorText.Length == 0)
                {
                    MessageBox.Show("Ошибка. Не все поля заполнены!");
                    return;
                }

                int addedQuoteId = _quoteRep.AddItem(new Quote { 
                    Content = contentText
                });

                List<Person> personByName = _personRep.GetItems(authorText);

                if (personByName.Count == 0)
                {
                    MessageBox.Show("Ошибка. Автор не найден!");
                    return;
                }

                int addedPerson_QuoteId = _person_QuoteRep.AddItem(new Person_Quote { 
                    PersonId = personByName[0].PersonId,
                    QuoteId = addedQuoteId
                });

                MessageBox.Show("Данные записаны");

            }

        }

        private void PersRadio(object sender, RoutedEventArgs e)
        {
            Label label = new Label();
            label.Content = "Персонаж";
            label.Width = 68;

            GroupBox groupBoxName = new GroupBox();
            groupBoxName.Height = 40;
            groupBoxName.Header = "Имя";

            TextBox textBoxName = new TextBox();
            textBoxName.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxName.TextWrapping = TextWrapping.Wrap;
            textBoxName.Width = 630;

            groupBoxName.Content = textBoxName;

            GroupBox groupBoxGender = new GroupBox();
            groupBoxGender.Height = 60;
            groupBoxGender.Header = "Пол";

            StackPanel stackPanelGender = new StackPanel();

            RadioButton radioButtonMale = new RadioButton();
            radioButtonMale.Content = "Мужской";
            radioButtonMale.IsChecked = true;

            RadioButton radioButtonFemale = new RadioButton();
            radioButtonFemale.Content = "Женский";

            stackPanelGender.Children.Add(radioButtonMale);
            stackPanelGender.Children.Add(radioButtonFemale);

            groupBoxGender.Content = stackPanelGender;

            GroupBox groupBoxDescription = new GroupBox();
            groupBoxDescription.Height = 100;
            groupBoxDescription.Header = "Описание";

            TextBox textBoxDescription = new TextBox();
            textBoxDescription.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxDescription.TextWrapping = TextWrapping.Wrap;
            textBoxDescription.Width = 630;

            groupBoxDescription.Content = textBoxDescription;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(label);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxName);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxGender);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxDescription);

            Button button = new Button();
            button.Content = "Запись";
            button.Width = 100;
            button.Click += (_sender, _e) => ReadFormData();
            ((StackPanel)listViewItem.Content).Children.Add(button);

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);

            void ReadFormData()
            {
                string nameText = textBoxName.Text;
                string descriptionText = textBoxDescription.Text;
                bool? maleSex = radioButtonMale.IsChecked;

                if (nameText.Length == 0 || descriptionText.Length == 0)
                {
                    MessageBox.Show("Ошибка. Не все поля заполнены!");
                    return;
                }

                int addedPersonId = _personRep.AddItem(new Person
                {
                    Name = nameText,
                    Sex = (bool)maleSex ? "мужской" : "женский",
                    Description = descriptionText
                });

                MessageBox.Show("Данные записаны");
                  
            }


        }

        private void ArticleRadio(object sender, RoutedEventArgs e)
        {
            Label label = new Label();
            label.Content = "Статья";
            label.Width = 48;

            GroupBox groupBoxTitle = new GroupBox();
            groupBoxTitle.Height = 40;
            groupBoxTitle.Header = "Название";

            TextBox textBoxTitle = new TextBox();
            textBoxTitle.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxTitle.TextWrapping = TextWrapping.Wrap;
            textBoxTitle.Width = 630;

            groupBoxTitle.Content = textBoxTitle;

            GroupBox groupBoxDescription = new GroupBox();
            groupBoxDescription.Height = 100;
            groupBoxDescription.Header = "Описание";

            TextBox textBoxDescription = new TextBox();
            textBoxDescription.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxDescription.TextWrapping = TextWrapping.Wrap;
            textBoxDescription.Width = 630;

            groupBoxDescription.Content = textBoxDescription;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(label);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxTitle);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxDescription);

            Button button = new Button();
            button.Content = "Запись";
            button.Width = 100;
            button.Click += (_sender, _e) => ReadFormData();
            ((StackPanel)listViewItem.Content).Children.Add(button);

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);

            void ReadFormData()
            {
                string nameText = textBoxTitle.Text;
                string descriptionText = textBoxDescription.Text;

                if (nameText.Length == 0 || descriptionText.Length == 0)
                {
                    MessageBox.Show("Ошибка. Не все поля заполнены!");
                    return;
                }

                int addedPersonId = _articleRep.AddItem(new Article
                {
                    Title = nameText,
                    Description = descriptionText,
                    DateOfPublication = DateTime.Now
                });

                MessageBox.Show("Данные записаны");

            }


        }

        private void FilmRadio(object sender, RoutedEventArgs e)
        {
            List<int> actorsIds = new List<int>();

            Label label = new Label();
            label.Content = "Фильм";
            label.Width = 48;

            GroupBox groupBoxTitle = new GroupBox();
            groupBoxTitle.Height = 40;
            groupBoxTitle.Header = "Название";

            TextBox textBoxTitle = new TextBox();
            textBoxTitle.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxTitle.TextWrapping = TextWrapping.Wrap;
            textBoxTitle.Width = 630;

            groupBoxTitle.Content = textBoxTitle;

            GroupBox groupBoxNumber = new GroupBox();
            groupBoxNumber.Height = 40;
            groupBoxNumber.Header = "Номер";

            TextBox textBoxNumber = new TextBox();
            textBoxNumber.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxNumber.TextWrapping = TextWrapping.Wrap;
            textBoxNumber.Width = 630;

            groupBoxNumber.Content = textBoxNumber;

            GroupBox groupBoxDescription = new GroupBox();
            groupBoxDescription.Height = 100;
            groupBoxDescription.Header = "Описание";

            TextBox textBoxDescription = new TextBox();
            textBoxDescription.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxDescription.TextWrapping = TextWrapping.Wrap;
            textBoxDescription.Width = 630;

            groupBoxDescription.Content = textBoxDescription;

            GroupBox groupBoxDate = new GroupBox();
            groupBoxDate.Height = 60;
            groupBoxDate.Header = "Дата съемок";

            DatePicker datePicker = new DatePicker();

            groupBoxDate.Content = datePicker;

            GroupBox groupBoxActors = new GroupBox();
            groupBoxActors.Height = 100;
            groupBoxActors.Header = "Актеры";

            Label actorLabel = new Label();
            actorLabel.Content = "Актеры: ";
            actorLabel.HorizontalAlignment = HorizontalAlignment.Left;

            TextBox textBoxActor = new TextBox();
            textBoxActor.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxActor.TextWrapping = TextWrapping.Wrap;
            textBoxActor.Width = 630;

            Button actorAdditionbutton = new Button();
            actorAdditionbutton.Content = "Добавить актера";
            actorAdditionbutton.Width = 100;
            actorAdditionbutton.Click += (_sender, _e) => AddActor();

            groupBoxActors.Content = new StackPanel();
            ((StackPanel)groupBoxActors.Content).Children.Add(actorLabel);
            ((StackPanel)groupBoxActors.Content).Children.Add(textBoxActor);
            ((StackPanel)groupBoxActors.Content).Children.Add(actorAdditionbutton);

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(label);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxTitle);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxNumber);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxDescription);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxDate);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxActors);


            Button button = new Button();
            button.Content = "Запись";
            button.Width = 100;
            button.Click += (_sender, _e) => ReadFormData();
            ((StackPanel)listViewItem.Content).Children.Add(button);

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);

            void AddActor()
            {
                string nameText = textBoxActor.Text;

                List<Person> existPerson = _personRep.GetItems(nameText);

                if(existPerson == null)
                {
                    MessageBox.Show("Ошибка. Такого актера нет!");
                    return;
                }

                List<Person_Film> person_Film = _person_Film.GetItems();

                if(person_Film.Count != 0 && person_Film.FirstOrDefault(a => a.PersonId == existPerson[0].PersonId) != null)
                {
                    MessageBox.Show("Ошибка. Такой актер уже есть!");
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

            void ReadFormData()
            {
                string nameText = textBoxTitle.Text;
                string descriptionText = textBoxDescription.Text;
                string part = textBoxNumber.Text;
                DateTime selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
                
                int partNumber;
                bool success = int.TryParse(part, out partNumber);

                if (nameText.Length == 0 || descriptionText.Length == 0 || !success)
                {
                    MessageBox.Show("Ошибка. Не все поля заполнены!");
                    return;
                }

                int addedFilmId = _filmRep.AddItem(new Film
                {
                    Title = nameText,
                    Description = descriptionText,
                    Part = partNumber,
                    DateOfPublication = selectedDate
                });

                if(actorsIds.Count > 0)
                {
                    foreach(int id in actorsIds)
                    {
                        int addedId = _person_Film.AddItem(new Person_Film
                        {
                            PersonId = id,
                            FilmId = addedFilmId
                        });
                    }
                }

                MessageBox.Show("Данные записаны");

            }


        }

    }
}
