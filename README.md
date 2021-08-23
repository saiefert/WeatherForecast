# 🌞 API Weather Forecast
### 👨🏻‍💻 About
This project was created for studies related to webscraping, Rest APIs and Worker Services. The site climatempo was used to get the weather information.
This project uses the Postgresql database and the ORM entity framework Core, in addition to .NET 5.

### 🚀 Running the API
- This project runs on Linux, Mac or Windows
- Before building this project
  - Insert your postgresql database credentials in the Settings.json file, if you want to change the database, 
you must change the connection string both in this file and in Settings.cs, and also change the library entity
  - Use Entity Core Migration for create necessary tables (Nuget Entity framework core tools)
    ``` Migration
     PM> Add-Migration <YourNameMigration>
     PM> Update-Database
     ```
 - We should populate the cities table, where the API and service will make your connections, checking ID, names without accents ETC
  - Download the cities here, there is the entire list of Brazilian cities and their respective names and other information.
  - 👉 https://github.com/kelvins/Municipios-Brasileiros
 - The site used uses some own Ids, for this reason, you must populate the database with both the names of the cities and the IDs. There is a webscraping class for this, called CitiesCodeWebScraping, 
 where the state is inserted and in the table already populated with IBGE cities, WebScraping takes care of everything.
 
 ### 🚀 Running the Service
 - Once that's done, everything is ready for the API, now for the service to run and get the weather forecast data by period and weather forecast by day, the main .exe service must be configured.
    - Using windows, just type something like (change path)
      ``` PowerShell
      PS> sc.exe create ".NET Joke Service" binpath=C:\Path\To\App.WindowsService.exe
      ```
 - If you are using linux, check your distribution documentation, you can use either systemd or otherwise.
 
Feel free to contribute, change or give some suggestion, in case something goes wrong, contact me without problems, this is a study project and I recommend using it only for educational purposes.
