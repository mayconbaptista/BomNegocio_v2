# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Esta fase é usada para compilar o projeto de serviço
# Cross-compile: o SDK roda na arquitetura do runner (BUILDPLATFORM, x64) e
# gera binarios para TARGETARCH (arm64 na t4g). Muito mais rapido que emular
# o SDK inteiro via QEMU. O runtime final e puxado na arquitetura alvo.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["Auth.WebApi/Auth.WebApi.csproj", "Auth.WebApi/"]
COPY ["BuildBlocks.WebApi/BuildBlocks.WebApi.csproj", "BuildBlocks.WebApi/"]
COPY ["BuildBlocks.Domain/BuildBlocks.Domain.csproj", "BuildBlocks.Domain/"]
RUN dotnet restore "./Auth.WebApi/Auth.WebApi.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/Auth.WebApi"

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.WebApi.csproj" -c $BUILD_CONFIGURATION -a $TARGETARCH --no-restore -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.WebApi.dll"]
