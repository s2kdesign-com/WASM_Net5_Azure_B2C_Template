#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClientAPI/S2kDesignTemplate.ClientAPI.csproj", "ClientAPI/"]
COPY ["Shared/S2kDesignTemplate.Shared.csproj", "Shared/"]
COPY ["ApiExtensions/S2kDesignTemplate.ApiExtensions.csproj", "ApiExtensions/"]
RUN dotnet restore "ClientAPI/S2kDesignTemplate.ClientAPI.csproj"
COPY . .
WORKDIR "/src/ClientAPI"
RUN dotnet build "S2kDesignTemplate.ClientAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "S2kDesignTemplate.ClientAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S2kDesignTemplate.ClientAPI.dll"]