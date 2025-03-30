using System;
using System.Collections.Generic;
using System.Data;
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
using MySql.Data.MySqlClient;

namespace TestTask
{
    public partial class ClientEditWindow : Window
    {
        public ClientModel Client { get; private set; }
        private bool IsEditMode => Client != null && Client.Id > 0;
        private DataTable _activitySpheres;
        public ClientEditWindow(ClientModel client = null)
        {
            InitializeComponent();
            Client = client ?? new ClientModel();
            LoadActivitySpheres();
            LoadClientData();
        }

        private void LoadActivitySpheres()
        {
            try
            {
                string query = "SELECT id, name FROM activity_spheres";
                _activitySpheres = DBConnection.ExecuteQuery(query);
                ActivitySphereComboBox.ItemsSource = _activitySpheres.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сфер деятельности: {ex.Message}");
            }
        }

        private void LoadClientData()
        {
            NameTextBox.Text = Client.Name;
            InnTextBox.Text = Client.Inn;
            NotesTextBox.Text = Client.Notes;

            if (Client.ActivitySphereId.HasValue)
            {
                foreach (DataRowView item in ActivitySphereComboBox.Items)
                {
                    if (Convert.ToInt32(item["id"]) == Client.ActivitySphereId.Value)
                    {
                        ActivitySphereComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Наименование клиента обязательно для заполнения!");
                return;
            }

            // Проверка длины ИНН (должно быть ровно 10 символов)
            string inn = InnTextBox.Text.Trim();
            if (inn.Length != 10)
            {
                MessageBox.Show("ИНН должен содержать 10 цифр!");
                return;
            }
            //Проверка заполнения поля примечание
            string note = NotesTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(note))
            {
                MessageBox.Show("Примечание обязательно для заполнения!");
                return;
            }
            // Проверка выбора в ComboBox "Сфера деятельности"
            if (ActivitySphereComboBox.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали сферу деятельности!");
                ActivitySphereComboBox.Focus();
                return;
            }

                Client.Name = NameTextBox.Text.Trim();
                Client.Inn = InnTextBox.Text.Trim();
                Client.Notes = NotesTextBox.Text.Trim();

            if (ActivitySphereComboBox.SelectedItem != null)
            {
                var selectedRow = (DataRowView)ActivitySphereComboBox.SelectedItem;
                Client.ActivitySphereId = Convert.ToInt32(selectedRow["id"]);
            }
            else
            {
                Client.ActivitySphereId = null;
            }

            try
            {
                if (IsEditMode)
                {
                    UpdateClient();
                }
                else
                {
                    InsertClient();
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения клиента: {ex.Message}");
            }
        }

        private void UpdateClient()
        {
            string query = @"UPDATE clients 
                           SET name = @name, inn = @inn, 
                           activity_sphere_id = @activitySphereId, notes = @notes
                           WHERE id = @id";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", Client.Name),
                new MySqlParameter("@inn", Client.Inn),
                new MySqlParameter("@activitySphereId", Client.ActivitySphereId ?? (object)DBNull.Value),
                new MySqlParameter("@notes", Client.Notes ?? (object)DBNull.Value),
                new MySqlParameter("@id", Client.Id)
            };

            DBConnection.ExecuteNonQuery(query, parameters);
        }

        private void InsertClient()
        {
            string query = @"INSERT INTO clients (name, inn, activity_sphere_id, notes)
                            VALUES (@name, @inn, @activitySphereId, @notes);
                            SELECT LAST_INSERT_ID();";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", Client.Name),
                new MySqlParameter("@inn", Client.Inn),
                new MySqlParameter("@activitySphereId", Client.ActivitySphereId ?? (object)DBNull.Value),
                new MySqlParameter("@notes", Client.Notes ?? (object)DBNull.Value)
            };

            Client.Id = Convert.ToInt32(DBConnection.ExecuteScalar(query, parameters));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
