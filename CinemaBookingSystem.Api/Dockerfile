#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 44334

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CinemaBookingSystem.Api/CinemaBookingSystem.Api.csproj", "CinemaBookingSystem.Api/"]
COPY ["CinemaBookingSystem.Common/CinemaBookingSystem.Common.csproj", "CinemaBookingSystem.Common/"]
COPY ["CinemaBookingSystem.Infrastructure/CinemaBookingSystem.Infrastructure.csproj", "CinemaBookingSystem.Infrastructure/"]
COPY ["CinemaBookingSystem.Application/CinemaBookingSystem.Application.csproj", "CinemaBookingSystem.Application/"]
COPY ["CinemaBookingSystem.Domain/CinemaBookingSystem.Domain.csproj", "CinemaBookingSystem.Domain/"]
COPY ["CinemaBookingSystem.Persistence/CinemaBookingSystem.Persistence.csproj", "CinemaBookingSystem.Persistence/"]
RUN dotnet restore "CinemaBookingSystem.Api/CinemaBookingSystem.Api.csproj"
COPY . .
WORKDIR "/src/CinemaBookingSystem.Api"
RUN dotnet build "CinemaBookingSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CinemaBookingSystem.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CinemaBookingSystem.Api.dll"]