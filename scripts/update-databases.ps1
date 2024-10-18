Write-Host "Updating App db"

dotnet-ef database update --project ../src/Migrations --startup-project ../src/Migrations --context AppDbContext