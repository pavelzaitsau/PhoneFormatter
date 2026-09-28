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

## More

Read [AGENTS.md](https://github.com/pavelzaitsau/PhoneFormatter/blob/master/AGENTS.md) before changing the library. See [libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp) for metadata and parsing behavior.
