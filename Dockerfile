FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the entire solution directory (all 5 projects)
COPY services/ProsperaServices/ .

# Restore all projects via solution file
RUN dotnet restore ProsperaServices.slnx

# Publish the main API project
RUN dotnet publish ProsperaServices/ProsperaServices.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProsperaServices.dll"]
