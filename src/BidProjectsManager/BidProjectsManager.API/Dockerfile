#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5219
EXPOSE 7030

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BidProjectsManager.API/BidProjectsManager.API.csproj", "BidProjectsManager.API/"]
COPY ["BidProjectsManager.DataLayer/BidProjectsManager.DataLayer.csproj", "BidProjectsManager.DataLayer/"]
COPY ["BidProjectsManager.Model/BidProjectsManager.Model.csproj", "BidProjectsManager.Model/"]
COPY ["BidProjectsManager.Logic/BidProjectsManager.Logic.csproj", "BidProjectsManager.Logic/"]
COPY ["BidProjectsManager.Mappings/BidProjectsManager.Mappings.csproj", "BidProjectsManager.Mappings/"]
COPY ["BidProjectsManager.Validation/BidProjectsManager.Validation.csproj", "BidProjectsManager.Validation/"]
RUN dotnet restore "BidProjectsManager.API/BidProjectsManager.API.csproj"
COPY . .
WORKDIR "/src/BidProjectsManager.API"
RUN dotnet build "BidProjectsManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BidProjectsManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS https://*:7030;http://*:5219
ENTRYPOINT ["dotnet", "BidProjectsManager.API.dll"]
