# S2KDesignTemplate

Start template with preconfigured infrastructure
![](docs/img/InfrastructureDiagram.drawio.png) 

### Nuget Packages:
#### Cors Policies in nuget: "S2kDesignTemplate.ApiEtensions"
Cors Policies for APIs are configured in "startup.cs" file : 

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
      "0": { "PolicyName": "WebStatus", "Url": "https://localhost:44364", "Enabled": true }
      "1": { "PolicyName": "Admin", "Url": "https://localhost:44365", "Enabled": true }
    }
  },
```
#### Swagger UI for APIs is configured in 