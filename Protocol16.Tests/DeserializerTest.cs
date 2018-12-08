using NUnit.Framework;

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

            Deserializer.Deserialize(out short result, buffer, ref offset);

            Assert.AreEqual(result, short.MaxValue);
            Assert.AreEqual(offset, 2);
        }

        [Test]
        public void DeserializeInteger()
        {
            var buffer = new byte[]
            {
                127, 255, 255, 255
            };
            int offset = 0;

            Deserializer.Deserialize(out int result, buffer, ref offset);

            Assert.AreEqual(result, int.MaxValue);
            Assert.AreEqual(offset, 4);
        }
    }
}
