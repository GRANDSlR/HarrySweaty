using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для AdditionWindow.xaml
    /// </summary>
    public partial class AdditionWindow : Window
    {
        private DataBaseContext dbContext;

        public AdditionWindow()
        {
            InitializeComponent();
            dbContext = new DataBaseContext();

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

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);

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

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);


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

            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);


        }

        private void FilmRadio(object sender, RoutedEventArgs e)
        {
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

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = new StackPanel();
            ((StackPanel)listViewItem.Content).Children.Add(label);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxTitle);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxNumber);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxDescription);
            ((StackPanel)listViewItem.Content).Children.Add(groupBoxDate);


            ResultView.Items.Clear();
            ResultView.Items.Add(listViewItem);


        }

    }
}
