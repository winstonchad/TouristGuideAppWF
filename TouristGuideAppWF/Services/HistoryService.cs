using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace TouristGuideAppWF.Services
{
    public class HistoryService
    {
        private const string HistoryFilePath = "history.json";
        private List<SearchHistoryItem> _history;

        public HistoryService()
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            if (File.Exists(HistoryFilePath))
            {
                string json = File.ReadAllText(HistoryFilePath);

                _history = JsonSerializer.Deserialize<List<SearchHistoryItem>>(json) ?? new List<SearchHistoryItem>();
            }
            else
            {
                _history = new List<SearchHistoryItem>();
            }
        }

        public void SaveHistory()
        {
            string json = JsonSerializer.Serialize(_history, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(HistoryFilePath, json);
        }

        public void AddToHistory(string city, string weatherInfo, string touristInfo)
        {
            _history.Add(new SearchHistoryItem
            {
                CityName = city,
                WeatherInfo = weatherInfo,
                TouristInfo = touristInfo,
                SearchTime = DateTime.Now
            });
            SaveHistory();
        }

        public List<SearchHistoryItem> GetHistory()
        {
            return _history;
        }

        public void ClearHistory()
        {
            _history.Clear();
            SaveHistory();
        }

        public void RemoveEntry(SearchHistoryItem entry)
        {
            _history.Remove(entry);
            SaveHistory();
        }

        public class SearchHistoryItem
        {
            public string CityName { get; set; }
            public string WeatherInfo { get; set; }
            public string TouristInfo { get; set; }
            public DateTime SearchTime { get; set; }
        }
}
