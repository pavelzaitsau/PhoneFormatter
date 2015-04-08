using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PavelZaitsau.PhoneFormatter.Tests
{
    [TestClass]
    public class InitializationTests
    {
        [TestMethod]
        public void SimpleInitializationTest()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            var formatter = Formatter.Number(countryPhoneCode, code, phoneNumber);

            Assert.IsNotNull(formatter);

            Assert.AreEqual(phoneNumber, formatter.PhoneNumber);

            Assert.AreEqual(code, formatter.Code);

            Assert.AreEqual(countryPhoneCode, formatter.CountryPhoneCode);
        }

        #region Phone number validations

        [TestMethod]
        [Description("Phone number contains less than 6 figures.")]
        [ExpectedException(typeof(FormatException))]
        public void PhoneNumberValidationTest2()
        {
            const string phoneNumber = "99999";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            // ReSharper disable once ObjectCreationAsStatement
            Formatter.Number(countryPhoneCode, code, phoneNumber);
        }

        [TestMethod]
        [Description("Phone number contains more than 8 figures.")]
        [ExpectedException(typeof(FormatException))]
        public void PhoneNumberValidationTest3()
        {
            const string phoneNumber = "999999999";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            // ReSharper disable once ObjectCreationAsStatement
            Formatter.Number(countryPhoneCode, code, phoneNumber);
        }
        #endregion

        #region Code validations

        [TestMethod]
        [Description("Code contains less than 2 figures.")]
        [ExpectedException(typeof(FormatException))]
        public void CodeValidationTest1()
        {
            const string phoneNumber = "284444";
            const string code = "1";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            // ReSharper disable once ObjectCreationAsStatement
            Formatter.Number(countryPhoneCode, code, phoneNumber);
        }

        [TestMethod]
        [Description("Code contains more than 4 figures.")]
        [ExpectedException(typeof(FormatException))]
        public void CodeValidationTest2()
        {
            const string phoneNumber = "284444";
            const string code = "99999";
            const CountryPhoneCode countryPhoneCode = CountryPhoneCode.Belarus;

            // ReSharper disable once ObjectCreationAsStatement
            Formatter.Number(countryPhoneCode, code, phoneNumber);
        }
        #endregion

        #region Country phone code validations

        [TestMethod]
        [Description("Country phone code is illegal enum value.")]
        [ExpectedException(typeof(System.ComponentModel.InvalidEnumArgumentException))]
        public void CountryPhoneCodeValidationTest1()
        {
            const string phoneNumber = "284444";
            const string code = "222";
            const CountryPhoneCode countryPhoneCode = (CountryPhoneCode)1000;

            // ReSharper disable once ObjectCreationAsStatement
            Formatter.Number(countryPhoneCode, code, phoneNumber);
        }
        #endregion
    }
}
