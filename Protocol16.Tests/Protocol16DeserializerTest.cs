using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Protocol16.Tests
{
    [TestFixture]
    public class Protocol16DeserializerTest
    {
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

        [Test]
        public void DeserializeByte()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            byte? result = Protocol16Deserializer.Deserialize(stream, typeCode) as byte?;

            Assert.NotNull(result);
            Assert.True(result.Equals(6));
        }

        [Test]
        public void DeserializeDouble()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            double? result = Protocol16Deserializer.Deserialize(stream, typeCode) as double?;

            Assert.NotNull(result);
            Assert.True(result == 1234.55);
        }

        //[Test]
        //public void DeserializeEventData()
        //{

        //}

        [Test]
        public void DeserializeFloat()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            float? result = Protocol16Deserializer.Deserialize(stream, typeCode) as float?;

            Assert.NotNull(result);
            Assert.True(result == 1234.55);
        }

        [Test]
        public void DeserializeInteger()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            int? result = Protocol16Deserializer.Deserialize(stream, typeCode) as int?;

            Assert.NotNull(result);
            Assert.True(result == 1234);
        }

        [Test]
        public void DeserializeShort()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            short? result = Protocol16Deserializer.Deserialize(stream, typeCode) as short?;

            Assert.NotNull(result);
            Assert.True(result == 1234);
        }

        [Test]
        public void DeserializeLong()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            long? result = Protocol16Deserializer.Deserialize(stream, typeCode) as long?;

            Assert.NotNull(result);
            Assert.True(result == 1234);
        }

        [Test]
        public void DeserializeIntegerArray()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            int[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as int[];

            Assert.NotNull(result);
            Assert.True(result[0] == 0);
            Assert.True(result[1] == 1);
        }

        [Test]
        public void DeserializeBoolean()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            bool? result = Protocol16Deserializer.Deserialize(stream, typeCode) as bool?;

            Assert.NotNull(result);
            Assert.True(result);
        }

        //[Test]
        //public void DeserializeOperationResponse()
        //{

        //}

        //[Test]
        //public void DeserializeOperationRequest()
        //{

        //}

        [Test]
        public void DeserializeString()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            string result = Protocol16Deserializer.Deserialize(stream, typeCode) as string;

            Assert.NotNull(result);
            Assert.True(result.Equals("test_message"));
        }

        [Test]
        public void DeserializeByteArray()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            byte[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as byte[];

            Assert.NotNull(result);
            Assert.True(result[0].Equals(6));
            Assert.True(result[1].Equals(7));
        }

        //[Test]
        //public void DeserializeArray()
        //{
        //    var buffer = new byte[]
        //    {

        //    };

        //    var stream = new Protocol16Stream(buffer);
        //    byte typeCode = (byte)stream.ReadByte();
        //    Array result = Protocol16Deserializer.Deserialize(stream, typeCode) as Array;

        //    Assert.NotNull(result);
        //    Assert.True();
        //}

        [Test]
        public void DeserializeObjectArray()
        {
            var buffer = new byte[]
            {

            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            object[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as object[];

            Assert.NotNull(result);
            Assert.True(result[0] is string);
            Assert.True(result[0].Equals("test1"));
            Assert.True(result[1] is string);
            Assert.True(result[1].Equals("test2"));
        }
    }
}
