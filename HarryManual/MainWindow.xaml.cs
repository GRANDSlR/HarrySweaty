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

        private string _searchString = "";
        private string _quote = "";
        private List<CheckBox> _checkedPersonsFilter = new List<CheckBox>();
        private List<CheckBox> _checkedFilmsFilter = new List<CheckBox>();
        private RadioButton _radioFavState;

        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;

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

        private void GetFilterValues(object sender, RoutedEventArgs e)
        {
            InitData();



        }

    }
}

