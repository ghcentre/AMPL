using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class DictionaryExtensions_Tests
    {
        #region IncludeObjectProperties
        
        [Test]
        public void IncludeObjectProperties_NullThis_Throws()
        {
            Dictionary<string, object>? dic = null;
            var anon = new { IntProp = 5, StringProp = "String" };

            Assert.Throws<ArgumentNullException>(() => dic!.IncludeObjectProperties(anon));
        }

        [Test]
        public void IncludeObjectPropertis_NullArg_Throws()
        {
            var dic = new Dictionary<string, object>();
            object? anon = null;

            Assert.Throws<ArgumentNullException>(() => dic.IncludeObjectProperties(anon!));
        }

        [Test]
        public void IncludeObjectProperties_Add_AddsAllProperties()
        {
            var dic = new Dictionary<string, object>();
            var anon = new { IntProp = 5, StringProp = "String" };

             dic.IncludeObjectProperties(anon);

            Assert.That(dic.Count, Is.EqualTo(2));
        }

        [Test]
        public void IncludeObjectProperties_Add_ReferencesSameObjects()
        {
            var dic = new Dictionary<string, object>();
            var anon = new { IntProp = 5, StringProp = "String" };

            dic.IncludeObjectProperties(anon);

            Assert.That((int)dic["IntProp"], Is.EqualTo(5));
            Assert.That((string)dic["StringProp"], Is.SameAs(anon.StringProp));
        }

        [Test]
        public void IncludeObjectProperties_AddToNonEmpty_Replaces()
        {
            var dic = new Dictionary<string, object>() { { "StringProp", "OldString" } };
            var anon = new { IntProp = 5, StringProp = "String" };

            dic.IncludeObjectProperties(anon);

            Assert.That(dic.Count, Is.EqualTo(2));
            Assert.That((string)dic["StringProp"], Is.SameAs(anon.StringProp));
            Assert.That((string)dic["StringProp"], Is.Not.EqualTo("OldString"));
        }

        #endregion
    }
}
