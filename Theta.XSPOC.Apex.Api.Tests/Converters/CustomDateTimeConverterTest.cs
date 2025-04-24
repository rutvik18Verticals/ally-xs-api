using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.Converters;

namespace Theta.XSPOC.Apex.Api.Tests.Converters
{
    [TestClass]
    public class CustomDateTimeConverterTest
    {

        [TestMethod]
        public void WriteJsonSerializeDateOnlySuccess()
        {
            // Arrange
            var converter = new CustomDateTimeConverter();

            var date = new DateTime(2024, 2, 27); // Example date with no time component

            // Act
            var serialized = JsonConvert.SerializeObject(date, converter);

            // Assert
            Assert.AreEqual("\"02/27/2024\"", serialized);

        }

        [TestMethod]
        public void WriteJsonSerializeDateTimeSuccess()
        {
            // Arrange
            var converter = new CustomDateTimeConverter();
            var dateTime = new DateTime(2024, 2, 27, 14, 30, 0);

            // Act
            var serialized = JsonConvert.SerializeObject(dateTime, converter);

            // Assert
            Assert.AreEqual("\"02/27/2024 2:30:00 PM\"", serialized);
        }

    }

}

