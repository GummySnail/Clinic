FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Profiles.Api/Profiles.Api.csproj", "Profiles.Api/"]
COPY ["Profiles.Core/Profiles.Core.csproj", "Profiles.Core/"]
COPY ["Profiles.Infrastructure/Profiles.Infrastructure.csproj", "Profiles.Infrastructure/"]
RUN dotnet restore "Profiles.Api/Profiles.Api.csproj"
COPY . .
WORKDIR "/src/Profiles.Api"
RUN dotnet build "Profiles.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Profiles.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profiles.Api.dll"]
