# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore as distinct layers
COPY ["Rater.API/Rater.API.csproj", "Rater.API/"]
COPY ["Rater.Business/Rater.Business.csproj", "Rater.Business/"]
COPY ["Rater.Data/Rater.Data.csproj", "Rater.Data/"]
COPY ["Rater.Domain/Rater.Domain.csproj", "Rater.Domain/"]
RUN dotnet restore "Rater.API/Rater.API.csproj"

# Copy the remaining files and build the project
COPY . .
WORKDIR "/src/Rater.API"
RUN dotnet build "Rater.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rater.API.csproj" -c Release -o /app/publish

# Use the official ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rater.API.dll"]
