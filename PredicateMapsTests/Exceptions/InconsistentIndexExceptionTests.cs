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
    public class InconsistentIndexExceptionTests
    {
        [Test]
        public void MessageForExceptionHasIntParametersInCorrectPossitionsInReturnMessage()
        {
            var keyCollectionSize = 3;
            var dataCollectionSize = 5;
            var exception = new InconsistentIndexException(keyCollectionSize, dataCollectionSize);
            var expectedMessage = string.Format(Strings.InconsistentCollectionSizeError, keyCollectionSize, dataCollectionSize);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

    }
}
