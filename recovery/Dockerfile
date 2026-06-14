FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /api

# ferramentas base
RUN apt-get update && apt-get install -y curl unzip

# vsdbg (debugger oficial)
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

# copiar projeto
COPY api.csproj ./
COPY src ./src
COPY appsettings.json ./

# restore (melhora performance)
RUN dotnet restore

# IMPORTANTE: não iniciar app aqui
# container fica "idle" para VS Code controlar
ENTRYPOINT ["sleep", "infinity"]