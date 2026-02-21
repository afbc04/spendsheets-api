FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /api
COPY *.csproj .
COPY *.json .

EXPOSE 8080

ENTRYPOINT ["dotnet", "watch", "run"]