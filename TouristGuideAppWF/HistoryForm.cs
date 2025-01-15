using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TouristGuideAppWF.Services;
using System.Diagnostics;


namespace TouristGuideAppWF
{
    public partial class HistoryForm : Form
    {
        private readonly HistoryService _historyService;
        public HistoryForm(HistoryService historyService)
        {
            InitializeComponent();
            _historyService = historyService;
            LoadHistory();
        }

        private void InitializeDataGridViewColumns()
        {
            dataGridView1.Columns.Clear(); // Удаляем старые столбцы (на случай повторного открытия)

            // Добавляем столбцы
            dataGridView1.Columns.Add("CityName", "City Name");
            dataGridView1.Columns.Add("SearchTime", "Search Time");
            dataGridView1.Columns.Add("WeatherInfo", "Weather Info");
            dataGridView1.Columns.Add("TouristInfo", "Tourist Info");
        }

        private void LoadHistory()
        {
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("CityName", "City Name");
                dataGridView1.Columns.Add("SearchTime", "Search Time");
                dataGridView1.Columns.Add("WeatherInfo", "Weather Info");
                dataGridView1.Columns.Add("TouristInfo", "Tourist Info");
            }
            var history = _historyService.GetHistory();
            dataGridView1.Rows.Clear(); // Очищаем только строки, без удаления столбцов

            if (history == null || history.Count == 0)
            {
                MessageBox.Show("No history found");
                return;
            }

            foreach (var item in history)
            {
                dataGridView1.Rows.Add(item.CityName, item.SearchTime, item.WeatherInfo, item.TouristInfo);
            }
        }


        private void HistoryForm_Load(object sender, EventArgs e)
        {
            InitializeDataGridViewColumns();
            LoadHistory(); // Загружаем данные после создания столбцов
        }

        private void historyListBox(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
                _historyService.RemoveFromHistory(selectedIndex);

                LoadHistory();
            }
            else
            {
                MessageBox.Show("Please select a row to delete");
            }
        }


        private void btnOpenSelectedInMap_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string cityName = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string googleMapsUrl = $"https://www.google.com/maps/search/?api=1&query={cityName}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = googleMapsUrl,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Please select a row to open in map");
            }
        }

        private void btnCLearHistory_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to clear history?", "Confirm Clear", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                _historyService.ClearHistory();
                LoadHistory();
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
