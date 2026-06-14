# ---------- Build & Publish ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Program.csproj .
RUN dotnet restore ./Program.csproj

COPY src/. .
WORKDIR /src
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Program.dll"]

# ---------- Dev  ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /app

EXPOSE 8080

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]
