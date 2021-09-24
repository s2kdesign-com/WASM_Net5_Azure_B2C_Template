#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Server/S2kDesignTemplate.Server.csproj", "Server/"]
COPY ["LandingPage/S2kDesignTemplate.LandingPage.csproj", "LandingPage/"]
COPY ["Shared/S2kDesignTemplate.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/S2kDesignTemplate.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "S2kDesignTemplate.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "S2kDesignTemplate.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S2kDesignTemplate.Server.dll"]
