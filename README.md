# S2KDesignTemplate

## Getting Started


| Image | Status | 
| ------------- | ------------- | 
| Blazor Web Client | [![Deploy to GitHub Pages](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/gh-pages.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/gh-pages.yml) |

| Nuget | Status | url| 
| ------------- | ------------- | ------------- | 
| S2kDesignTemplate.ApiEtensions | [![Publish Nuget Package - S2kDesignTemplate.ApiExtensions](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/nuget-s2kdesigntemplate-extensions.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/nuget-s2kdesigntemplate-extensions.yml) |[Nuget](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/)

##### Start with Visual Studio and [Docker](https://docs.docker.com/docker-for-windows/install/)
Clone the project and open  `"src/S2kDesignTemplate.sln"`. 
Right click on `"docker-compose"` and choose `"Set as startup"` then press F5.

##### Start with Visual Studio Without [Docker](https://docs.docker.com/docker-for-windows/install/)
Start the projects in this order with right click `"Set as startup"` :
```
S2kDesignTemplate.WebStatus - https://localhost:5107/
S2kDesignTemplate.ServerAPI - https://localhost:44363/
S2kDesignTemplate.Server - https://localhost:44314/
```

##### Start with Command line
Make sure you have [installed](https://docs.docker.com/docker-for-windows/install/) and [configured](https://github.com/dotnet-architecture/eShopOnContainers/wiki/Windows-setup#configure-docker) docker in your environment. After that, you can run the below commands from the **/src/** directory and get started with the `S2kDesignTemplate` immediately.

```powershell
docker-compose build
docker-compose up
```

You should be able to browse different components of the application by using the below URLs :

```
Web Status (HealthCheck Monitoring) : http://host.docker.internal:5107/

Web Client :  http://host.docker.internal:44314/
Web Client API :  TODO
Web Client Gateway :  TODO

Web Server :  http://host.docker.internal:44314/
Web Server API:  http://host.docker.internal:44363/
Web Server Gateway :  TODO

Web Files API: TOO
Web Files Gateway: TODO
```
>Note: If you are running this application in macOS then use `docker.for.mac.localhost` as DNS name in `.env` file and the above URLs instead of `host.docker.internal`.
---



Start template with preconfigured infrastructure
![](docs/img/InfrastructureDiagram.drawio.png) 

