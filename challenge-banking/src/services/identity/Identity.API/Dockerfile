#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/services/identity/Identity.API/Identity.API.csproj", "src/services/identity/Identity.API/"]
COPY ["src/services/identity/Identity.Service/Identity.Service.csproj", "src/services/identity/Identity.Service/"]
COPY ["src/services/identity/Identity.Data/Identity.Data.csproj", "src/services/identity/Identity.Data/"]
COPY ["src/services/identity/Identity.Domain/Identity.Domain.csproj", "src/services/identity/Identity.Domain/"]
COPY ["src/services/Service.Common/Service.Common.csproj", "src/services/Service.Common/"]
RUN dotnet restore "src/services/identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/src/services/identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
