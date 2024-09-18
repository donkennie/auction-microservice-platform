# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files from the correct paths
COPY src/AuctionRoomService/AuctionRoomService.csproj src/AuctionRoomService/
COPY src/KernelShared/KernelShared.csproj src/KernelShared/

# Restore dependencies
RUN dotnet restore "src/AuctionRoomService/AuctionRoomService.csproj"

# Copy the entire source directory
COPY src/ ./

# Build the application
WORKDIR "/src/AuctionRoomService"
RUN dotnet build "AuctionRoomService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuctionRoomService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage - prepare the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionRoomService.dll"]
