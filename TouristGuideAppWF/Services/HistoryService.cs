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
        private const string HistoryFilePath = "history.json"; // Path to the JSON file storing history
        private List<SearchHistoryItem> _history; // List to hold search history items

        public HistoryService()
        {
            _history = new List<SearchHistoryItem>();
            LoadHistory(); // Load history from the JSON file during initialization
        }

        /// <summary>
        /// Returns the current search history as a list.
        /// </summary>
        public List<SearchHistoryItem> GetHistory()
        {
            return _history;
        }

        /// <summary>
        /// Loads search history from the JSON file.
        /// </summary>
        private void LoadHistory()
        {
            Console.WriteLine($"📂 Reading JSON from: {HistoryFilePath}");

            if (File.Exists(HistoryFilePath))
            {
                string json = File.ReadAllText(HistoryFilePath);
                Console.WriteLine($"📂 Loaded data from {HistoryFilePath}:\n{json}");

                _history = JsonSerializer.Deserialize<List<SearchHistoryItem>>(json) ?? new List<SearchHistoryItem>();
                Console.WriteLine("✅ History successfully loaded!");
            }
            else
            {
                _history = new List<SearchHistoryItem>();
                Console.WriteLine("⚠️ History file not found, creating a new list.");
            }
        }

        /// <summary>
        /// Saves the current search history to the JSON file.
        /// </summary>
        public void SaveHistory()
        {
            Console.WriteLine("💾 Saving history...");
            string json = JsonSerializer.Serialize(_history, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(HistoryFilePath, json);

            Console.WriteLine($"✅ History successfully saved to {HistoryFilePath}");
            Console.WriteLine(json); // Log the JSON to the console
        }

        /// <summary>
        /// Adds a new entry to the search history.
        /// </summary>
        /// <param name="city">The city name.</param>
        /// <param name="weatherInfo">Weather information for the city.</param>
        /// <param name="touristInfo">Tourist attractions information for the city.</param>
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

        /// <summary>
        /// Clears all entries from the search history.
        /// </summary>
        public void ClearHistory()
        {
            Console.WriteLine("🛑 Clearing history...");
            _history.Clear();

            // Clear the file content by writing an empty JSON array
            File.WriteAllText(HistoryFilePath, "[]");

            SaveHistory();
            Console.WriteLine("✅ History successfully cleared!");
        }

        /// <summary>
        /// Represents a single search history item.
        /// </summary>
        public class SearchHistoryItem
        {
            public string CityName { get; set; } // Name of the city
            public string WeatherInfo { get; set; } // Weather details for the city
            public string TouristInfo { get; set; } // Tourist attractions information
            public DateTime SearchTime { get; set; } // Time of the search
        }

        /// <summary>
        /// Removes an entry from the search history based on its index.
        /// </summary>
        /// <param name="index">The index of the entry to remove.</param>
        public void RemoveFromHistory(int index)
        {
            if (index >= 0 && index < _history.Count)
            {
                Console.WriteLine($"❌ Removing entry at index {index}: {_history[index].CityName}");
                _history.RemoveAt(index);
                SaveHistory();
            }
            else
            {
                Console.WriteLine("⚠ Error: Invalid index!");
            }
        }
    }
}
