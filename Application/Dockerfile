FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8081
#COPY aspnetcore.pfx /https/aspnetcore.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.sln ./
COPY ["Model/Model.csproj", "Model/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Service/Service.csproj", "Service/"]
COPY ["Application/Application.csproj", "Application/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/Application"
RUN dotnet build "Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Application.dll"]
