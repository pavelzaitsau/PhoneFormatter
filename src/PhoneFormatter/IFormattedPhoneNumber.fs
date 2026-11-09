namespace PavelZaitsau.PhoneFormatter

/// <summary>A phone number that passed validation and formats itself as text.</summary>
/// <remarks>
/// Instances are immutable and safe to use from several threads.
/// <see cref="M:System.Object.ToString"/> returns the same text as <see cref="M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToInternationalFormat"/>.
/// </remarks>
type IFormattedPhoneNumber =
    /// <summary>Formats the number in E.164 form, for example <c>+14156667777</c>.</summary>
    /// <remarks>The result omits any extension. Use <see cref="M:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber.ToRfc3966"/> to keep it.</remarks>
    abstract member ToE164 : unit -> string

    /// <summary>Formats the number in E.123 international form, for example <c>+1 415 666 7777</c>.</summary>
    /// <remarks>Spaces separate the digit groups. The result keeps an extension as <c>ext.</c> text.</remarks>
    abstract member ToE123 : unit -> string

    /// <summary>Formats the number with the region's international grouping, for example <c>+1 415-666-7777</c>.</summary>
    /// <remarks>The separators come from the numbering metadata and differ between regions.</remarks>
    abstract member ToInternationalFormat : unit -> string

    /// <summary>Formats the number as a caller inside its region dials it, for example <c>(415) 666-7777</c>.</summary>
    /// <remarks>The result has no country calling code, so it identifies the number only within its region.</remarks>
    abstract member ToNationalFormat : unit -> string

    /// <summary>Formats the number as an RFC 3966 <c>tel:</c> URI, for example <c>tel:+1-415-666-7777;ext=123</c>.</summary>
    /// <remarks>The result keeps any extension as an <c>ext</c> parameter.</remarks>
    abstract member ToRfc3966 : unit -> string
