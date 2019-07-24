using NUnit.Framework;
using Protocol16.Photon;

namespace Protocol16.Tests
{
    [TestFixture]
    public class SerializerTest
    {
        [Test]
        public void SerializeInteger()
        {
            byte[] resultBuffer = new byte[]
            {
                0, 0, 4, 210
            };
            byte[] buffer = new byte[4];
            int offset = 0;

            NumberSerializer.Serialize(1234, buffer, ref offset);

            Assert.AreEqual(resultBuffer, buffer);
            Assert.AreEqual(4, offset);
        }
    }
}
