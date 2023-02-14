FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Appointments.Api/Appointments.Api.csproj", "Appointments.Api/"]
COPY ["Appointments.Core/Appointments.Core.csproj", "Appointments.Core/"]
COPY ["Appointments.Infrastructure/Appointments.Infrastructure.csproj", "Appointments.Infrastructure/"]
RUN dotnet restore "Appointments.Api/Appointments.Api.csproj"
COPY . .
WORKDIR "/src/Appointments.Api"
RUN dotnet build "Appointments.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Appointments.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointments.Api.dll"]
