FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/services/exchange/Exchange.API/Exchange.API.csproj", "src/services/exchange/Exchange.API/"]
COPY ["src/services/exchange/Exchange.Service/Exchange.Service.csproj", "src/services/exchange/Exchange.Service/"]
COPY ["src/services/Service.Common/Service.Common.csproj", "src/services/Service.Common/"]
COPY ["src/services/exchange/Exchange.Domain/Exchange.Domain.csproj", "src/services/exchange/Exchange.Domain/"]
COPY ["src/services/exchange/Exchange.Data/Exchange.Data.csproj", "src/services/exchange/Exchange.Data/"]
RUN dotnet restore "src/services/exchange/Exchange.API/Exchange.API.csproj"
COPY . .
WORKDIR "/src/src/services/exchange/Exchange.API"
RUN dotnet build "Exchange.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchange.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.API.dll"]
