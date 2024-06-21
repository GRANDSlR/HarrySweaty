using HarryManual.DataAccess;
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

        private readonly User _user;

        private readonly IRep<Article> _articleRep;
        private readonly IRep<Quote> _quoteRep;
        private readonly IRep<Person_Quote> _person_QuoteRep;
        private readonly IRepExtendTitle<Person> _personRep;
        private readonly IRep<Film> _filmRep;
        private readonly IRep<Person_Film> _person_Film;
        private readonly IRep<CustomCategory> _customCategoryRep;
        private readonly IRep<Notes> _noteRep;
        private readonly IRep<CustomCategory_Note> _customCategory_NoteRep;


        public AdditionWindow(User user)
        {
            InitializeComponent();

            _user = user;

            _articleRep = new ArticleRep(new DataBaseContext());
            _quoteRep = new QuoteRep(new DataBaseContext());
            _person_QuoteRep = new Person_QuoteRep(new DataBaseContext());
            _personRep = new PersonRep(new DataBaseContext());
            _filmRep = new FilmRep(new DataBaseContext());
            _person_Film = new Person_FilmRep(new DataBaseContext());
            _customCategoryRep = new CustomCategoryRep(new DataBaseContext());
            _noteRep = new NoteRep(new DataBaseContext());
            _customCategory_NoteRep = new CustomCategory_NoteRep(new DataBaseContext());

        }

        private void CategoryAddition(object sender, RoutedEventArgs e)
        {
            CategotyAdditionWindow categotyAdditionWindow = new CategotyAdditionWindow();

            categotyAdditionWindow.Show();
        }


        //отображение формы добавления записи в зависимости от выбранной категории
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
            groupBoxActors.Header = "Персонажи";

            Label actorLabel = new Label();
            actorLabel.Content = "";
            actorLabel.HorizontalAlignment = HorizontalAlignment.Left;

            TextBox textBoxActor = new TextBox();
            textBoxActor.HorizontalAlignment = HorizontalAlignment.Left;
            textBoxActor.TextWrapping = TextWrapping.Wrap;
            textBoxActor.Width = 630;

            Button actorAdditionbutton = new Button();
            actorAdditionbutton.Content = "Добавить персонажа";
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

                if(existPerson == null || existPerson.Count() == 0)
                {
                    MessageBox.Show("Ошибка. Такого персонажа нет!");
                    return;
                }

                List<Person_Film> person_Film = _person_Film.GetItems();

                if(person_Film.Count != 0 && person_Film.FirstOrDefault(a => a.PersonId == existPerson[0].PersonId) != null)
                {
                    MessageBox.Show("Ошибка. Такой персонажа уже есть!");
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

        private void CustomRadio(object sender, RoutedEventArgs e, CustomCategory category)
        {
            Label label = new Label();
            label.Content = category.CategoryName;

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
            button.Width = 200;
            button.Click += (_sender, _e) => ReadFormData();
            ((StackPanel)listViewItem.Content).Children.Add(button);

            Button deleteButton = new Button();
            deleteButton.Content = "Удалить";
            deleteButton.Width = 200;
            deleteButton.Margin= new Thickness(0, 10, 0, 0);
            deleteButton.Click += (_sender, _e) => DeleteCategory();
            ((StackPanel)listViewItem.Content).Children.Add(deleteButton);

            if(_user.Role != "admin")
                deleteButton.Visibility = Visibility.Collapsed;

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);

            void DeleteCategory()
            {
                try
                {
                    int deletedCategoryId = _customCategoryRep.DeleteItem(category.CategoryId);

                    List<CustomCategory_Note> customCategory_NoteToDelete = _customCategory_NoteRep.GetItems()
                        .Where(a => a.CustomCategoryId == category.CategoryId).ToList();

                    int deletedCustomCategory_NoteId;

                    foreach (CustomCategory_Note item in customCategory_NoteToDelete)
                        deletedCustomCategory_NoteId = _customCategory_NoteRep.DeleteItem(item.CustomCategory_NoteId);

                    MessageBox.Show("Категрия успешно удалена!");
                }
                catch {
                    MessageBox.Show("Ошибка удаления категории!");
                }
            }

            void ReadFormData()
            {
                string nameText = textBoxTitle.Text;
                string descriptionText = textBoxDescription.Text;

                if (nameText.Length == 0 || descriptionText.Length == 0)
                {
                    MessageBox.Show("Ошибка. Не все поля заполнены!");
                    return;
                }

                int addedNoteId = _noteRep.AddItem(new Notes
                {
                    NoteTitle = nameText,
                    NoteContent = descriptionText
                });

                int addedCategoryNote = _customCategory_NoteRep.AddItem(new CustomCategory_Note
                {
                    CustomCategoryId = category.CategoryId,
                    NoteId = addedNoteId
                });

                MessageBox.Show("Данные записаны");

            }


        }
        //

        //получение категорий
        private void GetCategories(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = new StackPanel();

            RadioButton radioButton1 = new RadioButton();
            radioButton1.Content = "Цитаты";
            radioButton1.Checked += QuoteRadio;

            RadioButton radioButton2 = new RadioButton();
            radioButton2.Content = "Персонажи";
            radioButton2.Checked += PersRadio;

            RadioButton radioButton3 = new RadioButton();
            radioButton3.Content = "Статьи";
            radioButton3.Checked += ArticleRadio;

            RadioButton radioButton4 = new RadioButton();
            radioButton4.Content = "Фильмы";
            radioButton4.Checked += FilmRadio;

            stackPanel.Children.Add(radioButton1);
            stackPanel.Children.Add(radioButton2);
            stackPanel.Children.Add(radioButton3);
            stackPanel.Children.Add(radioButton4);

            List<CustomCategory> customCategories = _customCategoryRep.GetItems();

            foreach (CustomCategory category in customCategories)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Content = category.CategoryName;
                radioButton.Checked += (_sender, _e) => CustomRadio(sender, e, category);

                stackPanel.Children.Add(radioButton);
            }

            RadioView.Items.Clear();
            RadioView.Items.Add(stackPanel);
        }

        //открытие выпадающего списка -> отображение категорий
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            GetCategories(sender, e);
        }
    }
}
