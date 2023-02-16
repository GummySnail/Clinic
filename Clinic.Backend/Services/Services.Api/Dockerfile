FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services.Api/Services.Api.csproj", "Services.Api/"]
COPY ["Services.Infrastructure/Services.Infrastructure.csproj", "Services.Infrastructure/"]
COPY ["Services.Core/Services.Core.csproj", "Services.Core/"]
RUN dotnet restore "Services.Api/Services.Api.csproj"
COPY . .
WORKDIR "/src/Services.Api"
RUN dotnet build "Services.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.Api.dll"]
