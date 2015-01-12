using NUnit.Framework;
using PredicateMaps.Exceptions;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMapsTests.Exceptions
{
    [TestFixture]
    public class InconsitentIndexExceptionTests
    {
        [Test]
        public void MessageForExceptionHasIntParametersInCorrectPossitionsInReturnMessage()
        {
            var keyCollectionSize = 3;
            var dataCollectionSize = 5;
            var exception = new InconsitentIndexException(keyCollectionSize, dataCollectionSize);
            var expectedMessage = string.Format(strings.InconsistentCollectionSizeError, keyCollectionSize, dataCollectionSize);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

    }
}
