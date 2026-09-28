namespace PavelZaitsau.PhoneFormatter

type IFormattedPhoneNumber =
    abstract member ToE164 : unit -> string
    abstract member ToE123 : unit -> string
    abstract member ToInternationalFormat : unit -> string
    abstract member ToNationalFormat : unit -> string
    abstract member ToRfc3966 : unit -> string
