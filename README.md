# PhoneFormatter

PhoneFormatter formats phone numbers for .NET applications using country codes for Belarus, Japan, and Russia or Kazakhstan.

## Use it

Install the .NET 10 SDK, then run the tests:

```bash
dotnet test PhoneFormatter.slnx
```

Reference `src/PhoneFormatter/PhoneFormatter.fsproj` from a .NET project. This C# example produces international and Belarusian national formats:

```csharp
var number = Formatter.Number(CountryPhoneCode.Belarus, "222", "284444");
Console.WriteLine(number.ToE123());           // +375222284444
Console.WriteLine(number.ToSpacedE123());     // +375 222 284444
Console.WriteLine(number.ToNationalFormat()); // 8-0222-284444
```

Add `using PavelZaitsau.PhoneFormatter;` to a C# file that uses the example.

## What it does not do

- The formatter does not parse or normalize a complete phone number.
- The formatter checks component lengths but does not check whether each character is a digit.
- National formatting supports Belarus only. Other supported country codes throw `InvalidOperationException` for that format.

## Configuration

| Setting | Default | Effect |
| --- | --- | --- |
| Country code | Required | Selects the international prefix and available national format |
| Area code | Required, 2–4 characters | Appears after the country prefix |
| Phone number | Required, 6–8 characters | Appears after the area code |

## Troubleshooting

| Symptom | Cause | Fix |
| --- | --- | --- |
| `FormatException` | A component is outside its accepted length | Pass a 2–4 character area code and a 6–8 character phone number |
| `InvalidOperationException` | No national formatter exists for the chosen country | Use `ToE123()` or `ToSpacedE123()` |
| `InvalidEnumArgumentException` | The country code is not a declared enum value | Use a member of `CountryPhoneCode` |

## More

Read [AGENTS.md](AGENTS.md) before changing the library or its tests.
