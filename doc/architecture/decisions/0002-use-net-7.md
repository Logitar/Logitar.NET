# 2. Use .NET 7

Date: 2023-07-07

## Status

Accepted

## Context

We are implementing a set of class libraries. We need to use the most production-ready and recent
.NET version.

## Decision

We will be using .NET 7. See [official documentation](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0#net-5-and-net-standard).

## Consequences

Our set of class libraries won't be compatible with older systems, such as .NET6- and .NET
Framework applications.
