# Bước 1: Sử dụng SDK Image .NET 9.0 để build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy file solution và các file .csproj vào trước
COPY WebCoffee.sln ./
COPY src/WebCoffee.BackendServer/WebCoffee.BackendServer.csproj ./src/WebCoffee.BackendServer/
COPY src/WebCoffee.ViewModels/WebCoffee.ViewModels.csproj ./src/WebCoffee.ViewModels/

# Restore các gói NuGet
RUN dotnet restore

# Copy toàn bộ mã nguồn và build
COPY . ./
WORKDIR /app/src/WebCoffee.BackendServer
RUN dotnet publish -c Release -o /app/publish

# Bước 2: Sử dụng Runtime Image .NET 9.0 để chạy
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
ENTRYPOINT ["dotnet", "WebCoffee.BackendServer.dll"]