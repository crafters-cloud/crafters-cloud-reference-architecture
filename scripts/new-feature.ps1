Write-Host "Adding new feature"
$featureName = Read-Host "Feature name (e.g. 'Order' or 'exit' to quit)"

# Optionally, include any logic you want to perform with the user input
# For example, you can check if the input matches certain criteria
if ($featureName -eq "exit")
{
    Write-Host "Exiting the script."
    exit
}

$featureNamePlural = Read-Host "Feature name in plural (e.g. 'Orders' or 'exit' to quit)"
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

    Write-Host "New feature added successfully! To complete the process you need to:
        1. Add permissions for the read/write feature: ``$featureName`` in the ``PermissionId.cs``:
            ``${featureName}Read`` and ``${featureName}Write``
        2. Update ``MigrationSeeding.cs`` and seeding for the new feature:
            ``new ${featureName}StatusSeeding()``
        3. Update ``DbContextSeeding.cs`` and seeding for the new feature:
            ``new ${featureName}Seeding()``
        4. Update ``TestDatabase.cs`` and the following line to the list of tables to ignore (this is need to avoid deleting the seeded data from the table between tests):
            ``nameof(${featureName}Status)``
        5. Add new database migration (run ``add-migration.ps1`` script)
        6. Run the tests and adjust received Verify files to make sure everything is working as expected
        7. Enjoy your new feature!
"
}