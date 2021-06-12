# 1: Utilizando a imagem Buster (Slim) com o .NET Runtime 5
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
# 2: Navegue/crie a pasta /app
WORKDIR /app
# 3: Exponha a porta 80 (http)
EXPOSE 80
# 4: Exponha a porta 443 (https)
EXPOSE 443

# 5: Utilizando a imagem Buster (Slim) com o .NET SDK 5
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
# 6: Navegue/crie a pasta /src
WORKDIR /src
# 7: Copie o .csproj da API (host) para o diret�rio (da imagem) src/CellCms.Api/
COPY ["src/CellCms.Api/CellCms.Api.csproj", "CellCms.Api/"]
# 8: Restaure as depend�ncias (NuGet)
RUN dotnet restore "CellCms.Api/CellCms.Api.csproj"
# 9: Copie o resto dos arquivos da pasta (host) para a imagem
COPY ["src/", "."]

# 10: Navegue/crie a pasta /tests
WORKDIR /tests
# 11: Copie o .csproj dos testes para a imagem
COPY ["tests/CellCms.Tests.Unit/CellCms.Tests.Unit.csproj", "CellCms.Tests.Unit/"]
# 12: Restaure as dependências (NuGet)
RUN dotnet restore "CellCms.Tests.Unit/CellCms.Tests.Unit.csproj"
# 13: Copie o resto
COPY ["tests/", "."]
# 14: Execute os testes
RUN dotnet test "CellCms.Tests.Unit/CellCms.Tests.Unit.csproj" --collect:"XPlat Code Coverage"
# TODO: Copiar code coverage para fora da imagem

# 15: Navegue � pasta do projeto API, na imagem
WORKDIR "/src/CellCms.Api"
# 16: Compile o projeto, jogando os resultados na pasta /app/build
RUN dotnet build "CellCms.Api.csproj" -c Release -o /app/build

# 17: Utilizando a imagem de build mas chamando-a de publish
FROM build AS publish
# 18: Execute o publish do projeto, jogando os resultados na pasta /app/publish
RUN dotnet publish "CellCms.Api.csproj" -c Release -o /app/publish

# 19: Utilizando a imagem base mas chamando-a de final
FROM base AS final
# 20: Navegue/crie a pasta /app
WORKDIR /app
# 21: Copie a pasta /app/publish da imagem publish para a pasta atual do final
COPY --from=publish /app/publish .
# 22: Deixe registrado que, ao executar a imagem, o comando 'dotnet' deve ser chamado com o par�metro 'CellCms.Api.dll"
ENTRYPOINT ["dotnet", "CellCms.Api.dll"]