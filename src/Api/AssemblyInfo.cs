using Microsoft.AspNetCore.Mvc;
using Riok.Mapperly.Abstractions;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
[assembly: ApiController]
[assembly: MapperDefaults(RequiredMappingStrategy = RequiredMappingStrategy.Target)]