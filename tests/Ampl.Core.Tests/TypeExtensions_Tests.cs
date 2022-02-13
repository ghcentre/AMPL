using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Ampl.Core.Tests
{
    [TestFixture]
    public class TypeExtensions_Tests
    {
        #region ExtractGenericInterface

        [Test]
        public void ExtractGeneticInterface_NullExtension_ReturnsNull()
        {
            Type? someType = null;
            Type? result = someType.ExtractGenericInterface(typeof(IEnumerable<>));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ExtractGeneticInterface_NullSecondParam_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(string).ExtractGenericInterface(null));
        }

        [Test]
        public void ExtractGeneticInterface_FirstNotImplements_ReturnsNull()
        {
            Type someType = typeof(int);
            Type? result = someType.ExtractGenericInterface(typeof(IEnumerable<>));
            Assert.IsNull(result);
        }

        [Test]
        public void ExtractGeneticInterface_Implementing_ReturnsFirstGeneric()
        {
            Type someType = typeof(string);
            Type? result = someType.ExtractGenericInterface(typeof(IEnumerable<>));
            Assert.AreEqual(typeof(IEnumerable<char>), result);
        }

        [Test]
        public void ExtractGeneticInterface_GenericType_ReturnsThisType()
        {
            Type someType = typeof(IEnumerable<string>);
            Type? result = someType.ExtractGenericInterface(typeof(IEnumerable<>));
            Assert.AreEqual(typeof(IEnumerable<string>), result);
        }

        [Test]
        public void ExtractGeneticInterface_GenericTypeSecondParamNotGeneric_ReturnsNull()
        {
            Type someType = typeof(IEnumerable<string>);
            Type? result = someType.ExtractGenericInterface(typeof(string));
            Assert.IsNull(result);
        }

        #endregion
    }
}
