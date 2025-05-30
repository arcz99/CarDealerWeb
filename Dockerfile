FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./CarDealerWeb.csproj"
RUN dotnet build "./CarDealerWeb.csproj" -c Release -o /app/build
RUN dotnet publish "./CarDealerWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CarDealerWeb.dll"]