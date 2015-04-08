namespace PavelZaitsau.PhoneFormatter

open System

type Formatter() =
    static member Number (countryPhoneCode: CountryPhoneCode, code: string, phoneNumber: string): IFormattedPhoneNumber =
        let length = String.length

        if phoneNumber.Length < 6 then
            raise (System.FormatException("Phone number contains less than 6 digits"))

        if phoneNumber.Length > 8 then
            raise (System.FormatException("Phone number contains more than 8 digits"))

        if code.Length < 2 then
            raise (System.FormatException("Code contains less than 2 digits"))

        if code.Length > 4 then
            raise (System.FormatException("Code contains more than 4 digits"))

        if not <| Enum.IsDefined(typeof<CountryPhoneCode>, countryPhoneCode) then
            raise (System.ComponentModel.InvalidEnumArgumentException("Illegal country phone code value"))

        new FormattedPhoneNumber(countryPhoneCode, code, phoneNumber) :> IFormattedPhoneNumber
