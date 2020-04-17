#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["src/CellCms.Api/CellCms.Api.csproj", "src/CellCms.Api/"]
RUN dotnet restore "src/CellCms.Api/CellCms.Api.csproj"
COPY . .
WORKDIR "/src/src/CellCms.Api"
RUN dotnet build "CellCms.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CellCms.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CellCms.Api.dll"]