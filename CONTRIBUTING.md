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

The release PR updates `.release-please-manifest.json`, `version.txt`, `src/PhoneFormatter/PhoneFormatter.fsproj`, and `CHANGELOG.md`. Merge the PR to create a `vX.Y.Z` tag and a GitHub Release. The release workflow runs three jobs in order:

1. `release` runs the tests, checks the package and the nuget.org setup, and hands the package to the next jobs.
1. `nuget` publishes the package to [nuget.org](https://www.nuget.org/packages/PavelZaitsau.PhoneFormatter). The push sends the `.snupkg` to the nuget.org symbol server.
1. `github` publishes the package to [GitHub Packages](https://github.com/pavelzaitsau?tab=packages) with the workflow's `GITHUB_TOKEN`, and attaches the `.nupkg` and `.snupkg` to the GitHub Release.

`github` waits for `nuget`, so a nuget.org failure stops the release before GitHub Packages changes. The workflow runs one release at a time.

The nuget.org push uses [trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing). The `nuget` job exchanges its OIDC token for an API key that expires after 1 hour, so the repository stores no nuget.org secret. Only that job can request the OIDC token. The `release` job runs `dotnet restore` and `dotnet test`, which execute third-party package code, so it never holds the token. Users install from nuget.org because GitHub Packages requires a GitHub token even for a public package.

## Publish a release again

Package publication can fail after the GitHub Release exists. Run the **Release** workflow manually with that release tag. The workflow rebuilds the tagged commit and skips a package version that a registry already holds. **Re-run failed jobs** also works while the run's `packages` artifact exists, which is the repository's artifact retention period.

A tag up to `v2.3.1` goes to GitHub Packages and the GitHub Release only. Version 2.3.0 has no XML documentation and a high `FSharp.Core` lower bound, and the README in 2.3.1 asks users for a GitHub token. nuget.org cannot delete a version, so `NUGET_ORG_LAST_EXCLUDED_VERSION` in the release workflow keeps those versions off nuget.org.

## Repository settings

- Enable **Allow GitHub Actions to create and approve pull requests** in the repository Actions settings. Without it, Release Please cannot open the release PR.
- GitHub does not run PR checks on a PR that `GITHUB_TOKEN` created. Set the `RELEASE_PLEASE_TOKEN` repository secret to a personal access token with repository write access to get CI checks on release PRs.
- GitHub Packages creates a new package as private. Set its visibility to **Public** in the package settings after the first release. GitHub does not allow a public package to become private again.
- Create the `nuget` environment in the repository settings. Under **Deployment branches and tags**, choose **Selected branches and tags** and add the `master` branch only. Without the rule, a manual run from any branch, with an edited workflow, can publish to nuget.org. The `release` job checks the rule and stops the release when the environment or the rule is missing.
- On nuget.org, open **Trusted Publishing** from the account menu and add a policy for the GitHub repository `pavelzaitsau/PhoneFormatter`, the workflow file `release.yml`, and the environment `nuget`. Without the policy, the nuget.org login step fails and the package does not reach nuget.org.
- Set the `NUGET_USER` repository variable to the nuget.org profile name that owns the policy, not the account email. Without it, the `release` job stops before any registry changes.

## Version history

The `v2.2` tag points to a commit whose package metadata says `2.0.0`. Release automation starts from `2.2.0` and leaves historical tags unchanged.
