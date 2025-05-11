# Stage 1: Build
FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
WORKDIR /app/EasyTourChoice.API
RUN dotnet publish -c Release -o /app/publish -r linux-arm -p:PublishReadyToRun=true

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy build output
COPY --from=build /app/publish .

# Configure volumes and ports
VOLUME /app/data
EXPOSE 8080

# Start both services
ENTRYPOINT ["dotnet", "EasyTourChoiceBackend.dll"]
