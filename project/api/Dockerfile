FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /api
COPY *.csproj .
COPY *.json .

EXPOSE 25000

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:25000"]