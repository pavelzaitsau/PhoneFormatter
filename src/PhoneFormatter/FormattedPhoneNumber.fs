namespace PavelZaitsau.PhoneFormatter

open System.Text.RegularExpressions
open PhoneNumbers

type internal FormattedPhoneNumber(number: PhoneNumber) =
    let util = PhoneNumberUtil.GetInstance()

    interface IFormattedPhoneNumber with
        member _.ToE164() = util.Format(number, PhoneNumberFormat.E164)

        member _.ToE123() =
            let international = util.Format(number, PhoneNumberFormat.INTERNATIONAL)
            Regex.Replace(international.Replace('-', ' '), @"\s+", " ").Trim()

        member _.ToInternationalFormat() = util.Format(number, PhoneNumberFormat.INTERNATIONAL)
        member _.ToNationalFormat() = util.Format(number, PhoneNumberFormat.NATIONAL)
        member _.ToRfc3966() = util.Format(number, PhoneNumberFormat.RFC3966)

    override _.ToString() = util.Format(number, PhoneNumberFormat.INTERNATIONAL)
