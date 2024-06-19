using HarryManual.DataAccess.HarryCarrier;
using HarryManual.DataAccess.Reps;
using HarryManual.Dependencies;
using System.Windows;

namespace HarryManual
{
    /// <summary>
    /// Логика взаимодействия для CategotyAdditionWindow.xaml
    /// </summary>
    public partial class CategotyAdditionWindow : Window
    {
        private readonly IRep<CustomCategory> _customCategoryRep;

        public CategotyAdditionWindow()
        {
            InitializeComponent();

            _customCategoryRep = new CustomCategoryRep(new DataBaseContext());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string categoryTitle = categoryName.Text;

            if(categoryTitle.Length == 0)
            {
                MessageBox.Show("Ошибка. Не все поля заполнены!");
                return;
            }

            int addedCategoryId = _customCategoryRep.AddItem(new CustomCategory
            {
                CategoryName = categoryTitle
            });

            MessageBox.Show("Новая категория добавлена успешно");

            this.Close();

        }
    }
}
