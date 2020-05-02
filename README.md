# Cynosura.Studio
![dev-build-status](https://dev.azure.com/CynosuraPlatform/Cynosura.Studio/_apis/build/status/Cynosura.Studio?branchName=development)

Cynosura.Studio is a part of entity based projects [Cynosura](https://github.com/CynosuraPlatform).

## Getting Started

1. Start backend Cynosura.Studio.Web
2. Start frontend Cynosura.Studio.Web.Ng
3. In browser create new solution in Solutions section

### Prerequisites

* dot net core 2.2 SDK
* Angular CLI 7
* MS SQL Server (you can use docker-compose if haven't)

### Templates

Cynosura.Studio supports custom templates for solution generation. You can set it in appsettings.json in Templates section. By default we put there Cynosura.Template.

If you want to create your own template then start by forking [Cynosura.Template](https://github.com/CynosuraPlatform/Cynosura.Template). Your template must be published in some nuget feed to use in Cynosura.Studio.

### NuGet Feeds

Cynosura.Studio supports private NuGet feeds in appsettings with keys Nuget/FeedUrl, and Nuget/Username and Nuget/Password.

For Template development, you can set local folder feed LocalFeed/SourcePath.

### Entities and Enums

Cynosura.Studio is entity based code-generator and entity manager. For entities cynosura generates all necessary code for REST API, EF DataSets, Angular interface. For customization Cynosura supports Entity properties behavior.

Entity and enum information stores in Core project/Metadata.

### Solution upgrade

Upgrade feature let's you upgrade your solution to the latest template version. All your changes and new template version changes merges in upgrade process.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md]).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

* Google's Diff Match Patch used for merging generated and user code in some cases can fails. We recommend to commit workspace before any changes by Cynosura.Studio.