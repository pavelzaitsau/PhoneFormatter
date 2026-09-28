# PhoneFormatter

PhoneFormatter parses and formats phone numbers from many regions in .NET applications.

## Use it

Reference `src/PhoneFormatter/PhoneFormatter.fsproj` from a .NET 10 project. This C# example formats a US number:

```csharp
using PavelZaitsau.PhoneFormatter;

var number = Formatter.Parse("+1 415 666 7777");
Console.WriteLine(number.ToE164());               // +14156667777
Console.WriteLine(number.ToE123());               // +1 415 666 7777
Console.WriteLine(number.ToInternationalFormat()); // +1 415-666-7777
Console.WriteLine(number.ToNationalFormat());      // (415) 666-7777
Console.WriteLine(number.ToRfc3966());             // tel:+1-415-666-7777
```

Pass a two-letter region for a national number: `Formatter.Parse("(415) 666-7777", "US")`. Run `dotnet test PhoneFormatter.slnx` to check the project.

## What it does not do

- A valid pattern does not prove a number is assigned, active, or reachable.
- `ToE164()` omits an extension. Use `ToRfc3966()` when an extension must remain in the output.
- Output formatting does not tell callers how to dial from their current location.

## Configuration

| Input | Default | Effect |
| --- | --- | --- |
| `phoneNumber` | Required | Accepts international, national, and `tel:` forms up to 250 characters |
| `defaultRegion` | Not needed for numbers beginning with `+` | Supplies an ISO two-letter region for national input |

The library uses region rules from `libphonenumber-csharp` 9.0.40. Update that package to receive newer numbering metadata.

## Troubleshooting

| Symptom | Cause | Fix |
| --- | --- | --- |
| `FormatException` | The input cannot be parsed or does not match a valid regional pattern | Check the number and provide its region for national input |
| `ArgumentException` | The supplied region is unsupported | Pass a supported ISO two-letter region |
| `ArgumentNullException` | A required argument is `null` | Pass a number and, for the two-argument overload, a region |

## Releases

The release workflow uses [Release Please](https://github.com/googleapis/release-please-action). It reads Conventional Commits on `master` and opens a release PR. A `fix:` commit bumps the patch version, a `feat:` commit bumps the minor version, and a breaking commit marked with `!` bumps the major version. Test, documentation, and build commits do not create a release by themselves.

The release PR updates `.release-please-manifest.json`, `version.txt`, `src/PhoneFormatter/PhoneFormatter.fsproj`, and `CHANGELOG.md`. Review and merge the PR to create a `vX.Y.Z` tag and GitHub Release. The release workflow runs tests, checks the package version, publishes the `.nupkg` to [GitHub Packages](https://nuget.pkg.github.com/pavelzaitsau/index.json), and attaches the `.nupkg` and `.snupkg` to the GitHub Release. GitHub Packages uses the workflow's `GITHUB_TOKEN`; no package secret is needed.

If package publication fails after the GitHub Release exists, run the **Release** workflow manually with that release tag. The workflow rebuilds the tagged commit and skips an existing package version. GitHub Packages creates the first package as private. After the first release, set its visibility to **Public** in the package settings. GitHub does not allow a public package to become private again. NuGet clients authenticate to this registry even for public packages. Follow [GitHub's NuGet registry instructions](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry) to add the source and install `PavelZaitsau.PhoneFormatter`.

Enable **Allow GitHub Actions to create and approve pull requests** in repository Actions settings. By default, Release Please uses `GITHUB_TOKEN`; GitHub does not trigger normal PR checks for PRs created with that token. The release workflow runs tests before creating a release. Set the `RELEASE_PLEASE_TOKEN` repository secret to a personal access token with repository write access when release PRs need automatic CI checks.

The existing `v2.2` tag points to a commit whose package metadata says `2.0.0`. Automation starts from `2.2.0` and leaves historical tags unchanged.

## More

Read [AGENTS.md](https://github.com/pavelzaitsau/PhoneFormatter/blob/master/AGENTS.md) before changing the library. See [libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp) for metadata and parsing behavior.
