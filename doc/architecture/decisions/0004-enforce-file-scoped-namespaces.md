# 4. Enforce file scoped namespaces

Date: 2023-07-07

## Status

Accepted

## Context

We want to write code that is easier to read and maintain.

## Decision

We will enforce file scoped namespaces. See [official documentation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-10.0/file-scoped-namespaces).

## Consequences

We won't be able to have multiple classes with different namespaces in the same file, but every line
in the files will have one less indentation.
