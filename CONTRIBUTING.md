# Contributing

This guide is for maintainers who build, test, and release `PavelZaitsau.PhoneFormatter`. Users of the package need only [README.md](README.md).

## Build and test

The solution targets .NET 10. Run the tests, build the package, and check its version and metadata:

```bash
dotnet test PhoneFormatter.slnx --configuration Release
dotnet pack src/PhoneFormatter/PhoneFormatter.fsproj --configuration Release --output artifacts
python3 scripts/check-version.py --package artifacts/PavelZaitsau.PhoneFormatter.*.nupkg
python3 scripts/check-package.py artifacts/PavelZaitsau.PhoneFormatter.*.nupkg
```

`check-version.py` confirms that the release manifest, `version.txt`, the project file, and the package carry one version. `check-package.py` confirms the license expression, release notes, `FSharp.Core` lower bound, and XML documentation for every public member. CI runs the same commands on every push and pull request.

## Release a version

The release workflow uses [Release Please](https://github.com/googleapis/release-please-action). It reads Conventional Commits on `master` and opens a release PR.

| Commit | Version change |
| --- | --- |
| `fix:` | Patch |
| `feat:` | Minor |
| Any type with `!` | Major |
| `test:`, `docs:`, `build:`, `ci:`, `chore:` | None by itself |

The release PR updates `.release-please-manifest.json`, `version.txt`, `src/PhoneFormatter/PhoneFormatter.fsproj`, and `CHANGELOG.md`. Merge the PR to create a `vX.Y.Z` tag and a GitHub Release. The release workflow then runs the tests, checks the package, publishes the `.nupkg` to [GitHub Packages](https://github.com/pavelzaitsau?tab=packages), and attaches the `.nupkg` and `.snupkg` to the GitHub Release. GitHub Packages accepts the workflow's `GITHUB_TOKEN`, so the push needs no package secret.

## Publish a release again

Package publication can fail after the GitHub Release exists. Run the **Release** workflow manually with that release tag. The workflow rebuilds the tagged commit and skips a package version that the registry already holds.

## Repository settings

- Enable **Allow GitHub Actions to create and approve pull requests** in the repository Actions settings. Without it, Release Please cannot open the release PR.
- GitHub does not run PR checks on a PR that `GITHUB_TOKEN` created. Set the `RELEASE_PLEASE_TOKEN` repository secret to a personal access token with repository write access to get CI checks on release PRs.
- GitHub Packages creates a new package as private. Set its visibility to **Public** in the package settings after the first release. GitHub does not allow a public package to become private again.

## Version history

The `v2.2` tag points to a commit whose package metadata says `2.0.0`. Release automation starts from `2.2.0` and leaves historical tags unchanged.
