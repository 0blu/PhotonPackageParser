using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Protocol16.Photon;

namespace Protocol16.Tests
{
    [TestFixture]
    public class Protocol16DeserializerTest
    {
        [Test]
        public void DeserializeHashtable()
        {
            TestDeserializeMethod(new byte[]
            {
                0x68, 0x00, 0x02, 0x73, 0x00, 0x01, 0x61, 0x69,
                0x00, 0x00, 0x00, 0x02, 0x69, 0x00, 0x00, 0x00,
                0x04, 0x66, 0x40, 0x13, 0x33, 0x33,
            }, new Hashtable
            {
                ["a"] = 2,
                [4] = 2.3f,
            });
        }

        [Test]
        public void DeserializeDictionary()
        {
            TestDeserializeMethod(new byte[]
            {
                0x44, 0x73, 0x73, 0x00, 0x02, 0x00, 0x08, 0x74,
                0x65, 0x73, 0x74, 0x4b, 0x65, 0x79, 0x31, 0x00,
                0x0a, 0x74, 0x65, 0x73, 0x74, 0x56, 0x61, 0x6c,
                0x75, 0x65, 0x31, 0x00, 0x08, 0x74, 0x65, 0x73,
                0x74, 0x4b, 0x65, 0x79, 0x32, 0x00, 0x0a, 0x74,
                0x65, 0x73, 0x74, 0x56, 0x61, 0x6c, 0x75, 0x65,
                0x32,
            }, new Dictionary<string, string>
            {
                ["testKey1"] = "testValue1",
                ["testKey2"] = "testValue2",
            });
        }

