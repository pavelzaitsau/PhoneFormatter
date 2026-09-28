# Agent guide

This repository contains an F# library and a C# MSTest project. The solution targets .NET 10.

## Work in this repository

- Change library code in `src/PhoneFormatter` and tests in `tests/PhoneFormatter.Tests`.
- Preserve public behavior unless the task asks for an API or behavior change.
- Add a focused test before changing behavior. Cover both the expected result and relevant error path.
- Run `dotnet test PhoneFormatter.slnx` after code changes.
- Keep generated `bin`, `obj`, and `TestResults` files out of Git.

## Public behavior

`Formatter.Number` accepts only ASCII digits and checks component lengths and declared country codes. It rejects null components with `ArgumentNullException`. National formatting exists for Belarus only. `ToString()` uses the national format for Belarus and the spaced international format for other supported countries. See [README.md](README.md) for output examples.
