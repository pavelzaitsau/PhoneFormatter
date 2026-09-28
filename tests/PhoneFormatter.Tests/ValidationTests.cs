using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelZaitsau.PhoneFormatter.Tests;

[TestClass]
public class ValidationTests
{
    [TestMethod]
    public void NullNumberNamesTheArgument()
    {
        var error = Assert.ThrowsExactly<ArgumentNullException>(() => Formatter.Parse(null!));

        Assert.AreEqual("phoneNumber", error.ParamName);
    }

    [TestMethod]
    public void NullRegionNamesTheArgument()
    {
        var error = Assert.ThrowsExactly<ArgumentNullException>(() => Formatter.Parse("4156667777", null!));

        Assert.AreEqual("defaultRegion", error.ParamName);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("1234")]
    [DataRow("+1 123")]
    public void InvalidNumberIsRejected(string input)
    {
        Assert.ThrowsExactly<FormatException>(() => Formatter.Parse(input));
    }

    [TestMethod]
    public void RegionCodeIsCaseInsensitive()
    {
        var number = Formatter.Parse("4156667777", "us");

        Assert.AreEqual("+14156667777", number.ToE164());
    }

    [TestMethod]
    public void OversizedInputIsRejected()
    {
        Assert.ThrowsExactly<FormatException>(() => Formatter.Parse(new string('1', 251), "US"));
    }

    [TestMethod]
    public void UnsupportedRegionIsRejected()
    {
        var error = Assert.ThrowsExactly<ArgumentException>(() => Formatter.Parse("4156667777", "ZZ"));

        Assert.AreEqual("defaultRegion", error.ParamName);
    }

    [TestMethod]
    [DataRow("5550123", "US")]
    [DataRow("4156667777", "GB")]
    public void ShortOrWrongRegionNationalNumberIsRejected(string input, string region)
    {
        Assert.ThrowsExactly<FormatException>(() => Formatter.Parse(input, region));
    }
}
