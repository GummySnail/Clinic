FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Offices.Api/Offices.Api.csproj", "Offices.Api/"]
COPY ["Offices.Core/Offices.Core.csproj", "Offices.Core/"]
COPY ["Offices.Infrastructure/Offices.Infrastructure.csproj", "Offices.Infrastructure/"]
RUN dotnet restore "Offices.Api/Offices.Api.csproj"
COPY . .
WORKDIR "/src/Offices.Api"
RUN dotnet build "Offices.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Offices.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Offices.Api.dll"]
