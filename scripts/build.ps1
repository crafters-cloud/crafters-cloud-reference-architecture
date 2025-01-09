# import utils.ps1
. ./utils.ps1

exec { & dotnet clean -c Release ..\CraftersCloud.ReferenceArchitecture.sln }

exec { & dotnet build -c Release ..\CraftersCloud.ReferenceArchitecture.sln }

exec { & dotnet test -c Release ..\CraftersCloud.ReferenceArchitecture.sln --no-build -l trx --verbosity=normal }



