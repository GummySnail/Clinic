FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Documents.Api/Documents.Api.csproj", "Documents.Api/"]
COPY ["Documents.Core/Documents.Core.csproj", "Documents.Core/"]
COPY ["Documents.Infrastructure/Documents.Infrastructure.csproj", "Documents.Infrastructure/"]
RUN dotnet restore "Documents.Api/Documents.Api.csproj"
COPY . .
WORKDIR "/src/Documents.Api"
RUN dotnet build "Documents.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Documents.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Documents.Api.dll"]
