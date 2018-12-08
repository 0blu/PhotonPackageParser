using NUnit.Framework;

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

            Serializer.Serialize(1234, buffer, ref offset);

            Assert.AreEqual(buffer, resultBuffer);
            Assert.AreEqual(offset, 4);
        }
    }
}
