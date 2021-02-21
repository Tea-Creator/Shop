#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Catalog/Catalog.WebApi/Catalog.WebApi.csproj", "src/Catalog/Catalog.WebApi/"]
COPY ["src/Catalog/Catalog.Application/Catalog.Application.csproj", "src/Catalog/Catalog.Application/"]
COPY ["src/Catalog/Catalog.Domain/Catalog.Domain.csproj", "src/Catalog/Catalog.Domain/"]
COPY ["src/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "src/Catalog/Catalog.Infrastructure/"]
RUN dotnet restore "src/Catalog/Catalog.WebApi/Catalog.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Catalog/Catalog.WebApi"
RUN dotnet build "Catalog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.WebApi.dll"]
