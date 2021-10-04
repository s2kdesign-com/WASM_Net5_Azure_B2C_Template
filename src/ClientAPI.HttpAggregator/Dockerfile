#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClientAPI.HttpAggregator/S2kDesignTemplate.ClientAPI.HttpAggregator.csproj", "ClientAPI.HttpAggregator/"]
COPY ["Shared/S2kDesignTemplate.Shared.csproj", "Shared/"]
COPY ["ApiExtensions/S2kDesignTemplate.ApiExtensions.csproj", "ApiExtensions/"]
RUN dotnet restore "ClientAPI.HttpAggregator/S2kDesignTemplate.ClientAPI.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/ClientAPI.HttpAggregator"
RUN dotnet build "S2kDesignTemplate.ClientAPI.HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "S2kDesignTemplate.ClientAPI.HttpAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S2kDesignTemplate.ClientAPI.HttpAggregator.dll"]