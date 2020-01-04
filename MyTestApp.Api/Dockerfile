FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MyTestApp.Api/MyTestApp.Api.csproj", "MyTestApp.Api/"]
RUN dotnet restore "MyTestApp.Api/MyTestApp.Api.csproj"
COPY . .
WORKDIR "/src/MyTestApp.Api"
RUN dotnet publish "MyTestApp.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyTestApp.Api.dll"]