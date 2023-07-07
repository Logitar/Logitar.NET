# 5. Enable nullable references

Date: 2023-07-07

## Status

Accepted

## Context

We want to write more concise and straight-to-the-point code.

## Decision

We will enable nullable reference types and treat warnings as errors. See [official documentation](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references).

## Consequences

Users of our class libraries will need to ensure they use nullable reference types, or only send
null object references where our code allow it. We won't be responsible for thrown
`NullReferenceException` if an user pass a null object reference where it is not allowed. Our code
will be more concise by eliminating null checking, and will be more related to what it does, and not
how it does it.
