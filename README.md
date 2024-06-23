# Rater API Backend

## Installation and setup 

To run the application on your local machine, make sure you have the following prerequisites installed:

- .NET 7 SDK
- Microsoft SQL Server (recommended)

1. **Clone the repository to your local machine:**

```bash 
  git clone https://github.com/Rater-Inc/dotnet-api.git
```

2. **Navigate to the project directory:**
```bash 
  cd dotnet-api
```

Remember to add your own connection string and your own token in the appsettings.json file. Otherwise the program will not work properly.

If the project does not recognize Entity Framework, please delete the NuGet package for Entity Framework and reinstall it.

4. **Run the Project**

```bash 
  dotnet run
```

### With Docker

```bash 
  docker-compose up --build -d
```
