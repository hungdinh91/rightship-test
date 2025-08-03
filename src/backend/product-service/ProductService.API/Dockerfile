#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/product-service/ProductService.API/ProductService.API.csproj", "src/backend/product-service/ProductService.API/"]
COPY ["src/backend/product-service/ProductService.Application/ProductService.Application.csproj", "src/backend/product-service/ProductService.Application/"]
COPY ["src/backend/shared-kernel/OrderService.Application.Shared/OrderSystem.Application.Shared.csproj", "src/backend/shared-kernel/OrderService.Application.Shared/"]
COPY ["src/backend/shared-kernel/OrderSystem.Domain.Shared/OrderSystem.Domain.Shared.csproj", "src/backend/shared-kernel/OrderSystem.Domain.Shared/"]
COPY ["src/backend/shared-kernel/OrderSystem.Shared/OrderSystem.Shared.csproj", "src/backend/shared-kernel/OrderSystem.Shared/"]
COPY ["src/backend/product-service/ProductService.Domain/ProductService.Domain.csproj", "src/backend/product-service/ProductService.Domain/"]
COPY ["src/backend/product-service/ProductService.Infrastructure/ProductService.Infrastructure.csproj", "src/backend/product-service/ProductService.Infrastructure/"]
COPY ["src/backend/shared-kernel/OrderSystem.Infrastructure.Shared/OrderSystem.Infrastructure.Shared.csproj", "src/backend/shared-kernel/OrderSystem.Infrastructure.Shared/"]
RUN dotnet restore "./src/backend/product-service/ProductService.API/ProductService.API.csproj"
COPY . .
WORKDIR "/src/src/backend/product-service/ProductService.API"
RUN dotnet build "./ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "ProductService.API.dll"]