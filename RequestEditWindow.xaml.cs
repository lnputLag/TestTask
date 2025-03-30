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
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.Ocsp;

namespace TestTask
{
    public partial class RequestEditWindow : Window
    {
        public RequestModel Request { get; private set; }
        private bool IsEditMode => Request != null && Request.Id > 0;
        private DataTable _clients;
        private DataTable _statuses;
        public RequestEditWindow(RequestModel request = null, int? clientId = null)
        {
            InitializeComponent();
            Request = request ?? new RequestModel();

            if (clientId.HasValue)
            {
                Request.ClientId = clientId.Value;
                ClientComboBox.IsReadOnly = true; // Блокируем выбор клиента, если он уже задан
            }
            LoadStatuses();
            LoadRequestData();
        }
       

        private void LoadStatuses()
        {
            try
            {
                string query = "SELECT id, name FROM request_statuses";
                _statuses = DBConnection.ExecuteQuery(query);
                StatusComboBox.ItemsSource = _statuses.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}");
            }
        }

        private void LoadRequestData()
        {
            {
                string query = "SELECT name FROM clients WHERE id = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id", Request.ClientId)
            };
                var clientsdata = DBConnection.ExecuteQuery(query, parameters);
                ClientComboBox.Text = clientsdata.Rows[0].ItemArray[0].ToString();
            }

            WorkNameTextBox.Text = Request.WorkName;
            WorkDescriptionTextBox.Text = Request.WorkDescription;

            // Установка статуса
            foreach (DataRowView item in StatusComboBox.Items)
            {
                if (Convert.ToInt32(item["id"]) == Request.StatusId)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void Request_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WorkNameTextBox.Text))
            {
                MessageBox.Show("Наименование работ обязательно для заполнения!");
                return;
            }

            Request.WorkName = WorkNameTextBox.Text.Trim();
            Request.WorkDescription = WorkDescriptionTextBox.Text.Trim();

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать статус!");
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkDescriptionTextBox.Text))
            {
                MessageBox.Show("Описание работ обязательно для заполнения!");
                return;
            }

            var selectedStatus = (DataRowView)StatusComboBox.SelectedItem;
            Request.StatusId = Convert.ToInt32(selectedStatus["id"]);

            try
            {
                if (IsEditMode)
                {
                    UpdateRequest();
                }
                else
                {
                    InsertRequest();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения заявки: {ex.Message}");
            }
        }

        private void UpdateRequest()
        {
            string query = @"UPDATE requests 
                            SET work_name = @workName, work_description = @workDescription,
                            status_id = @statusId
                            WHERE id = @id";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workName", Request.WorkName),
                new MySqlParameter("@workDescription", Request.WorkDescription ?? (object)DBNull.Value),
                new MySqlParameter("@statusId", Request.StatusId),
                new MySqlParameter("@id", Request.Id)
            };

            DBConnection.ExecuteNonQuery(query, parameters);
        }

        private void InsertRequest()
        {
            string query = @"INSERT INTO requests (client_id, work_name, work_description, status_id, request_date)
                            VALUES (@clientId, @workName, @workDescription, @statusId, NOW());
                            SELECT LAST_INSERT_ID();";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@clientId", Request.ClientId),
                new MySqlParameter("@workName", Request.WorkName),
                new MySqlParameter("@workDescription", Request.WorkDescription ?? (object)DBNull.Value),
                new MySqlParameter("@statusId", Request.StatusId)
            };

            Request.Id = Convert.ToInt32(DBConnection.ExecuteScalar(query, parameters));
        }

        private void Request_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
