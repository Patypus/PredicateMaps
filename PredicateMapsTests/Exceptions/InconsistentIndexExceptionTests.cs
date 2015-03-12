using NUnit.Framework;
using PredicateMaps.Exceptions;
using PredicateMaps.Resources;

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
            var expectedMessage = string.Format(StringResources.InconsistentCollectionSizeError, keyCollectionSize, dataCollectionSize);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

    }
}
