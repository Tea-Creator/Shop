#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Basket/Basket.WebApi/Basket.WebApi.csproj", "src/Basket/Basket.WebApi/"]
COPY ["src/Basket/Basket.Application/Basket.Application.csproj", "src/Basket/Basket.Application/"]
COPY ["src/Shared/EventBus/EventBus.csproj", "src/Shared/EventBus/"]
COPY ["src/Basket/Basket.Domain/Basket.Domain.csproj", "src/Basket/Basket.Domain/"]
COPY ["src/Basket/Basket.Infrastructure/Basket.Infrastructure.csproj", "src/Basket/Basket.Infrastructure/"]
RUN dotnet restore "src/Basket/Basket.WebApi/Basket.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Basket/Basket.WebApi"
RUN dotnet build "Basket.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.WebApi.dll"]