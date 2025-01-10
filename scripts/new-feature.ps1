Write-Host "Adding new feature"
$featureName = Read-Host "Feature name (e.g. 'Product' or 'exit' to quit)"

# Optionally, include any logic you want to perform with the user input
# For example, you can check if the input matches certain criteria
if ($featureName -eq "exit")
{
    Write-Host "Exiting the script."
    exit
}

$featureNamePlural = Read-Host "Feature name in plural (e.g. 'Products' or 'exit' to quit)"
if ($featureNamePlural -eq "exit")
{
    Write-Host "Exiting the script."
    exit
}
else
{
    # Navigate to the root of the solution
    Set-Location ..
    dotnet new crafters-feature --projectName "CraftersCloud.ReferenceArchitecture" --featureName $featureName --featureNamePlural $featureNamePlural --allow-scripts yes
}