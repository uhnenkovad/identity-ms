# Use the official .NET 7 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project file to the working directory
#COPY Auth.Api.csproj .

# Restore NuGet packages
#RUN dotnet restore

# Copy the remaining source code to the working directory
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Create a new image based on the .NET 7 runtime image
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS runtime

# Set the working directory to /app
WORKDIR /app

# Copy the built application from the build image
COPY --from=build /app/out ./

# Start the application
ENTRYPOINT ["dotnet", "Auth.Api.dll"]