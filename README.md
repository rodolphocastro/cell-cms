# Cell CMS

| Branch | Status | Descrição |
| ------ | ------ | --------- |
| Master | ![Build and Test](https://github.com/rodolphocastro/cell-cms/workflows/Build%20and%20Test/badge.svg?branch=master)| Ciclo estável, recomendado para produção |
| Develop | ![Build and Test](https://github.com/rodolphocastro/cell-cms/workflows/Build%20and%20Test/badge.svg?branch=develop)| Ciclo em desenvolvimento, recomendado para entusiastas |

**Cell CMS** é um content management system que visa ser:

+ Leve
+ Auto Contido (self-contained)
+ Prático de Utilizar

Nosso foco é em disponibilizar um CMS que desenvolvedores possam facilmente referenciar em seus aplicativos, sites e sistemas.

## 📚 Instruções

### Utilizando uma Versão publicada

> WIP, iremos suportar imagens Docker e executáveis

### Compilando

Você **precisará ter instalado** em seu ambiente o **SDK 5.0.101 do Dotnet**.

Uma vez configurado basta executar `dotnet build .\cell-cms.sln` na raiz do repositório.

### Testando

Execute `dotnet test .\cell-cms.sln` na raiz do repositório.

Caso queira capturar informações de cobertura de testes utilize:

`dotnet test --no-restore --collect:"XPlat Code Coverage" .\cell-cms.sln`

## ⚙ Configurações

### Autenticação/Autorização

O CellCMS utiliza o **Azure Active Directory** como *provider* de identidade, então você terá de configurar sua instância do **AAD** conforme explicado [neste post](https://dev.to/ardc_overflow/cell-cms-autenticando-o-admin-270b).

As seguintes variáveis de ambiente devem ser utilizadas:

| Nome | Explicação |
| ---- | ---------- |
| AzureAd__MetadataEndpoint | URL para o `metadata` do AAD |
| AzureAd__AuthorizeEndpoint | URL para o endpoint `authorize` do AAD |
| AzureAd__TokenEndpoint | URL para o endpoint `token` do AAD | 
| AzureAd__ClientId | `clientId` para identificar a aplicação com o AAD |

### Armazenamento

O CellCMS **utiliza um banco de dados SQLite**, portanto ele é armazenado próximo ao executável em um formato `.db`.

As seguintes variáveis podem ser utilizadas para controlar como o aplicativo lida com o banco:

| Nome | Explicação | Padrão |
| ---- | ---------- | ------ |
| ConnectionStrings__CellCmsContext | Caminho para acessar o banco de dados | `Data Source=cellCmsStorage.db;` |
| MigrateOnStartup | True/False indicando se devemos atualizar o banco de dados ao iniciar a API | `false` |

## 📰 Posts

Cada etapa de desenvolvimento do CellCMS teve um post escrito em meu blog pessoal. 

Os posts podem ser facilmente encontrados [Nesta série de Posts do dev.to](https://dev.to/ardc_overflow/series/7469).

## 🤝 Agradecimentos

> W.I.P.
