namespace PavelZaitsau.PhoneFormatter

open System
open PhoneNumbers

type Formatter private () =
    static let util = PhoneNumberUtil.GetInstance()

    static member private ParseCore(phoneNumber: string, defaultRegion: string): IFormattedPhoneNumber =
        if isNull phoneNumber then
            nullArg "phoneNumber"

        if String.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length > 250 then
            raise (FormatException("Invalid phone number"))

        try
            let parsed = util.Parse(phoneNumber, defaultRegion)

            if not (util.IsValidNumber(parsed)) then
                raise (FormatException("Invalid phone number"))

            FormattedPhoneNumber(parsed) :> IFormattedPhoneNumber
        with :? NumberParseException as error ->
            raise (FormatException("Invalid phone number", error))

    static member Parse(phoneNumber: string): IFormattedPhoneNumber =
        Formatter.ParseCore(phoneNumber, null)

    static member Parse(phoneNumber: string, defaultRegion: string): IFormattedPhoneNumber =
        if isNull defaultRegion then
            nullArg "defaultRegion"

        let region = defaultRegion.ToUpperInvariant()

        if not (util.GetSupportedRegions().Contains(region)) then
            invalidArg "defaultRegion" "Unsupported region"

        Formatter.ParseCore(phoneNumber, region)
