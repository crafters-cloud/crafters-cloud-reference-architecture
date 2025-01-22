# Crafters Cloud Reference Architecture

![CI](https://github.com/crafters-cloud/crafters-cloud-reference-architecture/workflows/CI/badge.svg)
[![NuGet](https://img.shields.io/nuget/dt/CraftersCloud.ReferenceArchitecture.ProjectTemplates.svg)](https://www.nuget.org/packages/CraftersCloud.ReferenceArchitecture.ProjectTemplates)
[![NuGet](https://img.shields.io/nuget/vpre/CraftersCloud.ReferenceArchitecture.ProjectTemplates.svg)](https://www.nuget.org/packages/CraftersCloud.ReferenceArchitecture.ProjectTemplates)
[![MyGet (dev)](https://img.shields.io/myget/crafters-cloud/v/CraftersCloud.ReferenceArchitecture.ProjectTemplates.svg)](https://myget.org/gallery/crafters-cloud)

## Overview

This project provides a reference for building scalable and maintainable applications using C# and SQL (Sql Server). It
also includes project templates which can be used to create new solutions (_crafters-starter_ project template), and to
add new features (_crafters-feature_ project template).

The architecture leverages several modern technologies and best practices to ensure high performance, maintainability,
and ease of development.
It is based on the principles of Domain-Driven Design (DDD), CQRS, extreme programming (XP)
and [Vertical Slice Architecture](https://www.jimmybogard.com/vertical-slice-architecture/).
It is primarily designed to be flexible and extensible, allowing developers to quickly add new features covered by the
integration tests (scaffolding of features is supported).
The architecture does not give preference to
either [Onion Architecture](https://medium.com/@alessandro.traversi/understanding-onion-architecture-an-example-folder-structure-9c62208cc97d)
or the [Clean Architecture](https://celepbeyza.medium.com/introduction-to-clean-architecture-acf25ffe0310).
It can support both, but only **if** and **when** needed, by applying the necessary project refactorings.

### Domain (and entities)

### Testing Strategy

Following the idea of XP and YAGNI (You Ain't Gonna Need It), the focus is to first add integration tests that are
covering happy flow paths. These tests require minimum needed code, maintenance and setup.
The tests follow principles of black box testing of API endpoints. The black box testing has added benefit of allowing
implementation (and architecture) of the system to evolve without need to change the tests.
White box testing (using faking and mocking) is still possible, but it considered a code smell. White box testing makes
changing the code and evolving the architecture harder since both implementation and tests need to be changed in
parallel, making architecture evolution harder.
For verifying the results in the tests the two libraries [VerifyTests](https://github.com/VerifyTests/Verify)
and [Shouldly](https://docs.shouldly.org/) are used, since they provide a more readable way of verifying the
results.

### Key Technologies

- [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview?view=aspnetcore-9.0): A
  lightweight framework for building HTTP APIs with minimal overhead, allowing for quick and efficient development of
  RESTful services.
- [Autofac](https://autofac.org/) A popular IoC container for .NET that provides a simple and flexible way to manage
  dependencies in your application.
- [Entity Framework](https://learn.microsoft.com/en-us/ef/core/): An object-relational mapper (ORM) that enables .NET
  developers to work with a database using .NET objects, eliminating the need for most of the data-access code.
- [MediatR](https://github.com/jbogard/MediatR): A simple, unambitious mediator implementation in .NET, used to decouple
  the sending of requests from handling them, promoting a clean architecture.
- [Mapperly](https://mapperly.riok.app): A high-performance object-to-object mapper that helps in transforming objects
  between different layers of the application.
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/): A popular .NET library for building strongly-typed
  validation rules, ensuring that your data models are validated consistently and efficiently.
- [NUnit](https://nunit.org/): A popular unit testing framework for .NET that provides a simple and flexible way to
  write and run tests.

This combination of technologies provides a robust foundation for developing modern, high-quality applications.

## Usage

### Prerequisites

- .NET SDK 9
- SQL Server
- Docker

### Getting Started

### Install the template from NuGet.org

``dotnet new install CraftersCloud.ReferenceArchitecture.ProjectTemplates``

### Create a new solution based on the template

``dotnet new crafters-starter --projectName Client.Project --friendlyName "Client Project" --allow-scripts yes``

### Open the new solution in Visual Studio or Rider

Build the solution and run the integration tests to ensure everything is working correctly.

### Scaffold new feature in the new solution

Either execute the _scripts\new-feature.ps1_ script

or run the following command from command line (position yourself in the root folder of the solution):
``dotnet new crafters-feature --projectName Client.Project --featureName Order --featureNamePlural Orders --allow-scripts yes``
