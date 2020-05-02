# Cynosura.Studio
![dev-build-status](https://dev.azure.com/CynosuraPlatform/Cynosura.Studio/_apis/build/status/Cynosura.Studio?branchName=development)

## Overview

Cynosura is a code generator that allows to quickly create projects with .NET and Angular.

Main features:
1. Backend is written in .NET Core 3.1
2. For API there are two options: JSON API or gRPC
3. For data storage Entity Framework Core is used
4. Business logic is implemented with [MediatR](https://github.com/jbogard/MediatR)
5. [FluentValudation](https://fluentvalidation.net/) is used for data validation
6. [AutoMapper](https://automapper.org) is used for data mapping
7. ASP.NET Core Identity and IdentityServer4 is used for user management and authentication 
8. Frontend is written in Angular 8
9. [Angular Material](https://material.angular.io) UI component library is used
10. Cynosura.Studio application is generated in Cynosura.Studio

## Getting Started

1. Start backend Cynosura.Studio.Web (`dotnet run`)
2. Start frontend Cynosura.Studio.Web.Ng (`npm run start`)
3. Open Cynosura.Studio at http://localhost:4300/
4. Create new solution in Solutions section and your new project is ready
5. Add your entity in Entities section

### Prerequisites

* .NET Core SDK 3.1
* Git (used for merging files when updating metadata or upgrading templates)

### Templates

Cynosura.Studio creates projects from default template [Cynosura.Template](https://github.com/CynosuraPlatform/Cynosura.Template). However you can use custom templates. 

If you want to create your own template then start by forking Cynosura.Template. Your template must be published in some nuget feed to use in Cynosura.Studio. To use custom template configure it in appsettings.json in Templates section.

### NuGet Feeds

Cynosura.Studio supports private NuGet feeds in appsettings with keys Nuget/FeedUrl, and Nuget/Username and Nuget/Password.

For Template development, you can set local folder feed LocalFeed/SourcePath.

### Entities and Enums

Cynosura.Studio is entity based code-generator and entity manager. For entities cynosura generates all necessary code for CQRS, EF, API, Angular web UI. For customization Cynosura supports Entity properties behavior.

Entity and enum information stored in Core project/Metadata.

### Solution upgrade

Upgrade feature let's you upgrade your solution to the latest template version. All your changes and new template version changes merges in upgrade process.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md).

## Roadmap

Features to be implemented:

1. Allow to have a choice for frontend implementation
2. Add Blazor WebAssembly frontend

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.