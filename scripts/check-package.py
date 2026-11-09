import argparse
import re
from pathlib import PurePosixPath
from xml.etree import ElementTree
from zipfile import ZipFile

PACKAGE_ID = "PavelZaitsau.PhoneFormatter"
LICENSE = "BSD-3-Clause"
# Consumers cannot use an FSharp.Core older than this lower bound.
# A newer bound forces every F# consumer to upgrade FSharp.Core.
MAX_FSHARP_CORE_MAJOR = 8
DOCUMENTED_MEMBERS = [
    "T:PavelZaitsau.PhoneFormatter.Formatter",
    "M:PavelZaitsau.PhoneFormatter.Formatter.Parse(System.String)",
    "M:PavelZaitsau.PhoneFormatter.Formatter.Parse(System.String,System.String)",
    "T:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber",
    "M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToE164",
    "M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToE123",
    "M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToInternationalFormat",
    "M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToNationalFormat",
    "M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToRfc3966",
]

parser = argparse.ArgumentParser(description="Check NuGet package metadata and contents.")
parser.add_argument("package", help="Path to the .nupkg file")
arguments = parser.parse_args()

errors = []

with ZipFile(arguments.package) as archive:
    names = archive.namelist()
    nuspec = ElementTree.fromstring(archive.read(f"{PACKAGE_ID}.nuspec"))
    metadata = nuspec.find("{*}metadata")

    license_element = metadata.find("{*}license")
    if license_element is None or license_element.get("type") != "expression" or license_element.text != LICENSE:
        errors.append(f"License must be the SPDX expression {LICENSE}")

    readme = metadata.findtext("{*}readme")
    if readme is None or readme not in names:
        errors.append("Package must contain the README that the nuspec names")

    repository = metadata.find("{*}repository")
    if repository is None or not repository.get("commit"):
        errors.append("Repository metadata must name the source commit")

    if not metadata.findtext("{*}releaseNotes"):
        errors.append("Package must have release notes")

    dependencies = {
        dependency.get("id"): dependency.get("version")
        for dependency in metadata.findall(".//{*}dependency")
    }
    fsharp_core = dependencies.get("FSharp.Core")
    match = re.match(r"\[?(\d+)\.", fsharp_core or "")
    if match is None:
        errors.append("Package must depend on FSharp.Core")
    elif int(match.group(1)) > MAX_FSHARP_CORE_MAJOR:
        errors.append(f"FSharp.Core lower bound {fsharp_core} is above major version {MAX_FSHARP_CORE_MAJOR}")

    assemblies = [name for name in names if name.startswith("lib/") and name.endswith(f"{PACKAGE_ID}.dll")]
    if not assemblies:
        errors.append("Package contains no library assembly")

    for assembly in assemblies:
        documentation = str(PurePosixPath(assembly).with_suffix(".xml"))
        if documentation not in names:
            errors.append(f"{documentation} is missing")
            continue

        documented = {
            member.get("name")
            for member in ElementTree.fromstring(archive.read(documentation)).iter("member")
            if (member.findtext("summary") or "").strip()
        }
        for member in DOCUMENTED_MEMBERS:
            if member not in documented:
                errors.append(f"{documentation} has no summary for {member}")

if errors:
    raise SystemExit("Package check failed:\n" + "\n".join(f"- {error}" for error in errors))

print(f"Package {arguments.package} passed metadata checks")
