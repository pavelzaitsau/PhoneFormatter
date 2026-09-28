using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelZaitsau.PhoneFormatter.Tests;

[TestClass]
public class FormatterTests
{
    [TestMethod]
    public void InternationalNumberHasStandardOutputFormats()
    {
        var number = Formatter.Parse("+1 415 666 7777");

        Assert.AreEqual("+14156667777", number.ToE164());
        Assert.AreEqual("+1 415 666 7777", number.ToE123());
        Assert.AreEqual("+1 415-666-7777", number.ToInternationalFormat());
        Assert.AreEqual("(415) 666-7777", number.ToNationalFormat());
        Assert.AreEqual("tel:+1-415-666-7777", number.ToRfc3966());
        Assert.AreEqual(number.ToInternationalFormat(), number.ToString());
    }

    [TestMethod]
    public void NationalInputUsesRegionToFindCountryCallingCode()
    {
        var number = Formatter.Parse("(415) 666-7777", "US");

        Assert.AreEqual("+14156667777", number.ToE164());
    }

    [TestMethod]
    public void ExtensionAppearsInRfc3966ButNotE164()
    {
        var number = Formatter.Parse("+1 415 666 7777 ext. 123");

        Assert.AreEqual("+14156667777", number.ToE164());
        Assert.AreEqual("tel:+1-415-666-7777;ext=123", number.ToRfc3966());
    }

    [TestMethod]
    public void ParsesTelephoneUriWithExtension()
    {
        var number = Formatter.Parse("tel:+1-415-666-7777;ext=123");

        Assert.AreEqual("+14156667777", number.ToE164());
        Assert.AreEqual("tel:+1-415-666-7777;ext=123", number.ToRfc3966());
    }

    [TestMethod]
    [DataRow("+441174960123")]
    [DataRow("+4930901820")]
    [DataRow("+81312345678")]
    [DataRow("+33123456789")]
    [DataRow("+375291234567")]
    [DataRow("+74951234567")]
    [DataRow("+77172123456")]
    public void FormatsValidNumbersFromDifferentRegions(string input)
    {
        var number = Formatter.Parse(input);

        Assert.AreEqual(input, number.ToE164());
        Assert.IsTrue(Regex.IsMatch(number.ToE123(), @"^\+[0-9 ]+$"));
        StringAssert.StartsWith(number.ToRfc3966(), "tel:+");
    }

    [TestMethod]
    [DataRow("+1 415 666 7777 ext. 123", "US")]
    [DataRow("(415) 666-7777 x123", "US")]
    [DataRow("tel:+1-415-666-7777;ext=123", "GB")]
    public void ExtensionSurvivesRfc3966RoundTrip(string input, string region)
    {
        var original = Formatter.Parse(input, region);
        var reparsed = Formatter.Parse(original.ToRfc3966());

        Assert.AreEqual("+14156667777", reparsed.ToE164());
        Assert.AreEqual("tel:+1-415-666-7777;ext=123", reparsed.ToRfc3966());
    }

    [TestMethod]
    [DataRow("+14156667777", "US")]
    [DataRow("+442079460018", "GB")]
    [DataRow("+4930901820", "DE")]
    [DataRow("+81312345678", "JP")]
    [DataRow("+33123456789", "FR")]
    [DataRow("+375291234567", "BY")]
    [DataRow("+74951234567", "RU")]
    public void FormattedOutputCanBeParsedAgain(string input, string region)
    {
        var original = Formatter.Parse(input);

        foreach (var formatted in new[]
        {
            original.ToE164(),
            original.ToE123(),
            original.ToInternationalFormat(),
            original.ToRfc3966(),
        })
        {
            Assert.AreEqual(input, Formatter.Parse(formatted).ToE164(), formatted);
        }

        Assert.AreEqual(input, Formatter.Parse(original.ToNationalFormat(), region).ToE164());
    }

    [TestMethod]
    public void ExplicitCountryCallingCodeTakesPrecedenceOverDefaultRegion()
    {
        var number = Formatter.Parse("+1 415 666 7777", "GB");

        Assert.AreEqual("+14156667777", number.ToE164());
        Assert.AreEqual("(415) 666-7777", number.ToNationalFormat());
    }
}
