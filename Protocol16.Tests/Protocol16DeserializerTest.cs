using NUnit.Framework;
using System.Collections.Generic;

namespace Protocol16.Tests
{
    [TestFixture]
    public class Protocol16DeserializerTest
    {
        //[Test]
        //public void ExampleTest()
        //{
        //    Init

        //    Code

        //     Assert
        //}

        [Test]
        public void DeserializeDicitonary()
        {
            var buffer = new byte[]
            {
                68, 115, 115, 0, 2, 0, 8, 116, 101, 115, 116, 75, 101, 121, 49, 0, 10, 116, 101, 115, 116, 86, 97, 108, 117, 101, 49, 0, 8, 116, 101, 115, 116, 75, 101, 121, 50, 0, 10, 116, 101, 115, 116, 86, 97, 108, 117, 101, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            Dictionary<string, string> result = Protocol16Deserializer.Deserialize(stream, typeCode) as Dictionary<string, string>;

            Assert.NotNull(result);
            Assert.True(result["testKey1"] == "testValue1");
            Assert.True(result["testKey2"] == "testValue2");
        }

        [Test]
        public void DeserializeStringArray()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            string[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as string[];

            Assert.NotNull(result);
            Assert.True(result[0].Equals("test1"));
            Assert.True(result[1].Equals("test2"));
        }

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}

        //[Test]
        //public void Deserialize()
        //{

        //}
    }
}
