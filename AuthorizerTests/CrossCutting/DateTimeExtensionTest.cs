using Authorizer.CrossCutting.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AuthorizerTests.CrossCutting
{
    [TestClass]
    public class DateTimeExtensionTest
    {
        [TestMethod]
        public void ShouldTransformDateCorrectly()
        {
            //Arrenge
            var date = new DateTime(2020, 2, 4, 12, 30, 30, 333);

            //Action
            var dateString = date.ToStringWithMilliseconds();

            //Assert
            Assert.AreEqual(dateString, "2020-02-04T12:30:30.333");
        }
    }
}
