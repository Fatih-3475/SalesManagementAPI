FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SalesManagementAPI/SalesManagementAPI.WebAPI.csproj SalesManagementAPI/
COPY Business/SalesManagementAPI.Business.csproj Business/
COPY Core/SalesManagementAPI.Core.csproj Core/
COPY DataAccess/SalesManagementAPI.DataAccess.csproj DataAccess/

RUN dotnet restore SalesManagementAPI/SalesManagementAPI.WebAPI.csproj

COPY . .

RUN dotnet publish SalesManagementAPI/SalesManagementAPI.WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SalesManagementAPI.WebAPI.dll"]