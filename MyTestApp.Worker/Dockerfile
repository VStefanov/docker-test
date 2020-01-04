FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./MyTestApp.Worker/MyTestApp.Worker.csproj", "MyTestApp.Worker/"]
RUN dotnet restore "MyTestApp.Worker/MyTestApp.Worker.csproj"
COPY ./MyTestApp.Worker/. .
WORKDIR "/src/MyTestApp.Worker"
RUN dotnet build "MyTestApp.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyTestApp.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyTestApp.Worker.dll"]