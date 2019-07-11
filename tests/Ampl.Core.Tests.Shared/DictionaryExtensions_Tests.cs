using NUnit.Framework;
using System;
using Ampl.Collections;
using System.Collections.Generic;

namespace Ampl.Core.Tests.Shared
{
    [TestFixture]
    public class DictionaryExtensions_Tests
    {
        [Test]
        public void IncludeObjectProperties_NullThis_Throws()
        {
            // arrange
            Dictionary<string, object> dic = null;
            var anon = new { IntProp = 5, StringProp = "String" };
            // act-assert
            Assert.Throws<ArgumentNullException>(() => dic.IncludeObjectProperties(anon));
        }

        [Test]
        public void IncludeObjectPropertis_NullArg_Throws()
        {
            // arrange
            var dic = new Dictionary<string, object>();
            object anon = null;
            // act-assert
            Assert.Throws<ArgumentNullException>(() => dic.IncludeObjectProperties(anon));
        }

        [Test]
        public void IncludeObjectProperties_Add_AddsAllProperties()
        {
            // arrange
            var dic = new Dictionary<string, object>();
            var anon = new { IntProp = 5, StringProp = "String" };
            // act
            dic.IncludeObjectProperties(anon);
            // assert
            Assert.That(dic.Count, Is.EqualTo(2));
        }

        [Test]
        public void IncludeObjectProperties_Add_ReferencesSameObjects()
        {
            // arrange
            var dic = new Dictionary<string, object>();
            var anon = new { IntProp = 5, StringProp = "String" };
            // act
            dic.IncludeObjectProperties(anon);
            // assert
            Assert.That((int)dic["IntProp"], Is.EqualTo(5));
            Assert.That((string)dic["StringProp"], Is.SameAs(anon.StringProp));
        }

        [Test]
        public void IncludeObjectProperties_AddToNonEmpty_Replaces()
        {
            // arrange
            var dic = new Dictionary<string, object>() { { "StringProp", "OldString" } };
            var anon = new { IntProp = 5, StringProp = "String" };
            // act
            dic.IncludeObjectProperties(anon);
            // assert
            Assert.That(dic.Count, Is.EqualTo(2));
            Assert.That((string)dic["StringProp"], Is.SameAs(anon.StringProp));
            Assert.That((string)dic["StringProp"], Is.Not.EqualTo("OldString"));
        }


        [Test]
        public void GetValueOrDefault_NullThis_Throws()
        {
            Dictionary<string, string> arg = null;
            Assert.Throws<ArgumentNullException>(() => Collections.DictionaryExtensions.GetValueOrDefault(arg, "test"));
        }

        private Dictionary<int, string> _errorMessages = new Dictionary<int, string>() {
            { 0, "The operation completed successfully" },
            { 1, "Incorrect function" },
            { 2, "File not found" },
            { 3, "Path not found" },
            { 4, "Cannot open file" },
            { 5, "Access denied" }
        };

        [Test]
        public void GetValueOrDefault_ExistingKey_Returns()
        {
            int key = 3;
            var result = Collections.DictionaryExtensions.GetValueOrDefault(_errorMessages, key);
            Assert.That(result, Is.EqualTo("Path not found"));
        }

        [Test]
        public void GetValueOrDefault_NonExistingKey_ReturnsNull()
        {
            int key = 1000;
            var result = Collections.DictionaryExtensions.GetValueOrDefault(_errorMessages, key);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetValueOrDefault_ValueTypesNonExistingKey_ReturnsDefault()
        {
            var dictionary = new Dictionary<string, int>() {
                ["one"] = 1,
                ["two"] = 2,
                ["three"] = 3
            };
            string key = "four";
            int result = Collections.DictionaryExtensions.GetValueOrDefault(dictionary, key);
            Assert.That(result, Is.EqualTo(default(int)));
        }
    }
}
