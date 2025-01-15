using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TouristGuideAppWF.Services;
using Xunit;

public class HistoryServiceTests
{
    private const string TestHistoryFilePath = "test_history.json";
    private HistoryService _historyService;

    public HistoryServiceTests()
    {
        // Remove the test file before each test run to ensure a clean state
        if (File.Exists(TestHistoryFilePath))
        {
            File.Delete(TestHistoryFilePath);
        }

        _historyService = new HistoryService();
    }

    [Fact]
    public void AddToHistory_ShouldAddEntry()
    {
        // Clear history before running the test
        _historyService.ClearHistory();

        // Add a new entry to the history
        _historyService.AddToHistory("Paris", "Sunny 25°C", "Eiffel Tower, Louvre Museum");

        // Retrieve the updated history
        var history = _historyService.GetHistory();

        // Verify that the history contains exactly one entry
        Assert.Single(history);

        // Verify that the added entry is correct
        var entry = history[0];
        Assert.Equal("Paris", entry.CityName);
        Assert.Equal("Sunny 25°C", entry.WeatherInfo);
        Assert.Equal("Eiffel Tower, Louvre Museum", entry.TouristInfo);
    }

    [Fact]
    public void ClearHistory_ShouldRemoveAllEntries()
    {
        // Arrange: Add multiple entries to the history
        _historyService.AddToHistory("London", "Cloudy 15°C", "Big Ben, Buckingham Palace");
        _historyService.AddToHistory("New York", "Rainy 10°C", "Statue of Liberty, Times Square");

        // Act: Clear the history
        _historyService.ClearHistory();
        var history = _historyService.GetHistory();

        // Assert: Verify that history is now empty
        Assert.Empty(history);
    }

    [Fact]
    public void RemoveFromHistory_ShouldRemoveCorrectEntry()
    {
        // Arrange: Add multiple entries to history
        _historyService.AddToHistory("Tokyo", "Sunny 30°C", "Shibuya Crossing, Tokyo Tower");
        _historyService.AddToHistory("Berlin", "Cloudy 12°C", "Brandenburg Gate, Berlin Wall");
        _historyService.AddToHistory("Sydney", "Windy 22°C", "Sydney Opera House, Bondi Beach");

        // Act: Remove the second entry (Berlin)
        _historyService.RemoveFromHistory(1);
        var history = _historyService.GetHistory();

        // Assert: Verify that the history contains only two entries now
        Assert.Equal(2, history.Count);
        Assert.Equal("Tokyo", history[0].CityName);
        Assert.Equal("Sydney", history[1].CityName);
    }

    [Fact]
    public void LoadHistory_ShouldRetrieveSavedData()
    {
        // Step 1: Clear history before the test
        _historyService.ClearHistory();

        // Step 2: Add one entry to the history
        _historyService.AddToHistory("Moscow", "Snowy -5°C", "Red Square, Kremlin");

        // Step 3: Create a new instance of HistoryService to verify data persistence
        var newHistoryService = new HistoryService();
        var loadedHistory = newHistoryService.GetHistory();

        // Step 4: Verify that the loaded history contains exactly one entry
        Assert.Single(loadedHistory);

        // Step 5: Verify that the loaded entry is correct
        var entry = loadedHistory[0];
        Assert.Equal("Moscow", entry.CityName);
        Assert.Equal("Snowy -5°C", entry.WeatherInfo);
        Assert.Equal("Red Square, Kremlin", entry.TouristInfo);
    }
}
