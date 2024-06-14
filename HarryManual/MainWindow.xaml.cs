using HarryManual;
using HarryManual.DataAccess;
using System.Data.Entity;
using System.IO.Packaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


namespace HarryManual
{

    public partial class MainWindow : Window
    {
        private DataBaseContext dbContext;

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new DataBaseContext();
        } 

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            if (rbRegistration.IsChecked == true)
            {
                Register(username, password);
            }
            else
            {
                Login(username, password);
            }
        }

        private void Login(string username, string password)
        {
            User user = dbContext.Users.FirstOrDefault(u => u.Name == username);

            if (user != null && user.PasswordHash == password)
            {
                MessageBox.Show("Вы вошли в систему.");
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }


        private void Register(string username, string password)
        {
            using (var dbContext = new DataBaseContext())
            {
                var newUser = new User
                {
                    Name = username,
                    PasswordHash = password,
                    Role = "user"
                };

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();


                if (dbContext.Users.Any(u => u.Name == username))
                {
                    MessageBox.Show("Пользователь успешно зарегистрирован.");
                }
                else
                {
                    MessageBox.Show("Ошибка при регистрации пользователя.");
                }
            }
        }


        /*        private string CalculateHash(string input)
                {
                    // Ваш код для вычисления хэша пароля.
                    // Здесь вы можете использовать, например, алгоритм SHA256.
                }*/
    }
}