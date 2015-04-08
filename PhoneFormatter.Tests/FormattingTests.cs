using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelZaitsau.PhoneFormatter.Tests
{
    [TestClass]
    public class FormattingTests
    {
        #region E.123 format tests

        [TestMethod]
        public void E123FormatTest1()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            Assert.IsNotNull(formatter.ToE123());

            Assert.AreEqual("+375222284444", formatter.ToE123());
        }

        [TestMethod]
        public void E123FormatTest2()
        {
            const string phoneNumber = "11234567";
            const string code = "42";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Japan;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            Assert.IsNotNull(formatter.ToE123());

            Assert.AreEqual("+814211234567", formatter.ToE123());
        }
        #endregion

        #region Spaced E.123 format tests

        [TestMethod]
        public void SpacedE123FormatTest1()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            Assert.IsNotNull(formatter.ToSpacedE123());

            Assert.AreEqual("+375 222 284444", formatter.ToSpacedE123());
        }

        [TestMethod]
        public void SpacedE123FormatTest2()
        {
            const string phoneNumber = "11234567";
            const string code = "42";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Japan;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            Assert.IsNotNull(formatter.ToSpacedE123());

            Assert.AreEqual("+81 42 11234567", formatter.ToSpacedE123());
        }
        #endregion

        #region National format tests

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NationalFormatWithInvalidCountryCodeTest1()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Japan;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            formatter.ToNationalFormat();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NationalFormatWithInvalidCountryCodeTest2()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Japan;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            var nationalFormattedPhone = formatter.ToString();

            Assert.AreEqual(string.Empty, nationalFormattedPhone);
        }

        [TestMethod]
        public void NationalFormatTest1()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            var nationalFormattedPhone = formatter.ToNationalFormat();

            Assert.IsNotNull(nationalFormattedPhone);

            Assert.AreEqual("8-0222-284444", nationalFormattedPhone);

            Assert.AreEqual("+375222284444", formatter.ToE123());

            Assert.AreEqual(nationalFormattedPhone, formatter.ToString());
        }

        [TestMethod]
        public void NationalFormatTest2()
        {
            const string phoneNumber = "6284444";
            const string code = "29";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            var nationalFormattedPhone = formatter.ToNationalFormat();

            Assert.IsNotNull(nationalFormattedPhone);

            Assert.AreEqual("8-029-6284444", nationalFormattedPhone);

            Assert.AreEqual("+375296284444", formatter.ToE123());

            Assert.AreEqual(nationalFormattedPhone, formatter.ToString());
        }
        #endregion
    }
}
