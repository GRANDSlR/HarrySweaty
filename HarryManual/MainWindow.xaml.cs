using HarryManual.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user;

        private List<CheckBox> checkedPersonsFilter = new List<CheckBox>();
        private List<CheckBox> checkedFilmsFilter = new List<CheckBox>();

        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;

        }

        private void InitCheckBoxes()
        {
            var checkBoxes = Persons.Children.OfType<CheckBox>();
            checkedPersonsFilter = checkBoxes.Where(cb => cb.IsChecked == true).ToList();

            var checkFilterBoxes = Films.Children.OfType<CheckBox>();
            checkedFilmsFilter = checkFilterBoxes.Where(cb => cb.IsChecked == true).ToList();
        }

        private void GetCheckedValues(object sender, RoutedEventArgs e)
        {
            InitCheckBoxes();

            string result = "";

            int counter = 0;

            foreach (CheckBox checkBox in checkedPersonsFilter)
            {
                result += checkBox.Content;
                counter++;
            }

            MessageBox.Show(result + counter);


        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

