﻿using HarryManual.DataAccess;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;


namespace HarryManual
{

    public partial class AuthWindow : Window
    {
        private DataBaseContext dbContext;

        public AuthWindow()
        {
            InitializeComponent();
            dbContext = new DataBaseContext();
        } 

        //активация ргистрации или логина при нажатии на "Вход"
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

            if (user != null && user.PasswordHash == ComputeHash(password))
            {
                MainWindow mainWondow = new MainWindow(user);

                mainWondow.Show();

                MessageBox.Show("Вы вошли в систему.");

                this.Close();
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
                User user = dbContext.Users.FirstOrDefault(u => u.Name == username);

                if (user != null)
                {
                    MessageBox.Show("Ошибка. Такой пользователь уже существует");
                    return;
                }


                var newUser = new User
                {
                    Name = username,
                    PasswordHash = ComputeHash(password),
                    Role = "user"
                };

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();


                if (dbContext.Users.Any(u => u.Name == username))
                {
                    MessageBox.Show("Пользователь успешно зарегистрирован. Войдите в систему.");
                }
                else
                {
                    MessageBox.Show("Ошибка при регистрации пользователя.");
                }
            }
        }

        //хеширование переданной строки
        public static string ComputeHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
 