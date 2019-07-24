using NUnit.Framework;
using Protocol16.Photon;

namespace Protocol16.Tests
{
    [TestFixture]
    public class DeserializerTest
    {
        [Test]
        public void DeserializeShort()
        {
            var buffer = new byte[]
            {
                127, 255
            };
            int offset = 0;

            NumberDeserializer.Deserialize(out short result, buffer, ref offset);

            Assert.AreEqual(short.MaxValue, result);
            Assert.AreEqual(2, offset);
        }

        [Test]
        public void DeserializeInteger()
        {
            var buffer = new byte[]
            {
                127, 255, 255, 255
            };
            int offset = 0;

            NumberDeserializer.Deserialize(out int result, buffer, ref offset);

            Assert.AreEqual(int.MaxValue, result);
            Assert.AreEqual(4, offset);
        }
    }
}
