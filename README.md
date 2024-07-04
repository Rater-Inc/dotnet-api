# Rater API Backend

## Setup Instructions

1. **Clone the Repository**:

   ```git
   git https://github.com/Rater-Inc/dotnet-api.git
   cd dotnet-api
   ```

2. **Restore Dependencies**:

   ```bash
   dotnet restore
   ```

3. **Build the Project**:

   ```bash
   dotnet build
   ```

4. **Run the Project**:

    Visit http://localhost:8031/swagger/index.html for the API details, Swagger Page.

   ```bash
   dotnet run
   ```

 **Run as a Container**:

   After this command API will be accessible at 8031 port. Do not forget to pull changes from latest branch!

   ```bash
   docker-compose up --build -d
   ```

## Dependencies

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0): The core framework required to build and run the project.
- [PostgreSQL](https://www.postgresql.org): For relational database management.
- [Redis](https://redis.io): For token caching.
