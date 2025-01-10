## Install the template locally from source (run in solution root)

``dotnet new install .\``

## Build the nuget package (run in solution root)

``dotnet pack templates/Solution/Solution.csproj -c Release -o ./artifacts ``

## Install the template locally from nupkg file

``dotnet new install "artifacts\CraftersCloud.ReferenceArchitecture.SolutionTemplate.*.nupkg"``

## Install the template from NuGet.org

``dotnet new install CraftersCloud.ReferenceArchitecture.ProjectTemplates``
If you are not authenticated automatically, add the --interactive argument.

## Install specific version of the template from NuGet.org

``dotnet new install CraftersCloud.Core.ReferenceArchitecture.ProjectTemplates::VERSION``

where VERSION should be replaced with the specific version you want to install, e.g. 2.0.1

## Deploy a new solution based on the template:

``dotnet new crafters-solution --projectName Client.Project --friendlyName "Client Project" --allow-scripts yes``
``dotnet new crafters-feature --projectName Client.Project --featureName Order --featureNamePlural Orders --allow-scripts yes``

## Uninstall the template (when installed from nupkg)

``dotnet new uninstall CraftersCloud.ReferenceArchitecture.ProjectTemplates``

## Uninstall the template (when installed from local source)

``dotnet new uninstall .\``
