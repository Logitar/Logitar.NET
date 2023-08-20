# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Implemented user password change.

### Changed

- Created a Locale value object.

## [1.4.0] - 2023-08-13

### Added

- Implemented a RandomStringGenerator.
- Added an AggregateId argument in Identity aggregate constructors.

## [1.3.0] - 2023-08-13

### Added

- Created a RoleAggregate.
- Created an UserAggregate.
- Created a SessionAggregate.

### Changed

- Protected setters on aggregate metadata.

## [1.2.0] - 2023-08-06

### Added

- Implemented query JOINs.
- Added AggregateRoot metadata.
- Created a struct for actor identifiers.
- Implemented an EventSourcing MongoDB store.
- Added a demo TodoController with CRUD endpoints.
- Implemented DeleteBuilders and UpdateBuilders for PostgreSQL and Microsoft SQL Server.

### Changed

- Reorganized the solution directory structure.
- Upgraded NuGet packages and fixed EventSourcing project dependencies.
- Replaced DeleteAction by a nullable boolean.
- Refactored AggregateRoot and DomainEvent.
- Injecting IEventSerializer as a dependency.

## [1.1.0] - 2023-07-11

### Added

- Implemented CollectionExtensions.
- Implemented InsertBuilders and QueryBuilders for PostgreSQL and Microsoft SQL Server.
- Implemented a Demo API with Swagger.
- Implemented Event Sourcing with multiple data stores.

## [1.0.0] - 2023-07-07

### Added

- Implemented StringExtensions.

[unreleased]: https://github.com/Logitar/Logitar.NET/compare/v1.4.0...HEAD
[1.4.0]: https://github.com/Logitar/Logitar.NET/releases/tag/v1.3.0...v1.4.0
[1.3.0]: https://github.com/Logitar/Logitar.NET/releases/tag/v1.2.0...v1.3.0
[1.2.0]: https://github.com/Logitar/Logitar.NET/releases/tag/v1.1.0...v1.2.0
[1.1.0]: https://github.com/Logitar/Logitar.NET/releases/tag/v1.0.0...v1.1.0
[1.0.0]: https://github.com/Logitar/Logitar.NET/releases/tag/v1.0.0
