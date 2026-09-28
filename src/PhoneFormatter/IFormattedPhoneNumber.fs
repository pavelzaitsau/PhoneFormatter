namespace PavelZaitsau.PhoneFormatter

type IFormattedPhoneNumber =
   abstract member PhoneNumber : string with get
   abstract member Code : string with get
   abstract member CountryPhoneCode : CountryPhoneCode with get

   abstract member ToE123 : unit -> string
   abstract member ToSpacedE123 : unit -> string
   abstract member ToNationalFormat : unit -> string
