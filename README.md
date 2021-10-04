# S2KDesignTemplate

## Getting Started

#### Projects 
| Image | Status | 
| ------------- | ------------- | 
| Blazor Web Client | [![Deploy to GitHub Pages](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/gh-pages.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/gh-pages.yml) |

#### Docker Images

| Image | Tag | Status | 
| ------------- | ------------- | ------------- | 
| Client API | [latest](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/pkgs/container/s2kdesigntemplate.clientapi) | [![Push Client API to Github Docker Registry](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/clientapi-docker-build.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/clientapi-docker-build.yml)|
| Client API HttpAggregator | [latest](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/pkgs/container/s2kdesigntemplate.clientapi.aggregator)  | [![Push Client API HTTP Aggregator to Github Docker Registry](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/clientapi-aggregator-docker-build.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/clientapi-aggregator-docker-build.yml) | 
| Server API | [latest](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/pkgs/container/s2kdesigntemplate.serverapi)  | [![Push Server API to Github Docker Registry](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/serverapi-docker-build.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/serverapi-docker-build.yml) |
| Web Status (HealthCheckUI) | [latest](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/pkgs/container/s2kdesigntemplate.webstatus)   |[![Push WebStatus (HealthCheckUI) to Github Docker Registry](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/webstatus-docker-build.yml/badge.svg)](https://github.com/s2kdesign-com/WASM_Net5_Azure_B2C_Template/actions/workflows/webstatus-docker-build.yml) |


#### Packages
All nuget/npm packages are published to [Github package registry ](https://github.com/orgs/s2kdesign-com/packages)

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
S2kDesignTemplate.ClientAPI.Gateway - https://localhost:44361/
S2kDesignTemplate.ClientAPI - https://localhost:44362/
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
Web Client API :  http://host.docker.internal:44362/
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

### Architecture Overview

#### Http Aggregators
* Client API Aggregator - Used from Landing Page and Mobile clients
* Server API Aggregator - Used from Hosted Services and Admin Dashboard

In a microservices architecture, the client apps usually need to consume functionality from more than one microservice. If that consumption is performed directly, the client needs to handle multiple calls to microservice endpoints. What happens when the application evolves and new microservices are introduced or existing microservices are updated? If your application has many microservices, handling so many endpoints from the client apps can be a nightmare. Since the client app would be coupled to those internal endpoints, evolving the microservices in the future can cause high impact for the client apps.

Therefore, having an intermediate level or tier of indirection (Gateway/Aggregator) can be convenient for microservice-based applications. If you don’t have API Gateways, the client apps must send requests directly to the microservices and that raises problems, such as the following issues

> The aggregator shows how a to make nested gRPC calls (a gRPC service calling another gRPC service). The gRPC client factory is used in ASP.NET Core to inject a client into services. The gRPC client factory is configured to propagate the context from the original call to the nested call.

#### gRPC 
gRPC is a modern, high-performance framework that evolves the age-old remote procedure call (RPC) protocol. At the application level, gRPC streamlines messaging between clients and back-end services.

gRPC uses HTTP/2 for its transport protocol. While compatible with HTTP 1.1, HTTP/2 features many advanced capabilities:

 * A binary framing protocol for data transport - unlike HTTP 1.1, which is text based.
 * Multiplexing support for sending multiple parallel requests over the same connection - HTTP 1.1 limits processing to one request/response message at a time.
 * Bidirectional full-duplex communication for sending both client requests and server responses simultaneously.
 * Built-in streaming enabling requests and responses to asynchronously stream large data sets.