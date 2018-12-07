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
            Assert.AreEqual(result["testKey1"], "testValue1");
            Assert.AreEqual(result["testKey2"], "testValue2");
        }

        [Test]
        public void DeserializeStringArray()
        {
            var buffer = new byte[]
            {
                121, 0, 2, 115, 0, 5, 116, 101, 115, 116, 49, 0, 5, 116, 101, 115, 116, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            string[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as string[];

            Assert.NotNull(result);
            Assert.AreEqual(result[0], "test1");
            Assert.AreEqual(result[1], "test2");
        }

        [Test]
        public void DeserializeByte()
        {
            var buffer = new byte[]
            {
                98, 6
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            byte? result = Protocol16Deserializer.Deserialize(stream, typeCode) as byte?;

            Assert.NotNull(result);
            Assert.AreEqual(result, 6);
        }

        [Test]
        public void DeserializeDouble()
        {
            var buffer = new byte[]
            {
                100, 64, 147, 74, 51, 51, 51, 51, 51, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            double? result = Protocol16Deserializer.Deserialize(stream, typeCode) as double?;

            Assert.NotNull(result);
            Assert.AreEqual(result, 1234.55);
        }

        [Test]
        public void DeserializeEventData()
        {
            var buffer = new byte[]
            {
                101, 100, 0, 2, 0, 115, 0, 5, 116, 101, 115, 116, 49, 1, 115, 0, 5, 116, 101, 115, 116, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            EventData result = Protocol16Deserializer.Deserialize(stream, typeCode) as EventData;

            Assert.NotNull(result);
            Assert.AreEqual(result.Code, 100);
            Assert.AreEqual(result.Parameters[0], "test1");
            Assert.AreEqual(result.Parameters[1], "test2");
        }

        [Test]
        public void DeserializeFloat()
        {
            var buffer = new byte[]
            {
                102, 68, 154, 81, 154, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            float? result = Protocol16Deserializer.Deserialize(stream, typeCode) as float?;

            Assert.NotNull(result);
            Assert.AreEqual(result, 1234.55f);
        }

        [Test]
        public void DeserializeInteger()
        {
            var buffer = new byte[]
            {
                105, 0, 0, 4, 210, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            int? result = Protocol16Deserializer.Deserialize(stream, typeCode) as int?;

            Assert.NotNull(result);
            Assert.AreEqual(result, 1234);
        }

        [Test]
        public void DeserializeShort()
        {
            var buffer = new byte[]
            {
                107, 4, 210, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            short? result = Protocol16Deserializer.Deserialize(stream, typeCode) as short?;

            Assert.NotNull(result);
            Assert.AreEqual(result, 1234);
        }

        [Test]
        public void DeserializeLong()
        {
            var buffer = new byte[]
            {
                108, 0, 0, 0, 0, 0, 0, 4, 210, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            long? result = Protocol16Deserializer.Deserialize(stream, typeCode) as long?;

            Assert.NotNull(result);
            Assert.AreEqual(result, 1234L);
        }

        [Test]
        public void DeserializeIntegerArray()
        {
            var buffer = new byte[]
            {
                121, 0, 2, 105, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            int[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as int[];

            Assert.NotNull(result);
            Assert.AreEqual(result[0], 0);
            Assert.AreEqual(result[1], 1);
        }

        [Test]
        public void DeserializeBoolean()
        {
            var buffer = new byte[]
            {
                111, 1
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            bool? result = Protocol16Deserializer.Deserialize(stream, typeCode) as bool?;

            Assert.NotNull(result);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void DeserializeOperationResponse()
        {
            var buffer = new byte[]
            {
                112, 100, 0, 100, 42, 0, 2, 0, 115, 0, 5, 116, 101, 115, 116, 49, 1, 115, 0, 5, 116, 101, 115, 116, 50, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            OperationResponse result = Protocol16Deserializer.Deserialize(stream, typeCode) as OperationResponse;

            Assert.NotNull(result);
            Assert.AreEqual(result.OperationCode, 100);
            Assert.AreEqual(result.ReturnCode, 100);
            Assert.AreEqual(result.Parameters[0], "test1");
            Assert.AreEqual(result.Parameters[1], "test2");
        }

        [Test]
        public void DeserializeOperationRequest()
        {
            var buffer = new byte[]
            {
                113, 100, 0, 2, 0, 115, 0, 5, 116, 101, 115, 116, 49, 1, 115, 0, 5, 116, 101, 115, 116, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            OperationRequest result = Protocol16Deserializer.Deserialize(stream, typeCode) as OperationRequest;

            Assert.NotNull(result);
            Assert.AreEqual(result.OperationCode, 100);
            Assert.AreEqual(result.Parameters[0], "test1");
            Assert.AreEqual(result.Parameters[1], "test2");
        }

        [Test]
        public void DeserializeString()
        {
            var buffer = new byte[]
            {
                115, 0, 12, 116, 101, 115, 116, 95, 109, 101, 115, 115, 97, 103, 101, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            string result = Protocol16Deserializer.Deserialize(stream, typeCode) as string;

            Assert.NotNull(result);
            Assert.AreEqual(result, "test_message");
        }

        [Test]
        public void DeserializeByteArray()
        {
            var buffer = new byte[]
            {
                120, 0, 0, 0, 2, 6, 7, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            byte[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as byte[];

            Assert.NotNull(result);
            Assert.AreEqual(result[0], 6);
            Assert.AreEqual(result[1], 7);
        }

        [Test]
        public void DeserializeArray()
        {
            var buffer = new byte[]
            {
                121, 0, 2, 115, 0, 5, 116, 101, 115, 116, 49, 0, 5, 116, 101, 115, 116, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            Array result = Protocol16Deserializer.Deserialize(stream, typeCode) as Array;

            Assert.NotNull(result);
            Assert.AreEqual(result.GetValue(0), "test1");
            Assert.AreEqual(result.GetValue(1), "test2");
        }

        [Test]
        public void DeserializeObjectArray()
        {
            var buffer = new byte[]
            {
                122, 0, 2, 115, 0, 5, 116, 101, 115, 116, 49, 115, 0, 5, 116, 101, 115, 116, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            object[] result = Protocol16Deserializer.Deserialize(stream, typeCode) as object[];

            Assert.NotNull(result);
            Assert.AreEqual(result[0], "test1");
            Assert.AreEqual(result[1], "test2");
        }
    }
}
