using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Protocol16
{
    public static class Protocol16Deserializer
    {
        #region fields
        private static readonly int shortSize = 2;
        private static readonly int integerSize = 4;
        private static readonly int longSize = 8;
        private static readonly int floatSize = 4;
        private static readonly int doubleSize = 8;
        #endregion

        #region methods
        public static object Deserialize(Protocol16Stream stream, byte typeCode)
        {
            switch ((Protocol16Type)typeCode)
            {
                case Protocol16Type.Unknown:
                case Protocol16Type.Null:
                    return null;
                case Protocol16Type.Dictionary:
                    return DeserializeDictionary(stream);
                case Protocol16Type.StringArray:
                    return DeserializeStringArray(stream);
                case Protocol16Type.Byte:
                    return DeserializeByte(stream);
                case Protocol16Type.Double:
                    return DeserializeDouble(stream);
                case Protocol16Type.EventData:
                    return DeserializeEventData(stream);
                case Protocol16Type.Float:
                    return DeserializeFloat(stream);
                case Protocol16Type.Integer:
                    return DeserializeInteger(stream);
                case Protocol16Type.Short:
                    return DeserializeShort(stream);
                case Protocol16Type.Long:
                    return DeserializeLong(stream);
                case Protocol16Type.IntegerArray:
                    return DeserializeIntArray(stream);
                case Protocol16Type.Boolean:
                    return DeserializeBoolean(stream);
                case Protocol16Type.OperationResponse:
                    return DeserializeOperationResponse(stream);
                case Protocol16Type.OperationRequest:
                    return DeserializeOperationRequest(stream);
                case Protocol16Type.String:
                    return DeserializeString(stream);
                case Protocol16Type.ByteArray:
                    return DeserializeByteArray(stream);
                case Protocol16Type.Array:
                    return DeserializeArray(stream);
                case Protocol16Type.ObjectArray:
                    return DeserializeObjectArray(stream);
                default:
                    throw new ArgumentException($"Type code: {typeCode} not implemented.");
            }


        }

        public static OperationRequest DeserializeOperationRequest(Protocol16Stream stream)
        {
            byte operationCode = DeserializeByte(stream);
            Dictionary<byte, object> parameters = DeserializeParameterTable(stream);

            return new OperationRequest(operationCode, parameters);
        }

        public static OperationResponse DeserializeOperationResponse(Protocol16Stream stream)
        {
            byte operationCode = DeserializeByte(stream);
            short returnCode = DeserializeShort(stream);
            string debugMessage = (Deserialize(stream, DeserializeByte(stream)) as string);
            Dictionary<byte, object> parameters = DeserializeParameterTable(stream);

            return new OperationResponse(operationCode, returnCode, debugMessage, parameters);
        }

        public static EventData DeserializeEventData(Protocol16Stream stream)
        {
            byte code = DeserializeByte(stream);
            Dictionary<byte, object> parameters = DeserializeParameterTable(stream);

            return new EventData(code, parameters);
        }
        #endregion

        #region private methods
        private static Type GetTypeOfCode(byte typeCode)
        {
            switch ((Protocol16Type)typeCode)
            {
                case Protocol16Type.Unknown:
                case Protocol16Type.Null:
                    return typeof(object);
                case Protocol16Type.Dictionary:
                    return typeof(IDictionary);
                case Protocol16Type.StringArray:
                    return typeof(string[]);
                case Protocol16Type.Byte:
                    return typeof(byte);
                case Protocol16Type.Double:
                    return typeof(double);
                case Protocol16Type.EventData:
                    return typeof(EventData);
                case Protocol16Type.Float:
                    return typeof(float);
                case Protocol16Type.Integer:
                    return typeof(int);
                case Protocol16Type.Short:
                    return typeof(short);
                case Protocol16Type.Long:
                    return typeof(long);
                case Protocol16Type.IntegerArray:
                    return typeof(int[]);
                case Protocol16Type.Boolean:
                    return typeof(bool);
                case Protocol16Type.OperationResponse:
                    return typeof(OperationResponse);
                case Protocol16Type.OperationRequest:
                    return typeof(OperationRequest);
                case Protocol16Type.String:
                    return typeof(string);
                case Protocol16Type.ByteArray:
                    return typeof(byte[]);
                case Protocol16Type.Array:
                    return typeof(Array);
                case Protocol16Type.ObjectArray:
                    return typeof(object[]);
                default:
                    throw new ArgumentException($"Type code: {typeCode} not implemented.");
            }
        }

        private static byte DeserializeByte(Protocol16Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        private static bool DeserializeBoolean(Protocol16Stream stream)
        {
            return stream.ReadByte() != 0;
        }

        public static short DeserializeShort(Protocol16Stream stream)
        {
            var buffer = new byte[shortSize];

            stream.Read(buffer, 0, shortSize);

            return (short)((int)buffer[0] << 8 | (int)buffer[1]);
        }

        private static int DeserializeInteger(Protocol16Stream stream)
        {
            var buffer = new byte[integerSize];

            stream.Read(buffer, 0, integerSize);

            return (int)buffer[0] << 24 | (int)buffer[1] << 16 | (int)buffer[2] << 8 | (int)buffer[3];
        }

        private static long DeserializeLong(Protocol16Stream stream)
        {
            var buffer = new byte[longSize];

            stream.Read(buffer, 0, longSize);
            if (BitConverter.IsLittleEndian)
            {
                return (long)((long)buffer[0] << 56 | (long)buffer[1] << 48 | (long)buffer[2] << 40 | (long)buffer[3] << 32 | (long)buffer[4] << 24 | (long)buffer[5] << 16 | (long)buffer[6] << 8 | (long)buffer[7]);
            }
            else
            {
                return BitConverter.ToInt64(buffer, 0);
            }
        }

        private static float DeserializeFloat(Protocol16Stream stream)
        {
            var buffer = new byte[floatSize];

            stream.Read(buffer, 0, floatSize);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return BitConverter.ToSingle(buffer, 0);
        }

        private static double DeserializeDouble(Protocol16Stream stream)
        {
            var buffer = new byte[doubleSize];

            stream.Read(buffer, 0, doubleSize);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return BitConverter.ToDouble(buffer, 0);
        }

        private static string DeserializeString(Protocol16Stream stream)
        {
#warning DeserializeInteger???
            int stringSize = DeserializeShort(stream);
            if (stringSize == 0)
            {
                return string.Empty;
            }
            var buffer = new byte[stringSize];

            stream.Read(buffer, 0, stringSize);

            return Encoding.UTF8.GetString(buffer, 0, stringSize);
        }

        private static byte[] DeserializeByteArray(Protocol16Stream stream)
        {
            int arraySize = DeserializeInteger(stream);

            var buffer = new byte[arraySize];
            stream.Read(buffer, 0, arraySize);

            return buffer;
        }

        private static int[] DeserializeIntArray(Protocol16Stream stream)
        {
            int arraySize = DeserializeInteger(stream);

            var array = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                array[i] = DeserializeInteger(stream);
            }

            return array;
        }

        private static string[] DeserializeStringArray(Protocol16Stream stream)
        {
#warning DeserializeInteger???
            int arraySize = DeserializeShort(stream);

            var array = new string[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                array[i] = DeserializeString(stream);
            }

            return array;
        }

        private static object[] DeserializeObjectArray(Protocol16Stream stream)
        {
            int arraySize = DeserializeShort(stream);

            var array = new object[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                byte typeCode = (byte)stream.ReadByte();
                array[i] = Deserialize(stream, typeCode);
            }

            return array;
        }

        private static IDictionary DeserializeDictionary(Protocol16Stream stream)
        {
            byte keyTypeCode = (byte)stream.ReadByte();
            byte valueTypeCode = (byte)stream.ReadByte();
            int dictionarySize = DeserializeShort(stream);
            Type keyType = GetTypeOfCode(keyTypeCode);
            Type valueType = GetTypeOfCode(valueTypeCode);
            Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(new Type[]
            {
                keyType,
                valueType
            });

            IDictionary dictionary = Activator.CreateInstance(dictionaryType) as IDictionary;
            for (int i = 0; i < dictionarySize; i++)
            {
                object key = Deserialize(stream, (keyTypeCode == 0 || keyTypeCode == 42) ? ((byte)stream.ReadByte()) : keyTypeCode);
                object value = Deserialize(stream, (valueTypeCode == 0 || valueTypeCode == 42) ? ((byte)stream.ReadByte()) : valueTypeCode);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private static bool DeserializeDictionaryArray(Protocol16Stream stream, short size, out Array array)
        {
            Type type = DeserializeDictionaryType(stream, out byte keyTypeCode, out byte valueTypeCode);
            array = Array.CreateInstance(type, size);
            for (int i = 0; i < size; i++)
            {
                if (!(Activator.CreateInstance(type) is IDictionary dictionary))
                {
                    return false;
                }

                //int dictionarySize = DeserializeShort(stream);
                //for (int j = 0; j < dictionarySize; j++)
                //{
                //    object key;
                //    if (keyTypeCode > 0)
                //    {
                //        key = Deserialize(stream, keyTypeCode);
                //    }
                //    else
                //    {
                //        byte nextKeyTypeCode = (byte)stream.ReadByte();
                //        key = Deserialize(stream, nextKeyTypeCode);
                //    }

                //    object value;
                //    if (valueTypeCode > 0)
                //    {
                //        value = Deserialize(stream, valueTypeCode);
                //    }
                //    else
                //    {
                //        byte nextValueTypeCode = (byte)stream.ReadByte();
                //        value = Deserialize(stream, nextValueTypeCode);
                //    }

                //    dictionary.Add(key, value);
                //}
                array.SetValue(DeserializeDictionary(stream), i);
            }

            return true;
        }

        private static Type DeserializeDictionaryType(Protocol16Stream stream, out byte keyTypeCode, out byte valueTypeCode)
        {
            keyTypeCode = (byte)stream.ReadByte();
            valueTypeCode = (byte)stream.ReadByte();
            Type keyType = GetTypeOfCode(keyTypeCode);
            Type valueType = GetTypeOfCode(valueTypeCode);

            return typeof(IDictionary<,>).MakeGenericType(new Type[]
            {
                keyType,
                valueType
            });
        }

        private static Array DeserializeArray(Protocol16Stream stream)
        {
            int arraySize = DeserializeShort(stream);
            byte typeCode = (byte)stream.ReadByte();

            switch ((Protocol16Type)typeCode)
            {
                case Protocol16Type.Array:
                    {
                        Array array = DeserializeArray(stream);
                        Type typeArray = array.GetType();
                        Array result = Array.CreateInstance(typeArray, arraySize);
                        result.SetValue(array, 0);
                        for (int i = 0; i < arraySize; i++)
                        {
                            array = DeserializeArray(stream);
                            result.SetValue(array, 0);
                        }

                        return result;
                    }
                case Protocol16Type.ByteArray:
                    {
                        Array result = Array.CreateInstance(typeof(byte[]), arraySize);
                        for (int i = 0; i < arraySize; i++)
                        {
                            Array array = DeserializeByteArray(stream);
                            result.SetValue(array, i);
                        }

                        return result;
                    }
                case Protocol16Type.Dictionary:
                    {
                        DeserializeDictionaryArray(stream, (short)arraySize, out Array result);

                        return result;
                    }
                default:
                    {
                        Array result = CreateArrayByType(typeCode, (short)arraySize);
                        for (int i = 0; i < arraySize; i++)
                        {
                            result.SetValue(Deserialize(stream, typeCode), i);
                        }

                        return result;
                    }
            }
        }

        private static Array CreateArrayByType(byte typeCode, short length)
        {
            return Array.CreateInstance(GetTypeOfCode(typeCode), length);
        }

        private static Dictionary<byte, object> DeserializeParameterTable(Protocol16Stream stream)
        {
            int dicitonarySize = DeserializeShort(stream);
            var dictionary = new Dictionary<byte, object>(dicitonarySize);
            for (int i = 0; i < dicitonarySize; i++)
            {
                byte key = (byte)stream.ReadByte();
                byte valueTypeCode = (byte)stream.ReadByte();
                object value = Deserialize(stream, valueTypeCode);
                dictionary[key] = value;
            }

            return dictionary;
        }
        #endregion
    }
}
