#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.OBet/Tgs.GameInterface.OBet.csproj", "Tgs.GameInterface.OBet/"]
COPY ["Tgs.GameInterface.OBet.Core/Tgs.GameInterface.OBet.Core.csproj", "Tgs.GameInterface.OBet.Core/"]
RUN dotnet restore "./Tgs.GameInterface.OBet/./Tgs.GameInterface.OBet.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.OBet"
RUN dotnet build "./Tgs.GameInterface.OBet.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.OBet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.OBet.dll"]