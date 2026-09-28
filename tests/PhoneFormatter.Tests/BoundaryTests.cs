using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelZaitsau.PhoneFormatter.Tests;

[TestClass]
public class BoundaryTests
{
    [TestMethod]
    [DataRow("12", "123456")]
    [DataRow("1234", "12345678")]
    public void AcceptedComponentLengthsPreserveInput(string code, string phoneNumber)
    {
        var number = Formatter.Number(CountryPhoneCode.Belarus, code, phoneNumber);

        Assert.AreEqual(code, number.Code);
        Assert.AreEqual(phoneNumber, number.PhoneNumber);
        Assert.AreEqual($"+375{code}{phoneNumber}", number.ToE123());
    }

    [TestMethod]
    [DataRow("1", "123456")]
    [DataRow("12345", "123456")]
    [DataRow("12", "12345")]
    [DataRow("12", "123456789")]
    public void RejectedComponentLengthsThrowFormatException(string code, string phoneNumber)
    {
        Assert.ThrowsExactly<FormatException>(() =>
            Formatter.Number(CountryPhoneCode.Belarus, code, phoneNumber));
    }

    [TestMethod]
    [DataRow(CountryPhoneCode.Japan, "+8142123456", "+81 42 123456")]
    [DataRow(CountryPhoneCode.RussiaKazakhstan, "+742123456", "+7 42 123456")]
    public void SupportedCountryCodesProduceInternationalFormat(CountryPhoneCode country, string compact, string spaced)
    {
        var number = Formatter.Number(country, "42", "123456");

        Assert.AreEqual(compact, number.ToE123());
        Assert.AreEqual(spaced, number.ToSpacedE123());
        Assert.ThrowsExactly<InvalidOperationException>(() => number.ToNationalFormat());
    }

    [TestMethod]
    [DataRow("02", "012345", "+37502012345", "8-002-012345")]
    public void FormatterPreservesComponentText(string code, string phoneNumber, string international, string national)
    {
        var number = Formatter.Number(CountryPhoneCode.Belarus, code, phoneNumber);

        Assert.AreEqual(code, number.Code);
        Assert.AreEqual(phoneNumber, number.PhoneNumber);
        Assert.AreEqual(international, number.ToE123());
        Assert.AreEqual(national, number.ToNationalFormat());
    }

    [TestMethod]
    [DataRow("AB", "123456")]
    [DataRow("12", "12-456")]
    [DataRow("١٢", "123456")]
    [DataRow("12", "１２３４５６")]
    public void NonAsciiOrNonDigitComponentsThrowFormatException(string code, string phoneNumber)
    {
        Assert.ThrowsExactly<FormatException>(() =>
            Formatter.Number(CountryPhoneCode.Belarus, code, phoneNumber));
    }

    [TestMethod]
    public void NullComponentsIdentifyTheArgument()
    {
        var codeError = Assert.ThrowsExactly<ArgumentNullException>(() =>
            Formatter.Number(CountryPhoneCode.Belarus, null!, "123456"));
        var numberError = Assert.ThrowsExactly<ArgumentNullException>(() =>
            Formatter.Number(CountryPhoneCode.Belarus, "12", null!));

        Assert.AreEqual("code", codeError.ParamName);
        Assert.AreEqual("phoneNumber", numberError.ParamName);
    }
}
