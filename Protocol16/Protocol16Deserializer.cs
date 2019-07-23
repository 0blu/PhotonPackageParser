using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Protocol16
{
    public static class Protocol16Deserializer
    {
        private static readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;
        #region methods
        public static object Deserialize(Protocol16Stream input, byte typeCode)
        {
            switch ((Protocol16Type)typeCode)
            {
                case Protocol16Type.Unknown:
                case Protocol16Type.Null:
                    return null;
                case Protocol16Type.Dictionary:
                    return DeserializeDictionary(input);
                case Protocol16Type.StringArray:
                    return DeserializeStringArray(input);
                case Protocol16Type.Byte:
                    return DeserializeByte(input);
                case Protocol16Type.Double:
                    return DeserializeDouble(input);
                case Protocol16Type.EventData:
                    return DeserializeEventData(input);
                case Protocol16Type.Float:
                    return DeserializeFloat(input);
                case Protocol16Type.Integer:
                    return DeserializeInteger(input);
                case Protocol16Type.Short:
                    return DeserializeShort(input);
                case Protocol16Type.Long:
                    return DeserializeLong(input);
                case Protocol16Type.IntegerArray:
                    return DeserializeIntArray(input);
                case Protocol16Type.Boolean:
                    return DeserializeBoolean(input);
                case Protocol16Type.OperationResponse:
                    return DeserializeOperationResponse(input);
                case Protocol16Type.OperationRequest:
                    return DeserializeOperationRequest(input);
                case Protocol16Type.String:
                    return DeserializeString(input);
                case Protocol16Type.ByteArray:
                    return DeserializeByteArray(input);
                case Protocol16Type.Array:
                    return DeserializeArray(input);
                case Protocol16Type.ObjectArray:
                    return DeserializeObjectArray(input);
                default:
                    throw new ArgumentException($"Type code: {typeCode} not implemented.");
            }
        }

        public static OperationRequest DeserializeOperationRequest(Protocol16Stream input)
        {
            byte operationCode = DeserializeByte(input);
            Dictionary<byte, object> parameters = DeserializeParameterTable(input);

            return new OperationRequest(operationCode, parameters);
        }

        public static OperationResponse DeserializeOperationResponse(Protocol16Stream input)
        {
            byte operationCode = DeserializeByte(input);
            short returnCode = DeserializeShort(input);
            string debugMessage = (Deserialize(input, DeserializeByte(input)) as string);
            Dictionary<byte, object> parameters = DeserializeParameterTable(input);

            return new OperationResponse(operationCode, returnCode, debugMessage, parameters);
        }

        public static EventData DeserializeEventData(Protocol16Stream input)
        {
            byte code = DeserializeByte(input);
            Dictionary<byte, object> parameters = DeserializeParameterTable(input);

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

        private static bool DeserializeBoolean(Protocol16Stream input)
        {
            return input.ReadByte() != 0;
        }

        public static short DeserializeShort(Protocol16Stream input)
        {
            var buffer = _pool.Rent(sizeof(short));
            input.Read(buffer, 0, buffer.Length);

            short result = (short) (buffer[0] << 8 | buffer[1]);

            _pool.Return(buffer);

            return result;
        }

        private static int DeserializeInteger(Protocol16Stream input)
        {
            var buffer = _pool.Rent(sizeof(int));
            input.Read(buffer, 0, sizeof(int));

            int result = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];

            _pool.Return(buffer);
            return result;
        }

        private static long DeserializeLong(Protocol16Stream input)
        {
            var buffer = _pool.Rent(sizeof(long));
            input.Read(buffer, 0, sizeof(long));

            long result;
            if (BitConverter.IsLittleEndian)
            {
                result = (long)buffer[0] << 56 | (long)buffer[1] << 48 | (long)buffer[2] << 40 | (long)buffer[3] << 32 | (long)buffer[4] << 24 | (long)buffer[5] << 16 | (long)buffer[6] << 8 | buffer[7];
            }
            else
            {
                result = BitConverter.ToInt64(buffer, 0);
            }

            _pool.Return(buffer);

            return result;
        }

        private static float DeserializeFloat(Protocol16Stream input)
        {
            var buffer = _pool.Rent(sizeof(float));
            input.Read(buffer, 0, sizeof(float));

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            float result = BitConverter.ToSingle(buffer, 0);

            _pool.Return(buffer);

            return result;
        }

        private static double DeserializeDouble(Protocol16Stream input)
        {
            var buffer = _pool.Rent(sizeof(double));
            input.Read(buffer, 0, sizeof(double));

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            double result = BitConverter.ToDouble(buffer, 0);

            _pool.Return(buffer);

            return result;
        }

        private static string DeserializeString(Protocol16Stream input)
        {
            int stringSize = DeserializeShort(input);
            if (stringSize == 0)
            {
                return string.Empty;
            }
            
            var buffer = _pool.Rent(stringSize);

            input.Read(buffer, 0, stringSize);

            string result = Encoding.UTF8.GetString(buffer, 0, stringSize);

            _pool.Return(buffer);

            return result;
        }

        private static byte[] DeserializeByteArray(Protocol16Stream input)
        {
            int arraySize = DeserializeInteger(input);

            var buffer = new byte[arraySize];
            input.Read(buffer, 0, arraySize);

            return buffer;
        }

        private static int[] DeserializeIntArray(Protocol16Stream input)
        {
            int arraySize = DeserializeInteger(input);

            var array = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                array[i] = DeserializeInteger(input);
            }

            return array;
        }

        private static string[] DeserializeStringArray(Protocol16Stream input)
        {
            int arraySize = DeserializeShort(input);

            var array = new string[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                array[i] = DeserializeString(input);
            }

            return array;
        }

        private static object[] DeserializeObjectArray(Protocol16Stream input)
        {
            int arraySize = DeserializeShort(input);

            var array = new object[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                byte typeCode = (byte)input.ReadByte();
                array[i] = Deserialize(input, typeCode);
            }

            return array;
        }

        private static IDictionary DeserializeDictionary(Protocol16Stream input)
        {
            byte keyTypeCode = (byte)input.ReadByte();
            byte valueTypeCode = (byte)input.ReadByte();
            int dictionarySize = DeserializeShort(input);
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
                object key = Deserialize(input, (keyTypeCode == 0 || keyTypeCode == 42) ? ((byte)input.ReadByte()) : keyTypeCode);
                object value = Deserialize(input, (valueTypeCode == 0 || valueTypeCode == 42) ? ((byte)input.ReadByte()) : valueTypeCode);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private static bool DeserializeDictionaryArray(Protocol16Stream input, short size, out Array result)
        {
            Type type = DeserializeDictionaryType(input, out byte keyTypeCode, out byte valueTypeCode);
            result = Array.CreateInstance(type, size);

            for (short i = 0; i < size; i += 1)
            {
                if (!(Activator.CreateInstance(type) is IDictionary dictionary))
                {
                    return false;
                }
                short arraySize = DeserializeShort(input);
                for (int j = 0; j < arraySize; j++)
                {
                    object key;
                    if (keyTypeCode > 0)
                    {
                        key = Deserialize(input, keyTypeCode);
                    }
                    else
                    {
                        byte nextKeyTypeCode = (byte)input.ReadByte();
                        key = Deserialize(input, nextKeyTypeCode);
                    }
                    object value;
                    if (valueTypeCode > 0)
                    {
                        value = Deserialize(input, valueTypeCode);
                    }
                    else
                    {
                        byte nextValueTypeCode = (byte)input.ReadByte();
                        value = Deserialize(input, nextValueTypeCode);
                    }
                    dictionary.Add(key, value);
                }
                result.SetValue(dictionary, i);
            }

            return true;
        }

        private static Array DeserializeArray(Protocol16Stream input)
        {
            short size = DeserializeShort(input);
            byte typeCode = (byte)input.ReadByte();
            switch ((Protocol16Type)typeCode)
            {
                case Protocol16Type.Array:
                    {
                        Array array = DeserializeArray(input);
                        Type arrayType = array.GetType();
                        Array result = Array.CreateInstance(arrayType, size);
                        result.SetValue(array, 0);
                        for (short i = 1; i < size; i += 1)
                        {
                            array = DeserializeArray(input);
                            result.SetValue(array, i);
                        }

                        return result;
                    }
                case Protocol16Type.ByteArray:
                    {
                        Array result = Array.CreateInstance(typeof(byte[]), size);
                        for (short i = 0; i < size; i += 1)
                        {
                            Array value = DeserializeByteArray(input);
                            result.SetValue(value, i);
                        }

                        return result;
                    }
                case Protocol16Type.Dictionary:
                    {
                        DeserializeDictionaryArray(input, size, out Array result);

                        return result;
                    }
                default:
                    {
                        Type arrayType = GetTypeOfCode(typeCode);
                        Array result = Array.CreateInstance(arrayType, size);

                        for (short i = 0; i < size; i += 1)
                        {
                            result.SetValue(Deserialize(input, typeCode), i);
                        }

                        return result;
                    }
            }
        }

        private static Type DeserializeDictionaryType(Protocol16Stream input, out byte keyTypeCode, out byte valueTypeCode)
        {
            keyTypeCode = (byte)input.ReadByte();
            valueTypeCode = (byte)input.ReadByte();
            Type keyType = GetTypeOfCode(keyTypeCode);
            Type valueType = GetTypeOfCode(valueTypeCode);

            return typeof(Dictionary<,>).MakeGenericType(new Type[]
            {
                keyType,
                valueType
            });
        }

        private static Dictionary<byte, object> DeserializeParameterTable(Protocol16Stream input)
        {
            int dictionarySize = DeserializeShort(input);
            var dictionary = new Dictionary<byte, object>(dictionarySize);
            for (int i = 0; i < dictionarySize; i++)
            {
                byte key = (byte)input.ReadByte();
                byte valueTypeCode = (byte)input.ReadByte();
                object value = Deserialize(input, valueTypeCode);
                dictionary[key] = value;
            }

            return dictionary;
        }
        #endregion
    }
}
