import argparse
import json
import re
from pathlib import Path
from xml.etree import ElementTree
from zipfile import ZipFile

root = Path(__file__).resolve().parent.parent
parser = argparse.ArgumentParser()
parser.add_argument("--tag")
parser.add_argument("--package", type=Path)
arguments = parser.parse_args()

manifest = json.loads((root / ".release-please-manifest.json").read_text())["."]
version_file = (root / "version.txt").read_text().strip()
project = ElementTree.parse(root / "src/PhoneFormatter/PhoneFormatter.fsproj")
project_version = project.findtext("./PropertyGroup/Version")

if not re.fullmatch(r"(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)\.(?:0|[1-9]\d*)", manifest):
    raise SystemExit(f"Invalid SemVer version in manifest: {manifest}")

if len({manifest, version_file, project_version}) != 1:
    raise SystemExit(
        f"Version mismatch: manifest={manifest}, version.txt={version_file}, project={project_version}"
    )

if arguments.tag is not None and arguments.tag != f"v{manifest}":
    raise SystemExit(f"Tag mismatch: {arguments.tag} != v{manifest}")

if arguments.package is not None:
    with ZipFile(arguments.package) as archive:
        nuspec_name = next(name for name in archive.namelist() if name.endswith(".nuspec"))
        nuspec = ElementTree.fromstring(archive.read(nuspec_name))
        package_version = nuspec.findtext("{*}metadata/{*}version")

    if package_version != manifest:
        raise SystemExit(f"Package mismatch: {package_version} != {manifest}")

print(f"Version {manifest} is consistent")
