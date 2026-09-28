namespace PavelZaitsau.PhoneFormatter

open System
open System.Collections.Generic

type internal FormattedPhoneNumber(countryPhoneCode, code, phoneNumber) =
    let nationnalTransformers = dict [
        (CountryPhoneCode.Belarus, fun (countryPhoneCode: CountryPhoneCode) (code: string) (phoneNumber: string) -> String.Format("8-0{0}-{1}", code, phoneNumber))]

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
            match nationnalTransformers.TryGetValue(countryPhoneCode) with
                | true, x -> x countryPhoneCode code phoneNumber
                | false, _ -> raise (InvalidOperationException("Unknown country code"))
            end

    override this.ToString() =
        (this :> IFormattedPhoneNumber).ToNationalFormat()
