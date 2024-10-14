#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Classifields.WebApi/Classifields.WebAPI.csproj", "Classifields.WebApi/"]
COPY ["Classifields.Infra.IoC/Classifields.Infra.IoC.csproj", "Classifields.Infra.IoC/"]
COPY ["Classifields.Application/Classifields.Application.csproj", "Classifields.Application/"]
COPY ["Classifields.Domain/Classifields.Domain.csproj", "Classifields.Domain/"]
COPY ["Classifields.Infra.Data/Classifields.Infra.Data.csproj", "Classifields.Infra.Data/"]
RUN dotnet restore "./Classifields.WebApi/Classifields.WebAPI.csproj"
COPY . .
WORKDIR "/src/Classifields.WebApi"
RUN dotnet build "./Classifields.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Classifields.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Classifields.WebAPI.dll"]