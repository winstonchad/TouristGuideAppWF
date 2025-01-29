TouristGuideAppWF
TouristGuideAppWF is a C# Windows Forms application designed to assist users in exploring tourist information about cities worldwide. The application integrates weather forecasts, tourist attractions, and map services to provide a comprehensive user experience.

Features
Weather Information
Fetches the current weather data for the selected city using the OpenWeatherMap API.

Tourist Attractions
Lists notable attractions in the city using the Google Gemini API.

Interactive Map
Opens the city location in Google Maps directly from the application.

Search History
Keeps a record of previous searches, allowing users to view, delete, or clear their history.

Technologies Used
C# with Windows Forms for the graphical user interface.
OpenWeatherMap API for weather data.
Google Gemini API for tourist attraction information.
Nominatim API for geocoding.
Dependency Injection via IHttpClientFactory.
Unit Testing using xUnit and Moq.
Installation and Setup
Prerequisites
.NET SDK (version 8.0 or later)
Internet connection for API requests
Steps
Clone the repository:
bash
Copy code
git clone https://github.com/yourusername/TouristGuideAppWF.git
Open the project in Visual Studio.
Restore NuGet packages:
bash
Copy code
dotnet restore
Build and run the project.
API Configuration
appsettings.json
Ensure that the appsettings.json file contains valid API keys:

json
Copy code
{
  "WeatherApi": {
    "ApiKey": "your_openweathermap_api_key"
  },
  "GoogleAi": {
    "ApiKey": "your_google_gemini_api_key"
  }
}
How to Use
Enter the name of the city in the input box.
Click Search to fetch weather and tourist attraction information.
Click Open Map in Browser to view the city's location on Google Maps.
Use the History menu to:
View previous searches.
Delete specific entries or clear all history.
Unit Testing
The project is covered by unit tests written with xUnit and uses Moq for mocking HTTP responses. To run the tests:

Open the solution in Visual Studio.
Run all tests via the Test Explorer.
Project Structure
Services/
Contains services responsible for API integration:

BaseService: Common HTTP functionality.
WeatherService: Interacts with OpenWeatherMap API.
GeminiService: Interacts with Google Gemini API.
NominatimService: Provides geocoding functionality.
HistoryService: Manages search history.
Tests/
Contains unit tests for all services.

Form1.cs
The main UI logic of the application.

Known Limitations
API keys included are for testing purposes and may have limited usage.
The application requires an active internet connection for API requests.
Contributing
Contributions are welcome! Please follow these steps:

Fork the repository.
Create a new branch for your feature or bug fix.
Submit a pull request.
License
This project is licensed under the MIT License. See the LICENSE file for details.