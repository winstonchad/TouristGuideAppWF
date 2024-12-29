# WeatherTouristGuide

**WeatherTouristGuide** is a Windows Forms application designed to help users quickly find both weather information and tourist attractions for any city in the world.

## Features

1.  **City Input**: Users can enter the name of a city in the application.
2.  **Weather Information**: Upon searching, the application displays the current weather in the selected city (temperature in Celsius, and weather description).
3.  **Tourist Attractions**: The application also provides a list of 5 popular tourist attractions for the entered city, complete with short descriptions.
4.  **Instant Results**: Information is updated and displayed immediately after clicking the search button.

## Technologies Used

*   **Windows Forms (.NET 8.0)**: Used for building the application's user interface.
*   **OpenWeatherMap API**: Provides current weather data for the specified location.
*   **Google Gemini API**: Generates a list of tourist attractions with short descriptions.
*   **Nominatim API**: Transforms city names into geographical coordinates (latitude and longitude).

## Installation and Usage

1.  Make sure you have **.NET 8.0 SDK** installed on your system.
2.  Clone this repository to your local machine:

    ```bash
    git clone https://github.com/your-username/your-repository-name.git
    ```
    Replace  `https://github.com/your-username/your-repository-name.git` with actual link to your repo.
3.  Open the `WeatherTouristGuide.sln` solution file using **Visual Studio**.
4.  Configure API keys:
     - Create an `appsettings.json` file in the project root directory.
     - Add your API keys for OpenWeatherMap and Google Gemini API as shown below:
        ```json
          {
              "WeatherApi": {
               "ApiKey": "YOUR_OPENWEATHERMAP_API_KEY_HERE"
               },
              "GoogleAi":{
               "ApiKey":"YOUR_GOOGLE_GEMINI_API_KEY_HERE"
             }
           }
         ```
   -   Replace `"your_openweathermap_api_key"` and `"your_google_gemini_api_key"` with your actual API keys.
5.  Build and run the application using **Visual Studio**.
6. Enter the City name in the text box and press Search. The application will provide detailed weather information and tourist attractions for the entered City.

## Notes:
* This application uses free api keys, it may require a paid subscription to openweathermap.
*  Ensure you've enabled billing options for your Google Cloud account (in case of issues with Gemini Api)

## License
This project is distributed under the MIT license