        [Test]
        public void DeserializeStringArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x79, 0x00, 0x02, 0x73, 0x00, 0x05, 0x74, 0x65,
                0x73, 0x74, 0x31, 0x00, 0x05, 0x74, 0x65, 0x73,
                0x74, 0x32,
            }, new[] {"test1", "test2"});
        }

        [Test]
        public void DeserializeByte()
        {
            TestDeserializeMethod(new byte[]
            {
                0x62, 0x06
            }, (byte)6);
        }

        [Test]
        public void DeserializeDouble()
        {
            TestDeserializeMethod(new byte[]
            {
                0x64, 0x40, 0x93, 0x4a, 0x33, 0x33, 0x33, 0x33,
                0x33,
            }, 1234.55d);
        }

        [Test]
        public void DeserializeFloat()
        {
            TestDeserializeMethod(new byte[]
            {
                0x66, 0x44, 0x9A, 0x51, 0x9A,
            }, 1234.55f);
        }

        [Test]
        public void DeserializeInteger()
        {
            TestDeserializeMethod(new byte[]
            {
                0x69, 0x00, 0x00, 0x04, 0xD2,
            }, 1234);
        }

        [Test]
        public void DeserializeShort()
        {
            TestDeserializeMethod(new byte[]
            {
                0x6B, 0x04, 0xD2,
            }, (short)1234);
        }

        [Test]
        public void DeserializeLong()
        {
            TestDeserializeMethod(new byte[]
            {
                0x6C, 0x01, 0xBA, 0xD6, 0x53, 0xBE, 0xEC, 0x89,
                0x8E,
            }, 124647594879912334L);
        }

        [Test]
        public void DeserializeIntegerArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x79, 0x00, 0x04, 0x69, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                0xFF, 0xFF, 0xFF, 0xFD,
            }, new[] { 1, 0, 5, -3 });   
        }

        [Test]
        public void DeserializeTrue()
        {
            TestDeserializeMethod(new byte[]
            {
                0x6f, 0x01,
            }, true);
        }

        [Test]
        public void DeserializeFalse()
        {
            TestDeserializeMethod(new byte[]
            {
                0x6f, 0x00,
            }, false);
        }

        [Test]
        public void DeserializeString()
        {
            TestDeserializeMethod(new byte[]
            {
                0x73, 0x00, 0x0C, 0x74, 0x65, 0x73, 0x74, 0x5F,
                0x6D, 0x65, 0x73, 0x73, 0x61, 0x67, 0x65,
            }, "test_message");
        }

        [Test]
        public void DeserializeByteArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x78, 0x00, 0x00, 0x00, 0x08, 0x01, 0x02, 0x03,
                0x04, 0x05, 0x06, 0x00, 0xFF,
            }, new byte[] { 1, 2, 3, 4, 5, 6, 0, 255 });
        }

        [Test]
        public void Deserialize2dByteArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x79, 0x00, 0x02, 0x78, 0x00, 0x00, 0x00, 0x02,
                0x01, 0x03, 0x00, 0x00, 0x00, 0x02, 0x04, 0x03,
            }, new[] { new byte[] {1, 3}, new byte[] {4, 3} });
        }

        [Test]
        public void Deserialize2dIntArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x79, 0x00, 0x02, 0x6E, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03,
                0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04,
                0x00, 0x00, 0x00, 0x02,
            }, new[] { new[] { 1, 3 }, new[] { 4, 2 } });
        }
        
        [Test]
        public void DeserializeObjectArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x7A, 0x00, 0x03, 0x69, 0x00, 0x00, 0x00, 0x01,
                0x73, 0x00, 0x01, 0x41, 0x44, 0x73, 0x73, 0x00,
                0x01, 0x00, 0x03, 0x61, 0x62, 0x63, 0x00, 0x03,
                0x64, 0x65, 0x66,
            }, new object[]
            {
                1, "A", new Dictionary<string, string>
                {
                    ["abc"] = "def",
                },
            });
        }

        [Test]
        public void DeserializeDictionaryArray()
        {
            TestDeserializeMethod(new byte[]
            {
                0x79, 0x00, 0x02, 0x44, 0x73, 0x73, 0x00, 0x01,
                0x00, 0x03, 0x61, 0x62, 0x63, 0x00, 0x03, 0x64,
                0x65, 0x66, 0x00, 0x01, 0x00, 0x03, 0x35, 0x2B,
                0x35, 0x00, 0x0C, 0x39, 0x20, 0x71, 0x75, 0x69,
                0x63, 0x6B, 0x20, 0x6D, 0x61, 0x74, 0x68,
            }, new[]
            {
                new Dictionary<string, string>
                {
                    ["abc"] = "def",
                },
                new Dictionary<string, string>
                {
                    ["5+5"] = "9 quick math",
                },
            });
        }

        [Test]
        public void DeserializeEventData()
        {
            var buffer = new byte[]
            {
                0x65, 0x64, 0x00, 0x02, 0x00, 0x73, 0x00, 0x05,
                0x74, 0x65, 0x73, 0x74, 0x31, 0x01, 0x73, 0x00,
                0x05, 0x74, 0x65, 0x73, 0x74, 0x32,
            };

            var stream = new Protocol16Stream(buffer);
            EventData result = Protocol16Deserializer.Deserialize(stream) as EventData;

            Assert.NotNull(result);
            Assert.AreEqual(100, result.Code);
            Assert.AreEqual("test1", result.Parameters[0]);
            Assert.AreEqual("test2", result.Parameters[1]);
        }


        [Test]
        public void DeserializeOperationResponse()
        {
            var buffer = new byte[]
            {
                0x70, 0x64, 0x00, 0x65, 0x2a, 0x00, 0x02, 0x00,
                0x73, 0x00, 0x05, 0x74, 0x65, 0x73, 0x74, 0x31,
                0x01, 0x73, 0x00, 0x05, 0x74, 0x65, 0x73, 0x74,
                0x32, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            OperationResponse result = Protocol16Deserializer.Deserialize(stream, typeCode) as OperationResponse;

            Assert.NotNull(result);
            Assert.AreEqual(result.OperationCode, 100);
            Assert.AreEqual(result.ReturnCode, 101);
            Assert.AreEqual(result.Parameters[0], "test1");
            Assert.AreEqual(result.Parameters[1], "test2");
        }


        [Test]
        public void DeserializeOperationResponseWithDebugMessage()
        {
            var buffer = new byte[]
            {
                0x70, 0x64, 0x00, 0x66, 0x73, 0x00, 0x11, 0x53,
                0x6F, 0x6D, 0x65, 0x44, 0x65, 0x62, 0x75, 0x67,
                0x20, 0x6D, 0x65, 0x73, 0x73, 0x61, 0x67, 0x65,
                0x00, 0x02, 0x00, 0x73, 0x00, 0x05, 0x74, 0x65,
                0x73, 0x74, 0x31, 0x01, 0x69, 0x00, 0x00, 0x00,
                0x02,
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            OperationResponse result = Protocol16Deserializer.Deserialize(stream, typeCode) as OperationResponse;

            Assert.NotNull(result);
            Assert.AreEqual(result.OperationCode, 100);
            Assert.AreEqual(result.ReturnCode, 102);
            Assert.AreEqual(result.DebugMessage, "SomeDebug message");
            Assert.AreEqual(result.Parameters[0], "test1");
            Assert.AreEqual(result.Parameters[1], 2);

        }

        [Test]
        public void DeserializeOperationRequest()
        {
            var buffer = new byte[]
            {
                0x71, 0x64, 0x00, 0x02, 0x00, 0x73, 0x00, 0x05,
                0x74, 0x65, 0x73, 0x74, 0x31, 0x01, 0x73, 0x00,
                0x05, 0x74, 0x65, 0x73, 0x74, 0x32,
            };

            var stream = new Protocol16Stream(buffer);
            byte typeCode = (byte)stream.ReadByte();
            OperationRequest result = Protocol16Deserializer.Deserialize(stream, typeCode) as OperationRequest;

            Assert.NotNull(result);
            Assert.AreEqual(100, result.OperationCode);
            Assert.AreEqual("test1", result.Parameters[0]);
            Assert.AreEqual("test2", result.Parameters[1]);
        }
        
        private void TestDeserializeMethod(byte[] byteInput, object expected)
        {
            var input = new Protocol16Stream(byteInput);
            var actual = Protocol16Deserializer.Deserialize(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
