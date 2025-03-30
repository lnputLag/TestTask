using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class RegisterWindow : Window
    {
        private bool _isRegistrationSuccess = false;
        public RegisterWindow()
        {
            InitializeComponent();
            this.Closing += Register_Window_Closing;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();
            string pass_2 = passBox_2.Password.Trim();

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
            else if (pass != pass_2)
            {
                MessageBox.Show("Пароли не совпадают!");
            }
            else
            {
                try
                {
                    if (RegisterUser(login, pass))
                    {
                        _isRegistrationSuccess = true;
                        this.Owner?.Show(); // Показываем окно авторизации (если Owner задан)
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        // Проверяем, существует ли пользователь
        private bool RegisterUser(string login, string pass)
        {
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            MySqlParameter[] checkParams = { new MySqlParameter("@Username", login) };

            int userCount = Convert.ToInt32(DBConnection.ExecuteScalar(checkQuery, checkParams));

            if (userCount > 0)
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка регистрации");
                return false; // Ранний выход, если пользователь существует
            }

            // Регистрируем нового пользователя
            string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
            MySqlParameter[] insertParams =
            {
                new MySqlParameter("@Username", login),
                new MySqlParameter("@Password", Hash(pass))
            };

            int rowsAffected = DBConnection.ExecuteNonQuery(insertQuery, insertParams);

            if (rowsAffected > 0)
            {
                MessageBox.Show("Регистрация прошла успешно!");
                return true;
            }

            MessageBox.Show("Ошибка при регистрации", "Ошибка");
            return false;
        }

        private void Register_Window_Closing(object sender, CancelEventArgs e)
        {
            // Если регистрация успешна - не спрашиваем подтверждение
            if (_isRegistrationSuccess)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Полный выход из приложения
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }

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
    }
}
