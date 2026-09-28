namespace PavelZaitsau.PhoneFormatter

open System

type internal FormattedPhoneNumber(countryPhoneCode, code, phoneNumber) =
    interface IFormattedPhoneNumber with
        member this.PhoneNumber with get() = phoneNumber
        member this.Code with get() = code
        member this.CountryPhoneCode with get() = countryPhoneCode

        member this.ToE123() =
            let cCode = (int countryPhoneCode).ToString("+#")
            String.concat "" [cCode; code; phoneNumber]

        member this.ToSpacedE123() =
            let cCode = (int countryPhoneCode).ToString("+#")
            String.concat " " [cCode; code; phoneNumber]

        member this.ToNationalFormat() =
            match countryPhoneCode with
            | CountryPhoneCode.Belarus -> String.Format("8-0{0}-{1}", code, phoneNumber)
            | _ -> raise (InvalidOperationException("Unknown country code"))

    override this.ToString() =
        (this :> IFormattedPhoneNumber).ToNationalFormat()
