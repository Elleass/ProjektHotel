FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Hotel.API/Hotel.API.csproj", "src/Hotel.API/"]
COPY ["src/Hotel.Application/Hotel.Application.csproj", "src/Hotel.Application/"]
COPY ["src/Hotel.Domain/Hotel.Domain.csproj", "src/Hotel.Domain/"]
COPY ["src/Hotel.Infrastructure/Hotel.Infrastructure.csproj", "src/Hotel.Infrastructure/"]

RUN dotnet restore "src/Hotel.API/Hotel.API.csproj"

COPY . .
WORKDIR "/src/src/Hotel.API"
RUN dotnet build "Hotel.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotel.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Hotel.API.dll"]