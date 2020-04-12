# Cell CMS - Changelog

## [Em Desenvolvimento]

### [Em Desenvolvimento] - Adicionado

+ Geração do **Swagger.Json** para documentação da API
+ Swagger UI
+ Autenticação através do **Azure Active Directory**
+ **Persistência** dos Models:
  + Feed
  + Content
  + Tag
+ Validação dos payloads através do **FluentValidation**
+ `.editorconfig` para reforçar *code style*
+ Logs estruturados através do **Serilog**
+ Monitoramento através do **ApplicationInsights**
+ **HealthChecks** para expor status da API

### [Em Desenvolvimento] - Alterado

+ `feature/refactor-swagger-config`: Refatora para que as configurações do Swagger fiquem separadas do `Startup.cs`
+ `feature/refactor-auth-config`: Refatora para que a configuração de Autenticação fique separado do `Startup.cs`
+ `feature/refactor-context`: Refatora para que a configuração de Persistência fique separado do `Startup.cs`
+ `feature/migrate-on-run`: Permite que a database seja automaticamente migrada durante a inicialização
+ `feature/auto-mapper`: Permite o mapeamento automático entre models, commands e queries
+ `feature/serilog-appsettings`: Permite a configuração do Serilog através do `appsettings.json`

### [Em Desenvolvimento] - Corrigido

### [Em Desenvolvimento] - Descontinuado

<!-- Links para as versões -->
[Em Desenvolvimento]:https://github.com/rodolphocastro/cell-cms/tree/develop