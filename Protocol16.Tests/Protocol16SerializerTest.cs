using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Protocol16.Photon;

namespace Protocol16.Tests
{
    [TestFixture]
    class Protocol16SerializerTest
    {
        [Test]
        public void SerializeArraySegment()
        {
            TestSerializeMethod(new byte[]
            {
                0x78, 0x00, 0x00, 0x00, 0x03, 0x03, 0x04, 0x00,
            }, new ArraySegment<byte>(new byte[]
            {
                1, 2, 3, 4, 0, 5, 6,
            }, 2, 3));
        }

        [Test]
        public void SerializeHashtable()
        {
            TestSerializeMethod(new byte[]
            {
                0x68, 0x00, 0x02, 0x69, 0x00, 0x00, 0x00, 0x04,
                0x66, 0x40, 0x13, 0x33, 0x33, 0x73, 0x00, 0x01,
                0x61, 0x69, 0x00, 0x00, 0x00, 0x02,
            }, new Hashtable
            {
                ["a"] = 2,
                [4] = 2.3f,
            });
        }

        [Test]
        public void SerializeDictionaryEasyTyped()
        {
            TestSerializeMethod(new byte[]
            {
                0x44, 0x73, 0x73, 0x00, 0x04, 0x00, 0x01, 0x32,
                0x00, 0x01, 0x35, 0x00, 0x01, 0x33, 0x00, 0x01,
                0x34, 0x00, 0x01, 0x35, 0x00, 0x01, 0x33, 0x00,
                0x01, 0x36, 0x00, 0x01, 0x32,
            }, new Dictionary<string, string>
            {
                ["2"] = "5",
                ["3"] = "4",
                ["5"] = "3",
                ["6"] = "2",
            });
        }

        [Test]
        public void SerializeDictionaryUnknownValue()
        {
            TestSerializeMethod(new byte[]
            {
                0x44, 0x73, 0x00, 0x00, 0x04, 0x00, 0x01, 0x32,
                0x73, 0x00, 0x01, 0x35, 0x00, 0x01, 0x33, 0x69,
                0x00, 0x00, 0x00, 0x02, 0x00, 0x01, 0x35, 0x6B,
                0x00, 0x05, 0x00, 0x01, 0x36, 0x44, 0x62, 0x00,
                0x00, 0x01, 0x14, 0x73, 0x00, 0x03, 0x2D, 0x32,
                0x30,
            }, new Dictionary<string, object>
            {
                ["2"] = "5",
                ["3"] = 2,
                ["5"] = (short)5,
                ["6"] = new Dictionary<byte, object>
                {
                    [20] = "-20",
                },
            });
        }

        [Test]
        public void SerializeDictionaryUnknownKey()
        {
            TestSerializeMethod(new byte[]
            {
                0x44, 0x00, 0x73, 0x00, 0x03, 0x69, 0x00, 0x00,
                0x00, 0x02, 0x00, 0x02, 0x35, 0x30, 0x66, 0x3F,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x31, 0x73, 0x00,
                0x04, 0x74, 0x65, 0x73, 0x74, 0x00, 0x04, 0x6E,
                0x75, 0x6C, 0x6C,
            }, new Dictionary<object, string>
            {
                [2] = "50",
                [0.5f] = "1",
                ["test"] = "null",
            });
        }

        [Test]
        public void SerializeDictionaryUnknownKeyAndValue()
        {
            TestSerializeMethod(new byte[]
            {
                0x44, 0x00, 0x00, 0x00, 0x04, 0x69, 0x00, 0x00,
                0x00, 0x02, 0x69, 0x00, 0x00, 0x00, 0x05, 0x66,
                0x3F, 0x00, 0x00, 0x00, 0x6B, 0xFF, 0x1A, 0x73,
                0x00, 0x04, 0x74, 0x65, 0x73, 0x74, 0x73, 0x00,
                0x04, 0x6E, 0x75, 0x6C, 0x6C, 0x64, 0x40, 0x10,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x44, 0x00,
                0x73, 0x00, 0x03, 0x69, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x02, 0x35, 0x30, 0x66, 0x3F, 0x00, 0x00,
                0x00, 0x00, 0x01, 0x31, 0x73, 0x00, 0x04, 0x74,
                0x65, 0x73, 0x74, 0x00, 0x04, 0x6E, 0x75, 0x6C,
                0x6C,
            }, new Dictionary<object, object>
            {
                [2] = 5,
                [0.5f] = (short)-230,
                ["test"] = "null",
                [4d] = new Dictionary<object, string>
                {
                    [2] = "50",
                    [0.5f] = "1",
                    ["test"] = "null",
                }
            });
        }

