FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./MyTestApp/MyTestApp.csproj", "MyTestApp/"]
RUN dotnet restore "MyTestApp/MyTestApp.csproj"
COPY ./MyTestApp/. .
WORKDIR "/src/MyTestApp"
RUN dotnet publish "MyTestApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS http://*:3000
ENTRYPOINT ["dotnet", "MyTestApp.dll"]