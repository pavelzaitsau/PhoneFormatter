namespace PavelZaitsau.PhoneFormatter

open System

type Formatter() =
    static member Number (countryPhoneCode: CountryPhoneCode, code: string, phoneNumber: string): IFormattedPhoneNumber =
        if isNull code then
            nullArg "code"

        if isNull phoneNumber then
            nullArg "phoneNumber"

        if phoneNumber.Length < 6 then
            raise (System.FormatException("Phone number contains less than 6 digits"))

        if phoneNumber.Length > 8 then
            raise (System.FormatException("Phone number contains more than 8 digits"))

        if code.Length < 2 then
            raise (System.FormatException("Code contains less than 2 digits"))

        if code.Length > 4 then
            raise (System.FormatException("Code contains more than 4 digits"))

        let asciiDigitsOnly (value: string) =
            value |> Seq.forall (fun character -> character >= '0' && character <= '9')

        if not (asciiDigitsOnly code) then
            raise (FormatException("Code must contain only ASCII digits"))

        if not (asciiDigitsOnly phoneNumber) then
            raise (FormatException("Phone number must contain only ASCII digits"))

        if not <| Enum.IsDefined(typeof<CountryPhoneCode>, countryPhoneCode) then
            raise (System.ComponentModel.InvalidEnumArgumentException("Illegal country phone code value"))

        new FormattedPhoneNumber(countryPhoneCode, code, phoneNumber) :> IFormattedPhoneNumber