        [Test]
        public void SerializeIntArray()
        {
            TestSerializeMethod(new byte[]
            {
                0x6E, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x04, 0xFF, 0xFF, 0xFF,
                0xFF, 0x00, 0x00, 0x00, 0x06, 0xFF, 0xFF, 0xFE,
                0x08, 0x00, 0x00, 0x01, 0xF8,
            }, new[] { 1, 2, 0, 4, -1, 6, -504, 504 });
        }

        [Test]
        public void Serialize2dIntArray()
        {
            TestSerializeMethod(new byte[]
            {
                0x79, 0x00, 0x02, 0x6E, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03,
                0x00, 0x00, 0x00, 0x04,
            }, new[] { new[] {1, 2}, new[] {3, 4} });
        }

        [Test]
        public void SerializeStringArray()
        {
            TestSerializeMethod(new byte[]
            {
                0x61, 0x00, 0x04, 0x00, 0x05, 0x48, 0x65, 0x6C,
                0x6C, 0x6F, 0x00, 0x05, 0x57, 0x6F, 0x72, 0x6C,
                0x64, 0x00, 0x01, 0x21, 0x00, 0x00,
            }, new [] { "Hello", "World", "!", "" });
        }

        [Test]
        public void SerializeStringArrayWithNullInIt()
        {
            // Expect fallback to object array
            TestSerializeMethod(new byte[]
            {
                0x7A, 0x00, 0x04, 0x73, 0x00, 0x05, 0x48, 0x65,
                0x6C, 0x6C, 0x6F, 0x73, 0x00, 0x05, 0x57, 0x6F,
                0x72, 0x6C, 0x64, 0x73, 0x00, 0x01, 0x21, 0x2A,
            }, new[] { "Hello", "World", "!", null });
        }

        [Test]
        public void Serialize2dFloatArray()
        {
            TestSerializeMethod(new byte[]
            {
                0x79, 0x00, 0x02, 0x79, 0x00, 0x02, 0x66, 0x3F,
                0x80, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00,
                0x02, 0x66, 0x40, 0xA3, 0x33, 0x33, 0xC0, 0xC0,
                0x00, 0x00,
            }, new [] { new [] {1f, 2f}, new [] {5.1f, -6f} });
        }

        [Test]
        public void SerializeSuperLongStringArray()
        {
            var actual = new Protocol16Stream();
            var input = new string[short.MaxValue + 1];
            Assert.Throws<NotSupportedException>(
                () => { Protocol16Serializer.Serialize(actual, input, true); });
        }

        [Test]
        public void SerializeByteArray()
        {
            TestSerializeMethod(new byte[]
            {
                0x78, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x03,
                0x04, 0x05,
            }, new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 });
        }

        [Test]
        public void SerializeFalse()
        {
            TestSerializeMethod(new byte[]
            {
                0x6f, 0x00
            }, false);
        }

        [Test]
        public void SerializeTrue()
        {
            TestSerializeMethod(new byte[]
            {
                0x6f, 0x01
            }, true);
        }

        [Test]
        public void SerializeByte()
        {
            TestSerializeMethod(new byte[]
            {
                0x62, 0x85,
            }, (byte)133);
        }

        [Test]
        public void SerializeShort()
        {
            TestSerializeMethod(new byte[]
            {
                0x6B, 0x30, 0xa9
            }, (short)12457);
        }

        [Test]
        public void SerializeInteger()
        {
            TestSerializeMethod(new byte[]
            {
                0x69, 0x00, 0x00, 0x05, 0x2C,
            }, 1324);
        }

        [Test]
        public void SerializeLong()
        {
            TestSerializeMethod(new byte[]
            {
                0x6C, 0x00, 0x00, 0x70, 0x48, 0x86, 0x19, 0x30, 0x5F,
            }, 123456789753951L);
        }

        [Test]
        public void SerializeFloat()
        {
            TestSerializeMethod(new byte[]
            {
                0x66, 0x3F, 0xA0, 0x00, 0x00,
            }, 1.25f);
        }
        [Test]

        public void SerializeDouble()
        {
            TestSerializeMethod(new byte[]
            {
                0x64, 0x40, 0x28, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00,
            }, 12.25d);
        }

