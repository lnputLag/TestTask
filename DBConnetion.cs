using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Security.Cryptography;

namespace TestTask
{
    public static class DBConnection
    {
        private static string _connectionString;

        public static void Init()
        {
            // Получаем строку подключения из app.config
            _connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            // Проверка, что строка подключения не пустая
            if (string.IsNullOrEmpty(_connectionString))
            {
                MessageBox.Show("Ошибка: строка подключения не найдена в app.config.");
                return;
            }

            // Использование строки подключения для подключения к MySQL
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Подключение к базе данных MySQL успешно установлено.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }



        // Метод для выполнения запросов без возврата данных (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при выполнении запроса: " + ex.Message);
                }
            }
            
        }

        // Метод для выполнения запросов с возвратом данных (SELECT)
        public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при выполнении запроса: " + ex.Message);
                }
            }
        }

        public static object ExecuteScalar(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при выполнении запроса: " + ex.Message);
                }
            }
        }
    }
}
