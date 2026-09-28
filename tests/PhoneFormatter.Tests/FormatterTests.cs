using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelZaitsau.PhoneFormatter.Tests;

[TestClass]
public class FormatterTests
{
    [TestMethod]
    public void BelarusNumberExposesComponentsAndFormats()
    {
        var number = Formatter.Number(CountryPhoneCode.Belarus, "222", "284444");

        Assert.AreEqual(CountryPhoneCode.Belarus, number.CountryPhoneCode);
        Assert.AreEqual("222", number.Code);
        Assert.AreEqual("284444", number.PhoneNumber);
        Assert.AreEqual("+375222284444", number.ToE123());
        Assert.AreEqual("+375 222 284444", number.ToSpacedE123());
        Assert.AreEqual("8-0222-284444", number.ToNationalFormat());
        Assert.AreEqual(number.ToNationalFormat(), number.ToString());
    }

    [TestMethod]
    public void UnknownCountryCodeIsRejected()
    {
        Assert.ThrowsExactly<InvalidEnumArgumentException>(() =>
            Formatter.Number((CountryPhoneCode)1000, "222", "284444"));
    }

    [TestMethod]
    public void ToStringUsesNationalFormatAndReportsUnsupportedCountry()
    {
        var number = Formatter.Number(CountryPhoneCode.Japan, "42", "11234567");

        Assert.AreEqual("+814211234567", number.ToE123());
        Assert.ThrowsExactly<InvalidOperationException>(() => number.ToString());
    }
}
