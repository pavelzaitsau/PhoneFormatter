# Agent guide

This repository contains an F# phone-number library and a C# MSTest project. The solution targets .NET 10.

## Work in this repository

- Change the public API in `src/PhoneFormatter` and tests in `tests/PhoneFormatter.Tests`.
- Use `libphonenumber-csharp` for regional parsing, validation, and grouping rules. Avoid hand-written country rules.
- Add a failing test before changing behavior. Test valid, invalid, and extension input when relevant.
- Run `dotnet test PhoneFormatter.slnx --configuration Release` after code changes.
- Run `dotnet pack src/PhoneFormatter/PhoneFormatter.fsproj --configuration Release` after package changes.
- Keep generated `bin`, `obj`, `TestResults`, and `artifacts` files out of Git.

## Public behavior

`Formatter.Parse` accepts an international number or a national number with a region. It rejects numbers that do not match the metadata's valid patterns. The output methods provide E.164, E.123, metadata-specific international and national, and RFC 3966 forms. `ToE164()` omits extensions; `ToRfc3966()` preserves them. See [README.md](README.md) for examples and limitations.
