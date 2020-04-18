
# 1: Utilizando a imagem Alpine com o .NET Runtime 3.1
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
# 2: Navegue/crie a pasta /app
WORKDIR /app
# 3: Exponha a porta 80 (http)
EXPOSE 80
# 4: Exponha a porta 443 (https)
EXPOSE 443

# 5: Utilizando a imagem Alpine com o .NET SDK 3.1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
# 6: Navegue/crie a pasta /src
WORKDIR /src
# 7: Copie o .csproj da API (host) para o diretório (da imagem) src/CellCms.Api/
COPY ["src/CellCms.Api/CellCms.Api.csproj", "src/CellCms.Api/"]
# 8: Restaure as dependências (NuGet)
RUN dotnet restore "src/CellCms.Api/CellCms.Api.csproj"
# 9: Copie o resto dos arquivos da pasta (host) para a imagem
COPY . .
# 10: Navegue á pasta do projeto API, na imagem
WORKDIR "/src/src/CellCms.Api"
# 11: Compile o projeto, jogando os resultados na pasta /app/build
RUN dotnet build "CellCms.Api.csproj" -c Release -o /app/build

# 12: Utilizando a imagem de build mas chamando-a de publish
FROM build AS publish
# 13: Execute o publish do projeto, jogando os resultados na pasta /app/publish
RUN dotnet publish "CellCms.Api.csproj" -c Release -o /app/publish

# 14: Utilizando a imagem base mas chamando-a de final
FROM base AS final
# 15: Navegue/crie a pasta /app
WORKDIR /app
# 16: Copie a pasta /app/publish da imagem publish para a pasta atual do final
COPY --from=publish /app/publish .
# 17: Deixe registrado que, ao executar a imagem, o comando 'dotnet' deve ser chamado com o parâmetro 'CellCms.Api.dll"
ENTRYPOINT ["dotnet", "CellCms.Api.dll"]