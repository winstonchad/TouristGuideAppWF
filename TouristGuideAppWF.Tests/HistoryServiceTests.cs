using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TouristGuideAppWF.Services;
using Xunit;

namespace TouristGuideAppWF.Tests
{
    public class HistoryServiceTests
    {
        private readonly HistoryService _historyService; // Instance of HistoryService for testing

        public HistoryServiceTests()
        {
            _historyService = new HistoryService(); // Initialize HistoryService before each test
        }

        [Fact]
        public void AddToHistory_ShouldAddEntry()
        {
            _historyService.ClearHistory(); // Ensure history is empty before the test

            // Add a new entry to history
            _historyService.AddToHistory("London", "Sunny 15°C", "Big Ben, London Eye");

            var history = _historyService.GetHistory();

            // Verify that only one entry is present
            Assert.Single(history);

            // Check if the entry data matches the expected values
            Assert.Equal("London", history[0].CityName);
            Assert.Equal("Sunny 15°C", history[0].WeatherInfo);
            Assert.Equal("Big Ben, London Eye", history[0].TouristInfo);
        }

        [Fact]
        public void ClearHistory_ShouldRemoveAllEntries()
        {
            // Add an entry before testing the clear function
            _historyService.AddToHistory("New York", "Rainy 10°C", "Statue of Liberty, Times Square");

            _historyService.ClearHistory(); // Clear the history

            var history = _historyService.GetHistory();

            // Ensure the history is empty after clearing
            Assert.Empty(history);
        }

        [Fact]
        public void RemoveFromHistory_ShouldRemoveEntry()
        {
            _historyService.ClearHistory(); // Start with a clean state

            // Add multiple entries to history
            _historyService.AddToHistory("Paris", "Cloudy 20°C", "Eiffel Tower, Louvre Museum");
            _historyService.AddToHistory("Berlin", "Windy 18°C", "Brandenburg Gate, Berlin Wall");

            // Remove the first entry (Paris)
            _historyService.RemoveFromHistory(0);

            var history = _historyService.GetHistory();

            // Ensure that only one entry remains
            Assert.Single(history);

            // Verify that the remaining entry is Berlin
            Assert.Equal("Berlin", history[0].CityName);
        }

        [Fact]
        public void LoadHistory_ShouldRetrieveSavedData()
        {
            _historyService.ClearHistory(); // Clear history before the test

            // Add an entry to history
            _historyService.AddToHistory("Tokyo", "Sunny 30°C", "Shibuya Crossing, Tokyo Tower");

            // Create a new instance of HistoryService to test data persistence
            var newHistoryService = new HistoryService();
            var history = newHistoryService.GetHistory();

            // Verify that the new instance successfully retrieves the previously saved data
            Assert.Single(history);
            Assert.Equal("Tokyo", history[0].CityName);
        }
    }
}
