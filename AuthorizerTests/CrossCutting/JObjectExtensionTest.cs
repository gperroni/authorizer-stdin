using Authorizer.CrossCutting.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizerTests.CrossCutting
{
    [TestClass]
    public class JOBjectExtensionTest
    {
        [TestMethod]
        public void ShoudTypeOjbectCorrectly()
        {
            //Arrenge
            var jObject = new JObject
            {
                ["Property"] = 123
            };

            //Action
            var value = jObject.GetTypedProperty<int>("Property");

            //Assert
            Assert.AreEqual(value, 123);
        }
    }
}
