#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.OBet/Tgs.GameInterface.OBet.csproj", "Tgs.GameInterface.OBet/"]
COPY ["Tgs.GameInterface.OBet.Core/Tgs.GameInterface.OBet.Core.csproj", "Tgs.GameInterface.OBet.Core/"]
RUN dotnet restore "Tgs.GameInterface.OBet/Tgs.GameInterface.OBet.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.OBet"
RUN dotnet build "Tgs.GameInterface.OBet.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameInterface.OBet.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.OBet.dll"]