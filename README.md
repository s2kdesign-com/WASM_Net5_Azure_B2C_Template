# S2KDesignTemplate

Start template with preconfigured infrastructure
![](docs/img/InfrastructureDiagram.drawio.png) 

#### Nuget Packages:
### Cors Policies in "S2kDesignTemplate.ApiEtensions"
Cors Policies for APIs are configured  : 

```c# 
public void ConfigureServices(IServiceCollection services){
    ...
    services.AddCorsExtensions(Configuration.GetSection(nameof(CorsPoliciesConfiguration)).Get<CorsPoliciesConfiguration>());
}
```
```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
    ...
    app.UseCorsExtensions();
}
```
Add configuration to appsettings.json ->
```json
  "CorsPoliciesConfiguration": {
    "CorsPolicies": {
      "0": {
        "PolicyName": "WebStatus",
        "Url": "https://localhost:44364",
        "Enabled": true
      }
    }
  },
```