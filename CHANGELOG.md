# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- Changed YML extension to YAML.

### Removed

- Removed `ExceptionDetail` and dependency from `System.Text.Json`.

## [1.19.2] - 2024-11-19

### Fixed

- Upgraded NuGet.

## [1.19.1] - 2024-09-24

### Fixed

- `ColumnId` constructors.

## [1.19.0] - 2024-09-24

### Added

- Implemented column aliases.

## [1.18.0] - 2024-07-30

### Added

- `DateTimeExtensions`.

### Fixed

- Upgraded NuGet package.

## [1.17.0] - 2024-06-06

### Changed

- `Logitar` class library now targets .NET Standard 2.1.

### Fixed

- System usings.

## [1.16.0] - 2024-03-26

### Added

- Implemented a TwilioClient.

## [1.15.0] - 2024-03-03

### Added

- Implemented the Mask string extension method.

## [1.14.0] - 2024-02-25

### Added

- Implemented a MailgunClient.

### Fixed

- Password recovery message template.

## [1.13.1] - 2024-01-29

### Fixed

- Fixed documentation.
- Fixed GetErrorCode.

## [1.13.0] - 2024-01-21

### Added

- Added `GetBase64String` methods to `RandomStringGenerator`.
- Implemented cryptographically strong string generation from a list of characters.

### Changed

- Improved performance of `RandomStringGenerator.GetString` method.

## [1.12.2] - 2024-01-21

### Fixed

- DateTime claim creation again.

## [1.12.1] - 2024-01-20

### Fixed

- DateTime claim creation.

## [1.12.0] - 2024-01-19

### Added

- Implemented HTTP utils and API clients.
- Implemented a SendGridClient.

## [1.11.0] - 2024-01-10

### Fixed

- `Logitar.Security` NuGet package version.

### Removed

- Obsolete `purpose` claim.

## [1.10.0] - 2024-01-10

### Added

- Docker Compose file.

### Changed

- Upgraded to .NET8.
- Upgraded NuGet packages.
- Integration test configurations.

## [1.9.0] - 2024-01-10

### Added

- Implemented Truncate string method.

### Changed

- Updated LICENSE files to 2024.

## [1.8.0] - 2023-11-03

### Added

- Implemented ErrorMessageBuilder, ExceptionDetail, ExceptionExtensions and TypeExtensions.
- Added build.yml.

### Changed

- Moved Framework projects.

### Removed

- Removed Identity projects.
- Removed Demo project.
- Removed EventSourcing projects.

## [1.7.0] - 2023-09-21

### Added

- Implemented JOIN extensions.

### Changed

- Marked Purpose claim as obsolete.

### Fixed

- Renamed JOIN parameters.

## [1.6.0] - 2023-09-01

### Added

- Implemented a ClaimHelper.

## [1.5.0] - 2023-08-25

### Added

- Implemented user password change.
- Added claim constants.

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

[unreleased]: https://github.com/Logitar/Logitar.NET/compare/v1.19.2...HEAD
[1.19.2]: https://github.com/Logitar/Logitar.NET/compare/v1.19.1...v1.19.2
[1.19.1]: https://github.com/Logitar/Logitar.NET/compare/v1.19.0...v1.19.1
[1.19.0]: https://github.com/Logitar/Logitar.NET/compare/v1.18.0...v1.19.0
[1.18.0]: https://github.com/Logitar/Logitar.NET/compare/v1.17.0...v1.18.0
[1.17.0]: https://github.com/Logitar/Logitar.NET/compare/v1.16.0...v1.17.0
[1.16.0]: https://github.com/Logitar/Logitar.NET/compare/v1.15.0...v1.16.0
[1.15.0]: https://github.com/Logitar/Logitar.NET/compare/v1.14.0...v1.15.0
[1.14.0]: https://github.com/Logitar/Logitar.NET/compare/v1.13.1...v1.14.0
[1.13.1]: https://github.com/Logitar/Logitar.NET/compare/v1.13.0...v1.13.1
[1.13.0]: https://github.com/Logitar/Logitar.NET/compare/v1.12.2...v1.13.0
[1.12.2]: https://github.com/Logitar/Logitar.NET/compare/v1.12.1...v1.12.2
[1.12.1]: https://github.com/Logitar/Logitar.NET/compare/v1.12.0...v1.12.1
[1.12.0]: https://github.com/Logitar/Logitar.NET/compare/v1.11.0...v1.12.0
[1.11.0]: https://github.com/Logitar/Logitar.NET/compare/v1.10.0...v1.11.0
[1.10.0]: https://github.com/Logitar/Logitar.NET/compare/v1.9.0...v1.10.0
[1.9.0]: https://github.com/Logitar/Logitar.NET/compare/v1.8.0...v1.9.0
[1.8.0]: https://github.com/Logitar/Logitar.NET/compare/v1.7.0...v1.8.0
[1.7.0]: https://github.com/Logitar/Logitar.NET/compare/v1.6.0...v1.7.0
[1.6.0]: https://github.com/Logitar/Logitar.NET/compare/v1.5.0...v1.6.0
[1.5.0]: https://github.com/Logitar/Logitar.NET/compare/v1.4.0...v1.5.0
[1.4.0]: https://github.com/Logitar/Logitar.NET/compare/v1.3.0...v1.4.0
[1.3.0]: https://github.com/Logitar/Logitar.NET/compare/v1.2.0...v1.3.0
[1.2.0]: https://github.com/Logitar/Logitar.NET/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/Logitar/Logitar.NET/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/Logitar/Logitar.NET/releases/tag/v1.0.0