        [Test]
        public void SerializeEmptyEventData()
        {
            TestSerializeMethod(new byte[]
            {
                0x65, 0x14, 0x00, 0x00,
            }, new EventData(20, null));
        }

        [Test]
        public void SerializeOperationRequestMessage()
        {
            TestSerializeMethod(new byte[]
            {
                0x71, 0x7B, 0x00, 0x02, 0x00, 0x69, 0x00, 0x00,
                0x00, 0x02, 0x04, 0x73, 0x00, 0x03, 0x61, 0x62,
                0x63,
            }, new OperationRequest(123, new Dictionary<byte, object>
            {
                [0] = 2,
                [4] = "abc",
            }));
        }

        [Test]
        public void SerializeOperationResponseWithDebugMessage()
        {
            TestSerializeMethod(new byte[]
            {
                0x70, 0x7B, 0x00, 0x52, 0x73, 0x00, 0x09, 0x47,
                0x6F, 0x6F, 0x64, 0x20, 0x4A, 0x6F, 0x62, 0x21,
                0x00, 0x02, 0x00, 0x69, 0x00, 0x00, 0x00, 0x02,
                0x04, 0x73, 0x00, 0x03, 0x61, 0x62, 0x63,
            }, new OperationResponse(123, 82, "Good Job!", new Dictionary<byte, object>
            {
                [0] = 2,
                [4] = "abc",
            }));
        }

        [Test]
        public void SerializeOperationResponse()
        {
            TestSerializeMethod(new byte[]
            {
                0x70, 0x7B, 0x00, 0x52, 0x2A, 0x00, 0x02, 0x00,
                0x69, 0x00, 0x00, 0x00, 0x02, 0x04, 0x73, 0x00,
                0x03, 0x61, 0x62, 0x63,
            }, new OperationResponse(123, 82, null, new Dictionary<byte, object>
            {
                [0] = 2,
                [4] = "abc",
            }));
        }

        [Test]
        public void SerializeSimpleEventData()
        {
            TestSerializeMethod(new byte[]
            {
                0x65, 0x14, 0x00, 0x02, 0x00, 0x69, 0x00, 0x00,
                0x00, 0x02, 0x04, 0x73, 0x00, 0x03, 0x61, 0x62,
                0x63,
            }, new EventData(20, new Dictionary<byte, object>
            {
                [0] = 2,
                [4] = "abc",
            }));
        }

        [Test]
        public void SerializeEmptyString()
        {
            TestSerializeMethod(new byte[]
            {
                0x73, 0, 0,
            }, string.Empty);
        }

        [Test]
        public void SerializeSuperLongString()
        {
            var actual = new Protocol16Stream();
            string input = string.Empty.PadLeft(short.MaxValue + 1, '0');
            Assert.Throws<NotSupportedException>(
                () => { Protocol16Serializer.Serialize(actual, input, true); });
        }

        [Test]
        public void SerializeSuperLongObjectArray()
        {
            var actual = new Protocol16Stream();
            object[] input = new object[short.MaxValue + 1];
            Assert.Throws<NotSupportedException>(
                () => { Protocol16Serializer.Serialize(actual, input, true); });
        }

        [Test]
        public void SerializeArray()
        {
            TestSerializeMethod(new byte[]
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
        public void SerializeSuperLongArray()
        {
            var actual = new Protocol16Stream();
            float[] input = new float[short.MaxValue + 1];
            Assert.Throws<NotSupportedException>(
                () => { Protocol16Serializer.Serialize(actual, input, true); });
        }

        [Test]
        public void SerializeDictionaryArray()
        {
            TestSerializeMethod(new byte[]
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
        public void SerializeString()
        {
            TestSerializeMethod(new byte[]
            {
                0x73, 0x00, 0x0c, 0x48, 0x65, 0x6C, 0x6C, 0x6F,
                0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x21
            }, "Hello World!");
        }

        private void TestSerializeMethod(byte[] expectedBytes, object obj)
        {
            var expected = new MemoryStream(expectedBytes);
            var actual = new Protocol16Stream();
            Protocol16Serializer.Serialize(actual, obj, true);
            ToString(actual);
            Assert.AreEqual(expected, actual);
        }

        private void ToString(Stream s)
        {
            MemoryStream ms = new MemoryStream();
            s.Position = 0;
            s.CopyTo(ms);
            string d = "";
            for (int i = 0; i < ms.Length; i++)
            {
                d += "0x" + ms.GetBuffer()[i].ToString("X2") + ", ";
            }
            Console.WriteLine(d);
        }
    }
}
