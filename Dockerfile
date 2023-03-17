FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["identiry-ms/Auth.Api/Auth.Api.csproj", "identiry-ms/"]
RUN dotnet restore "identiry-ms/Auth.Api/Auth.Api.csproj"
COPY . .
WORKDIR "/src/identiry-ms"
RUN dotnet build "Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "identiry-ms.dll"]