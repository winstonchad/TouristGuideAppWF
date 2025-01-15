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
            _history = new List<SearchHistoryItem>();
            LoadHistory();
        }

        public List<SearchHistoryItem> GetHistory()
        {
            return _history;
        }

        private void LoadHistory()
        {
            Console.WriteLine($"📂 Читаем JSON из: {HistoryFilePath}");

            if (File.Exists(HistoryFilePath))
            {
                string json = File.ReadAllText(HistoryFilePath);
                Console.WriteLine($"📂 Загруженные данные из {HistoryFilePath}:\n{json}");

                _history = JsonSerializer.Deserialize<List<SearchHistoryItem>>(json) ?? new List<SearchHistoryItem>();
                Console.WriteLine("✅ История загружена!");
            }
            else
            {
                _history = new List<SearchHistoryItem>();
                Console.WriteLine("⚠️ Файл истории не найден, создаем новый список.");
            }
        }

        public void SaveHistory()
        {
            Console.WriteLine("💾 Сохранение истории...");
            string json = JsonSerializer.Serialize(_history, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(HistoryFilePath, json);

            Console.WriteLine($"✅ История успешно сохранена в {HistoryFilePath}");
            Console.WriteLine(json); // Выводим JSON в консоль
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
        

        public void ClearHistory()
        {
            Console.WriteLine("🛑 Очистка истории...");
            _history.Clear();

            // Очищаем сам файл, записав в него пустой список JSON
            File.WriteAllText(HistoryFilePath, "[]");

            SaveHistory();
            Console.WriteLine("✅ История очищена!");
        }

        public class SearchHistoryItem
        {
            public string CityName { get; set; }
            public string WeatherInfo { get; set; }
            public string TouristInfo { get; set; }
            public DateTime SearchTime { get; set; }
        }


        public void RemoveFromHistory(int index)
        {
            if (index >= 0 && index < _history.Count)
            {
                Console.WriteLine($"❌ Удаляем {index}-ю запись: {_history[index].CityName}");
                _history.RemoveAt(index);
                SaveHistory();
            }
            else
            {
                Console.WriteLine("⚠ Ошибка: Некорректный индекс!");
            }
        }
    }
}
