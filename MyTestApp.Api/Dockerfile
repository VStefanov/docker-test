FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS publisher
WORKDIR /src
COPY src/. .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim as final
WORKDIR /app
COPY --from=publisher /app/publish .
ENV ASPNETCORE_URLS http://*:7000
EXPOSE 7000
ENTRYPOINT ["dotnet", "MyTestApp.Api.dll"]