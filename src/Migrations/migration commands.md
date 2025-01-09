# Entity framework migration commands

## Command line commands

### Install global tool

``` shell
dotnet tool install --global dotnet-ef
```

### Update global tool to latest version

``` shell
dotnet tool update --global dotnet-ef
```

### Add migration

``` shell
dotnet-ef migrations add MIGRATION_NAME_HERE --project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations --startup-project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations --context AppDbContext
```

### Update database

``` shell
dotnet-ef database update --project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations --startup-project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations --context AppDbContext
```

### Remove migration

``` shell
dotnet-ef migration remove --project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations --startup-project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations
```

### Revert to a specific migration (discard all migrations created after the specified one)

``` shell
dotnet-ef database update --project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations --startup-project CraftersCloud.Core.ReferenceArchitecture.Data.Migrations THE-LAST-GOOD-MIGRATION-NAME
```
