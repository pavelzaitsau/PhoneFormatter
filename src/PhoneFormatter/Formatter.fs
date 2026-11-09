namespace PavelZaitsau.PhoneFormatter

open System
open PhoneNumbers

/// <summary>Parses phone number text into a validated <see cref="T:PavelZaitsau.PhoneFormatter.IFormattedPhoneNumber"/>.</summary>
/// <remarks>
/// Validation checks the number against the numbering patterns of <c>libphonenumber-csharp</c>.
/// A valid pattern does not prove that the number is assigned or reachable.
/// The members are safe to call from several threads.
/// </remarks>
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

    /// <summary>Parses a number that starts with a country calling code, such as <c>+1 415 666 7777</c>.</summary>
    /// <param name="phoneNumber">
    /// An international number with a leading <c>+</c>, or a <c>tel:</c> URI.
    /// The text holds at most 250 characters and can include an extension.
    /// </param>
    /// <returns>The parsed number. The method never returns <c>null</c>.</returns>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="phoneNumber"/> is <c>null</c>.</exception>
    /// <exception cref="T:System.FormatException">
    /// The text is blank, exceeds 250 characters, cannot be parsed, has no country calling code,
    /// or does not match a valid pattern.
    /// </exception>
    static member Parse(phoneNumber: string): IFormattedPhoneNumber =
        Formatter.ParseCore(phoneNumber, null)

    /// <summary>Parses a national or international number, such as <c>(415) 666-7777</c> for region <c>US</c>.</summary>
    /// <param name="phoneNumber">
    /// A national number, an international number with a leading <c>+</c>, or a <c>tel:</c> URI.
    /// The text holds at most 250 characters and can include an extension.
    /// </param>
    /// <param name="defaultRegion">
    /// An ISO 3166-1 two-letter region code in any letter case, such as <c>US</c> or <c>gb</c>.
    /// The method uses the region only when <paramref name="phoneNumber"/> has no country calling code.
    /// </param>
    /// <returns>The parsed number. The method never returns <c>null</c>.</returns>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="phoneNumber"/> or <paramref name="defaultRegion"/> is <c>null</c>.</exception>
    /// <exception cref="T:System.ArgumentException"><paramref name="defaultRegion"/> is not a supported region.</exception>
    /// <exception cref="T:System.FormatException">
    /// The text is blank, exceeds 250 characters, cannot be parsed, or does not match a valid pattern.
    /// </exception>
    static member Parse(phoneNumber: string, defaultRegion: string): IFormattedPhoneNumber =
        if isNull defaultRegion then
            nullArg "defaultRegion"

        let region = defaultRegion.ToUpperInvariant()

        if not (util.GetSupportedRegions().Contains(region)) then
            invalidArg "defaultRegion" "Unsupported region"

        Formatter.ParseCore(phoneNumber, region)
