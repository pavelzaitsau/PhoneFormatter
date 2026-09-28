# Agent guide

This repository contains an F# library and a C# MSTest project. The solution targets .NET 10.

## Work in this repository

- Change library code in `src/PhoneFormatter` and tests in `tests/PhoneFormatter.Tests`.
- Preserve public behavior unless the task asks for an API or behavior change.
- Add a focused test before changing behavior. Cover both the expected result and relevant error path.
- Run `dotnet test PhoneFormatter.slnx` after code changes.
- Keep generated `bin`, `obj`, and `TestResults` files out of Git.

## Public behavior

`Formatter.Number` checks component lengths and declared country codes. National formatting exists for Belarus only. See [README.md](README.md) for supported output and limitations.
