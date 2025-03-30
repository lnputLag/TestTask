using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using MySql.Data.MySqlClient;

namespace TestTask
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DBConnection.Init();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Owner = this;
            registerWindow.Show();
        }

        //Создание флага для определния типа закрытия
        private bool _isNavigation = false; 

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {            
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Логин и пароль обязательны для заполнения!");
                return;
            }
            else if (login.Length <= 3)
            {
                MessageBox.Show("Введите логин длинной больше трёх символов");
            }
            else if (pass.Length <= 4)
            {
                MessageBox.Show("Введите пароль более четырёх символов");
            }
            else
            {
                try
                {
                    if (ValidateUser(login, pass))
                    {
                        _isNavigation = true;
                        MessageBox.Show("Авторизация успешна!");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
                
        private bool ValidateUser(string login, string pass)
        {
            string query = "SELECT * FROM users WHERE Username = @Username AND Password = @Password";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Username", login),
                new MySqlParameter("@Password", Hash(pass))
            };

            DataTable result = DBConnection.ExecuteQuery(query, parameters);
            return result.Rows.Count > 0;
        }

        private string Hash(string input)
        {
            byte[] temp = Encoding.UTF8.GetBytes(input);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(temp);
                return Convert.ToBase64String(hash);
            }
        }

        private void Login_Window_Closing(object sender, CancelEventArgs e)
        {
            if (_isNavigation) return; // Если это навигация - не показываем подтверждение

            // Если есть открытые дочерние окна (например, RegisterWindow) - не спрашиваем подтверждение
            if (this.OwnedWindows.Count > 0)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            e.Cancel = (result == MessageBoxResult.No);
        }

    }
}
