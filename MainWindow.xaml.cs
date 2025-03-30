using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace TestTask
{
    public partial class MainWindow : Window
    {

        private List<ClientViewModel> _allClients;
        private List<StatusViewModel> _statuses;
        public MainWindow()
        {
            InitializeComponent();
            DBConnection.Init();
            LoadClients();
            LoadAllRequests();
            LoadStatuses();
            // Подписываемся на событие SelectionChanged
            ClientsGrid.SelectionChanged += ClientsGrid_SelectionChanged;
        }

        private void Main_Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            e.Cancel = (result == MessageBoxResult.No);
        }

        // Обработчик события выбора клиента
        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is ClientViewModel selectedClient)
            {
                LoadClientRequests(selectedClient.Id);

                // Обновляем примечание для выбранного клиента
                ClientNotesTextBox.Text = selectedClient.Notes;
            }
        }

        private void LoadClients()
        {
            try
            {
                string query = "SELECT id, name, activity_sphere_id FROM clients ORDER BY name";
                DataTable clientsData = DBConnection.ExecuteQuery(query);

                _allClients = new List<ClientViewModel>();
                foreach (DataRow row in clientsData.Rows)
                {
                    _allClients.Add(new ClientViewModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString(),
                        ActivitySphereId = row["activity_sphere_id"] != DBNull.Value
                            ? Convert.ToInt32(row["activity_sphere_id"])
                            : (int?)null
                    });
                }

                // Заполняем ComboBox фильтра
                ClientFilterComboBox.ItemsSource = _allClients;

                // Загружаем полные данные для вкладки Клиенты
                LoadFullClientsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
            }
        }

        private void LoadFullClientsData()
        {
            try
            {
                string query = @"
                   SELECT
                   c.id,
                   c.name,
                   c.inn,
                   c.notes, 
                   c.activity_sphere_id,
                   a.name AS activity_sphere_name,
                   COUNT(r.id) AS request_count,
                   MAX(r.request_date) AS last_request_date
                   FROM clients c
                   LEFT JOIN activity_spheres a ON c.activity_sphere_id = a.id
                   LEFT JOIN requests r ON c.id = r.client_id
                   GROUP BY c.id, c.name, c.inn, c.notes, c.activity_sphere_id, a.name
                   ORDER BY c.name";

                DataTable clientsData = DBConnection.ExecuteQuery(query);

                List<ClientViewModel> clients = new List<ClientViewModel>();
                foreach (DataRow row in clientsData.Rows)
                {
                    clients.Add(new ClientViewModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString(),
                        Inn = row["inn"].ToString(),
                        ActivitySphereId = row["activity_sphere_id"] != DBNull.Value
                            ? Convert.ToInt32(row["activity_sphere_id"])
                            : (int?)null,
                        ActivitySphereName = row["activity_sphere_name"].ToString(),
                        RequestCount = Convert.ToInt32(row["request_count"]),
                        LastRequestDate = row["last_request_date"] != DBNull.Value
                            ? Convert.ToDateTime(row["last_request_date"]).ToShortDateString()
                            : "Нет заявок",
                        Notes = row["notes"].ToString()
                    });
                }

                ClientsGrid.ItemsSource = clients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
            }
        }

        private void LoadClientRequests(int clientId)
        {
            try
            {
                string query = @"
                    SELECT
                    r.id,
                    r.request_date,
                    r.work_name,
                    r.work_description,
                    s.name AS status_name
                    FROM
                    requests r
                    JOIN
                    request_statuses s ON r.status_id = s.id
                    WHERE
                    r.client_id = @clientId
                    ORDER BY
                    r.request_date DESC";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@clientId", clientId)
                };

                DataTable requestsData = DBConnection.ExecuteQuery(query, parameters);

                List<RequestViewModel> requests = new List<RequestViewModel>();
                foreach (DataRow row in requestsData.Rows)
                {
                    requests.Add(new RequestViewModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        RequestDate = Convert.ToDateTime(row["request_date"]).ToShortDateString(),
                        WorkName = row["work_name"].ToString(),
                        WorkDescription = row["work_description"].ToString(),
                        StatusName = row["status_name"].ToString()
                    });
                }

                ClientRequestsGrid.ItemsSource = requests;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок клиента: {ex.Message}");
            }
        }

        private void LoadAllRequests()
        {
            try
            {
                string query = @"
                    SELECT
                    r.id,
                    r.client_id,
                    c.name AS client_name, 
                    r.request_date, r.work_name, r.work_description, 
                    r.status_id, s.name AS status_name
                    FROM requests r
                    JOIN clients c ON r.client_id = c.id
                    JOIN request_statuses s ON r.status_id = s.id
                    ORDER BY r.request_date DESC";

                DataTable requestsData = DBConnection.ExecuteQuery(query);

                List<RequestViewModel> requests = new List<RequestViewModel>();
                foreach (DataRow row in requestsData.Rows)
                {
                    requests.Add(new RequestViewModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        ClientId = Convert.ToInt32(row["client_id"]),
                        ClientName = row["client_name"].ToString(),
                        RequestDate = Convert.ToDateTime(row["request_date"]).ToShortDateString(),
                        WorkName = row["work_name"].ToString(),
                        WorkDescription = row["work_description"].ToString(),
                        StatusId = Convert.ToInt32(row["status_id"]),
                        StatusName = row["status_name"].ToString()
                    });
                }

                AllRequestsGrid.ItemsSource = requests;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке всех заявок: {ex.Message}");
            }
        }

        private void LoadStatuses()
        {
            try
            {
                string query = "SELECT id, name FROM request_statuses";
                DataTable statusData = DBConnection.ExecuteQuery(query);

                _statuses = new List<StatusViewModel>();
                foreach (DataRow row in statusData.Rows)
                {
                    _statuses.Add(new StatusViewModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        StatusName = row["name"].ToString()
                    });
                }

                //  Установка источника данных для ComboBoxColumn
                var statusColumn = AllRequestsGrid.Columns[4] as DataGridComboBoxColumn;
                statusColumn.ItemsSource = _statuses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статусов: {ex.Message}");
            }
        }

        private void ClientFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyRequestsFilter();
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClientFilterComboBox.SelectedItem = null;
            ClientFilterComboBox.Text = string.Empty;
            ApplyRequestsFilter();
        }

        private void ApplyRequestsFilter()
        {
            if (AllRequestsGrid.ItemsSource == null) return;

            var view = System.Windows.Data.CollectionViewSource.GetDefaultView(AllRequestsGrid.ItemsSource);
            view.Filter = item =>
            {
                var request = item as RequestViewModel;
                if (ClientFilterComboBox.SelectedItem == null) return true;

                var selectedClient = ClientFilterComboBox.SelectedItem as ClientViewModel;
                return request.ClientId == selectedClient.Id;
            };
        }

        private void AllRequestsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Статус" && e.EditAction == DataGridEditAction.Commit)
            {
                var editedRequest = e.Row.Item as RequestViewModel;
                var comboBox = e.EditingElement as System.Windows.Controls.ComboBox;
                if (comboBox != null && editedRequest != null)
                {
                    int newStatusId = (int)comboBox.SelectedValue;

                    try
                    {
                        string query = "UPDATE requests SET status_id = @statusId WHERE id = @requestId";
                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@statusId", newStatusId),
                            new MySqlParameter("@requestId", editedRequest.Id)
                        };

                        DBConnection.ExecuteNonQuery(query, parameters);

                        // Обновляем данные в представлении
                        editedRequest.StatusId = newStatusId;
                        editedRequest.StatusName = _statuses.Find(s => s.Id == newStatusId).StatusName;

                        // Обновляем список заявок для текущего клиента (если открыта вкладка Клиенты)
                        if (ClientsGrid.SelectedItem != null)
                        {
                            var selectedClient = ClientsGrid.SelectedItem as ClientViewModel;
                            LoadClientRequests(selectedClient.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}");
                    }
                }
            }
        }

        //Метод добавления клиента
        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ClientEditWindow();
            if (editWindow.ShowDialog() == true)
            {
                LoadClients();
                LoadAllRequests();
            }
        }

        //Метод для редактирования клиента
        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is ClientViewModel selectedClient)
            {
                var client = new ClientModel
                {
                    Id = selectedClient.Id,
                    Name = selectedClient.Name,
                    Inn = selectedClient.Inn,
                    Notes = selectedClient.Notes,
                    ActivitySphereId = selectedClient.ActivitySphereId  
                };

                var editWindow = new ClientEditWindow(client);
                if (editWindow.ShowDialog() == true)
                {
                    LoadClients();
                    LoadAllRequests();
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования");
            }
        }

        //Метод для удаления клиента
        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is ClientViewModel selectedClient)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить клиента {selectedClient.Name}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Сначала удаляем связанные заявки
                        string deleteRequestsQuery = "DELETE FROM requests WHERE client_id = @clientId";
                        MySqlParameter[] requestParams = new MySqlParameter[]
                        {
                    new MySqlParameter("@clientId", selectedClient.Id)
                        };
                        DBConnection.ExecuteNonQuery(deleteRequestsQuery, requestParams);

                        // Затем удаляем клиента
                        string deleteClientQuery = "DELETE FROM clients WHERE id = @clientId";
                        DBConnection.ExecuteNonQuery(deleteClientQuery, requestParams);

                        LoadClients();
                        LoadAllRequests();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления");
            }
        }

        //Метод для добавления заявки
        private void AddRequestButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли клиент в фильтре (только на вкладке "Заявки")
            if (ClientFilterComboBox.SelectedItem is ClientViewModel selectedClient)
            {
                var editWindow = new RequestEditWindow(null, selectedClient.Id);
                if (editWindow.ShowDialog() == true)
                {
                    // 1. Очищаем выбранное значение в ComboBox
                    ClientFilterComboBox.SelectedItem = null;

                    // 2. Сбрасываем текст поиска (если используется)
                    ClientFilterComboBox.Text = string.Empty;

                    // 3. Обновляем список заявок (теперь покажет все)
                    LoadAllRequests();

                    // 4. Принудительно обновляем отображение
                    ClientFilterComboBox.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента в фильтре (вкладка 'Заявки') перед созданием заявки!",
                    "Клиент не выбран",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }           
        }

        //Метод для удаления заявки
        private void DeleteRequestButton_Click(object sender, RoutedEventArgs e)
        {
            // Переименовываем переменную, чтобы избежать конфликта имен
            var selectedRequestItem = AllRequestsGrid.SelectedItem as RequestViewModel ??
                                   ClientRequestsGrid.SelectedItem as RequestViewModel;

            if (selectedRequestItem != null)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить заявку {selectedRequestItem.WorkName}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        string query = "DELETE FROM requests WHERE id = @requestId";
                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                    new MySqlParameter("@requestId", selectedRequestItem.Id)
                        };

                        DBConnection.ExecuteNonQuery(query, parameters);

                        if (ClientsGrid.SelectedItem != null)
                        {
                            var currentClient = ClientsGrid.SelectedItem as ClientViewModel; // Переименовали
                            LoadClientRequests(currentClient.Id);
                        }
                        LoadAllRequests();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении заявки: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления");
            }
        }
    }
}
