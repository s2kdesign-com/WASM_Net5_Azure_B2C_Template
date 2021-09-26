## Defined methods in nuget
---
#### Cors Policies
Nuget: "[S2kDesignTemplate.ApiEtensions](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/packages/1008690)"

Cors Policies for APIs are configured in `startup.cs` file : 

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

Add configuration to `appsettings.json` ->
```json
  "CorsPoliciesConfiguration": {
    "CorsPolicies": {
      "0": { "PolicyName": "WebStatus", "Url": "https://localhost:44364", "Enabled": true }
      "1": { "PolicyName": "Admin", "Url": "https://localhost:44365", "Enabled": true }
    }
  },
```
---
#### Swagger UI 
Nuget: "[S2kDesignTemplate.ApiEtensions](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/packages/1008690)"

Swagger UI for APIs is configured in `startup.cs` file: 
```c# 
public void ConfigureServices(IServiceCollection services){
    ...
    services.AddControllers();
    services.AddSwaggerExtensions(Configuration.GetSection("SwaggerConfiguration"));
    ...
}
```
```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env){   
    app.UseSwaggerExtensions(Configuration.GetSection("SwaggerUIOptions"));
    ...
}
```
Add configuration to `appsettings.json` ->
```json
  "SwaggerConfiguration": {
    "Title" :  "Server API" ,
    "OpenApiOAuthFlow": {
      "AuthorizationUrl": "https://xxx.b2clogin.com/xxx.onmicrosoft.com/xxx/oauth2/v2.0/authorize",
      "TokenUrl": "https://xxx.b2clogin.com/xxx.onmicrosoft.com/oauth2/v2.0/token",
      "Scopes": {
        "ServerAPI.Read": "https://xxx.onmicrosoft.com/xxx/xxx"
      }
    }
  },
  "SwaggerUIOptions": {
    "OAuthConfigObject": {
      "ClientId": "xxx",
      "AppName": "S2KDesignTemplate-SwaggerUI"
    }
```
---
#### Health Checks
Nuget: "[S2kDesignTemplate.ApiEtensions](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/packages/1008690)"
HealthChecks for APIs is configured in `startup.cs` file: 
```c# 
public void ConfigureServices(IServiceCollection services){
    services.AddHealthChecksExtensions(Configuration.GetSection("HealthChecksConfiguration"));
    ...
}
```
```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env){   
    ...
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHealthChecksExtensions();
    });
}
```

If one project is referencing another, Add configuration to `appsettings.json` to master project ->
```json
  "HealthChecksConfiguration": {
    "UrlGroup": {
      "0": {
        "Url": "https://localhost-dependant-project:44363/hc",
        "Name": "server-api-check",
        "Tags": [
          "server-api"
        ]
      }
    }
  },
```