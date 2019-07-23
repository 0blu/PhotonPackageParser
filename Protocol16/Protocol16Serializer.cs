using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Protocol16
{
    public static class Protocol16Serializer
    {
        private static readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;
        private static readonly ArrayPool<long> _poolLong = ArrayPool<long>.Shared;
        private static readonly ArrayPool<float> _poolFloat = ArrayPool<float>.Shared;
        private static readonly ArrayPool<double> _poolDouble = ArrayPool<double>.Shared;

        public static void Serialize(Protocol16Stream output, object obj, bool writeTypeCode)
        {
            if (obj == null)
            {
                output.WriteTypeCodeIfTrue(Protocol16Type.Null, writeTypeCode);
                return;
            }

            switch (TypeCodeToProtocol16Type(obj.GetType()))
            {
                case Protocol16Type.Boolean:
                    SerializeBoolean(output, (bool)obj, writeTypeCode);
                    return;
                case Protocol16Type.Byte:
                    SerializeByte(output, (byte)obj, writeTypeCode);
                    return;
                case Protocol16Type.Short:
                    SerializeShort(output, (short)obj, writeTypeCode);
                    return;
                case Protocol16Type.Integer:
                    SerializeInteger(output, (int)obj, writeTypeCode);
                    return;
                case Protocol16Type.Long:
                    SerializeLong(output, (long)obj, writeTypeCode);
                    return;
                case Protocol16Type.Float:
                    SerializeFloat(output, (float)obj, writeTypeCode);
                    return;
                case Protocol16Type.Double:
                    SerializeDouble(output, (double)obj, writeTypeCode);
                    return;
                case Protocol16Type.IntegerArray:
                    SerializeIntArray(output, (int[])obj, writeTypeCode);
                    return;
                case Protocol16Type.String:
                    SerializeString(output, (string)obj, writeTypeCode);
                    return;
                case Protocol16Type.EventData:
                    SerializeEventData(output, (EventData)obj, writeTypeCode);
                    return;
                case Protocol16Type.Dictionary:
                    SerializeDictionary(output, (IDictionary)obj, writeTypeCode);
                    return;
                case Protocol16Type.StringArray:
                    SerializeStringArray(output, (string[])obj, writeTypeCode);
                    return;
                case Protocol16Type.OperationResponse:
                    SerializeOperationResponse(output, (OperationResponse)obj, writeTypeCode);
                    return;
                case Protocol16Type.OperationRequest:
                    SerializeOperationRequest(output, (OperationRequest)obj, writeTypeCode);
                    return;
                case Protocol16Type.ByteArray:
                    SerializeByteArray(output, (byte[])obj, writeTypeCode);
                    return;
                case Protocol16Type.Array:
                    SerializeArray(output, (Array)obj, writeTypeCode);
                    return;
                case Protocol16Type.ObjectArray:
                    SerializeObjectArray(output, (object[])obj, writeTypeCode);
                    return;
            }

            throw new ArgumentException($"Cannot serialize objects of type {obj.GetType()} / System.TypeCode: {Type.GetTypeCode(obj.GetType())}");
        }

        private static void SerializeBoolean(Protocol16Stream output, bool value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Boolean, writeTypeCode);
            output.WriteByte(value ? (byte)1 : (byte)0);
        }

        private static void SerializeByte(Protocol16Stream output, byte value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Byte, writeTypeCode);
            output.WriteByte(value);
        }

        private static void SerializeShort(Protocol16Stream output, short value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Short, writeTypeCode);
            byte[] buffer = _pool.Rent(sizeof(short));
            buffer[0] = (byte)(value >> 8);
            buffer[1] = (byte)(value);
            output.Write(buffer, 0, sizeof(short));
            _pool.Return(buffer);
        }

        private static void SerializeInteger(Protocol16Stream output, int value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Integer, writeTypeCode);
            byte[] buffer = _pool.Rent(sizeof(int));
            buffer[0] = (byte)(value >> 24);
            buffer[1] = (byte)(value >> 16);
            buffer[2] = (byte)(value >> 8);
            buffer[3] = (byte)(value);
            output.Write(buffer, 0, sizeof(int));
            _pool.Return(buffer);
        }

        private static void SerializeLong(Protocol16Stream output, long value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Long, writeTypeCode);
            long[] longBuffer = _poolLong.Rent(1);
            longBuffer[0] = value;
            byte[] buffer = _pool.Rent(sizeof(long));
            Buffer.BlockCopy(longBuffer, 0, buffer, 0, sizeof(long));
            _poolLong.Return(longBuffer);
            if (BitConverter.IsLittleEndian)
            {
                byte b0 = buffer[0];
                byte b1 = buffer[1];
                byte b2 = buffer[2];
                byte b3 = buffer[3];
                buffer[0] = buffer[7];
                buffer[1] = buffer[6];
                buffer[2] = buffer[5];
                buffer[3] = buffer[4];
                buffer[4] = b3;
                buffer[5] = b2;
                buffer[6] = b1;
                buffer[7] = b0;
            }
            output.Write(buffer, 0, sizeof(long));
            _pool.Return(buffer);
        }

        private static void SerializeFloat(Protocol16Stream output, float value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Float, writeTypeCode);
            float[] floatBuffer = _poolFloat.Rent(1);
            floatBuffer[0] = value;
            byte[] buffer = _pool.Rent(sizeof(float));
            Buffer.BlockCopy(floatBuffer, 0, buffer, 0, sizeof(float));
            _poolFloat.Return(floatBuffer);
            if (BitConverter.IsLittleEndian)
            {
                byte b0 = buffer[0];
                byte b1 = buffer[1];
                buffer[0] = buffer[7];
                buffer[1] = buffer[6];
                buffer[2] = b1;
                buffer[3] = b0;
            }
            output.Write(buffer, 0, sizeof(float));
            _pool.Return(buffer);
        }

        private static void SerializeDouble(Protocol16Stream output, double value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Double, writeTypeCode);
            double[] doubleBuffer = _poolDouble.Rent(1);
            doubleBuffer[0] = value;
            byte[] buffer = _pool.Rent(sizeof(double));
            Buffer.BlockCopy(doubleBuffer, 0, buffer, 0, sizeof(double));
            _poolDouble.Return(doubleBuffer);
            if (BitConverter.IsLittleEndian)
            {
                byte b0 = buffer[0];
                byte b1 = buffer[1];
                byte b2 = buffer[2];
                byte b3 = buffer[3];
                buffer[0] = buffer[7];
                buffer[1] = buffer[6];
                buffer[2] = buffer[5];
                buffer[3] = buffer[4];
                buffer[4] = b3;
                buffer[5] = b2;
                buffer[6] = b1;
                buffer[7] = b0;
            }
            output.Write(buffer, 0, sizeof(double));
            _pool.Return(buffer);
        }

        private static void SerializeIntArray(Protocol16Stream output, int[] ints, bool writeTypeCode)
        {
            if (ints.Length > short.MaxValue)
            {
                throw new NotSupportedException($"int[] can only have a maximum size of {short.MaxValue} (short.MaxValue). Yours is: {ints.Length}");
            }
            output.WriteTypeCodeIfTrue(Protocol16Type.Array, writeTypeCode);
            SerializeShort(output, (short)ints.Length, false);
            output.WriteTypeCodeIfTrue(Protocol16Type.Integer, true);
            byte[] array = _pool.Rent(ints.Length * sizeof(int));
            int num = 0;
            for (int i = 0; i < ints.Length; i++)
            {
                array[num++] = (byte)(ints[i] >> 24);
                array[num++] = (byte)(ints[i] >> 16);
                array[num++] = (byte)(ints[i] >> 8);
                array[num++] = (byte)(ints[i]);
            }
            output.Write(array, 0, array.Length);
            _pool.Return(array);
        }

        private static void SerializeString(Protocol16Stream output, string value, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.String, writeTypeCode);
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            if (bytes.Length > short.MaxValue)
            {
                throw new NotSupportedException($"Strings that exceed a UTF8-encoded byte-length of {short.MaxValue} (short.MaxValue) are not supported. Yours is: {bytes.Length}");
            }
            SerializeShort(output, (short)bytes.Length, false);
            output.Write(bytes, 0, bytes.Length);
        }
        
        private static void SerializeStringArray(Protocol16Stream output, string[] strings, bool writeTypeCode)
        {
            if (strings.Length > short.MaxValue)
            {
                throw new NotSupportedException($"string[] can only have a maximum size of {short.MaxValue} (short.MaxValue). Yours is: {strings.Length}");
            }
            output.WriteTypeCodeIfTrue(Protocol16Type.StringArray, writeTypeCode);
            SerializeShort(output, (short)strings.Length, false);
            foreach (var s in strings)
            {
                SerializeString(output, s, writeTypeCode);
            }
        }

        private static void SerializeEventData(Protocol16Stream output, EventData data, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.EventData, writeTypeCode);
            output.WriteByte(data.Code);
            SerializeParameterTable(output, data.Parameters);
        }

        private static void SerializeOperationResponse(Protocol16Stream output, OperationResponse data, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.OperationResponse, writeTypeCode);
            output.WriteByte(data.OperationCode);
            SerializeShort(output, data.ReturnCode, false);
            if (string.IsNullOrEmpty(data.DebugMessage))
            {
                output.WriteTypeCodeIfTrue(Protocol16Type.Null, true);
            }
            else
            {
                SerializeString(output, data.DebugMessage, false);
            }
            SerializeParameterTable(output, data.Parameters);
        }

        private static void SerializeOperationRequest(Protocol16Stream output, OperationRequest data, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.OperationRequest, writeTypeCode);
            output.WriteByte(data.OperationCode);
            SerializeParameterTable(output, data.Parameters);
        }

        private static void SerializeParameterTable(Protocol16Stream output, Dictionary<byte, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                SerializeShort(output, 0, false);
                return;
            }

            SerializeShort(output, (short)parameters.Count, false);
            foreach (KeyValuePair<byte, object> keyValuePair in parameters)
            {
                output.WriteByte(keyValuePair.Key);
                Serialize(output, keyValuePair.Value, true);
            }
        }

        private static void SerializeByteArray(Protocol16Stream output, byte[] obj, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.ByteArray, writeTypeCode);
            SerializeInteger(output, obj.Length, false);
            output.Write(obj, 0, obj.Length);
        }

        private static void SerializeArray(Protocol16Stream output, Array array, bool writeTypeCode)
        {
            if (array.Length > short.MaxValue)
            {
                throw new NotSupportedException($"Arrays can only have a maximum size of {short.MaxValue} (short.MaxValue). Yours is: {array.Length}");
            }
            output.WriteTypeCodeIfTrue(Protocol16Type.Array, writeTypeCode);
            SerializeShort(output, (short)array.Length, writeTypeCode);

            Type elementType = array.GetType().GetElementType();

            Protocol16Type protocol16Type = TypeCodeToProtocol16Type(elementType);
            if (protocol16Type == Protocol16Type.Unknown)
            {
                throw new Exception("Custom types are currently not supported");
            }
            output.WriteTypeCodeIfTrue(protocol16Type, true);
            if (protocol16Type == Protocol16Type.Dictionary)
            {
                // WTF ExitGames. Why are you trying to get GetGenericArguments() of an array..?!
                // I think what you wanted to do is just giving the element type to SerializeDictionaryHeader not the array type
                // TODO: need to test this
                SerializeDictionaryHeader(output, (IDictionary)array, out var writeKeyCode, out var writeValueCode);
                foreach (object o in array)
                {
                    SerializeDictionaryElements(output, (IDictionary) o, writeKeyCode, writeValueCode);
                }
            }
            else
            {
                foreach (object o in array)
                {
                    Serialize(output, o, false);
                }
            }
        }

        private static void SerializeObjectArray(Protocol16Stream output, object[] objects, bool writeTypeCode)
        {
            if (objects.Length > short.MaxValue)
            {
                throw new NotSupportedException($"objects[] can only have a maximum size of {short.MaxValue} (short.MaxValue). Yours is: {objects.Length}");
            }
            output.WriteTypeCodeIfTrue(Protocol16Type.ObjectArray, writeTypeCode);
            SerializeShort(output, (short)objects.Length, false);
            foreach (var s in objects)
            {
                Serialize(output, s, true);
            }
        }
        
        private static void SerializeDictionary(Protocol16Stream output, IDictionary data, bool writeTypeCode)
        {
            output.WriteTypeCodeIfTrue(Protocol16Type.Dictionary, writeTypeCode);

            SerializeDictionaryHeader(output, data, out var writeKeyCode, out var writeValueCode);
            SerializeDictionaryElements(output, data, writeKeyCode, writeValueCode);
        }

        private static void SerializeDictionaryHeader(Protocol16Stream output, IDictionary data, out bool writeKeyCode, out bool writeValueCode)
        {
            Type[] genericArguments = data.GetType().GetGenericArguments();
            writeKeyCode = (genericArguments[0] == typeof(object));
            writeValueCode = (genericArguments[1] == typeof(object));
            if (writeKeyCode)
            {
                output.WriteTypeCodeIfTrue(Protocol16Type.Unknown, true);
            }
            else
            {
                Protocol16Type typeOfKey = TypeCodeToProtocol16Type(genericArguments[0]);
                if (typeOfKey == Protocol16Type.Unknown || typeOfKey == Protocol16Type.Dictionary)
                {
                    throw new Exception("Unexpected - cannot serialize Dictionary with key type: " + genericArguments[0]);
                }
                output.WriteTypeCodeIfTrue(typeOfKey, true);
            }
            if (writeValueCode)
            {
                output.WriteTypeCodeIfTrue(Protocol16Type.Unknown, true);
            }
            else
            {
                Protocol16Type typeOfValue = TypeCodeToProtocol16Type(genericArguments[1]);
                if (typeOfValue == Protocol16Type.Unknown)
                {
                    throw new Exception("Unexpected - cannot serialize Dictionary with value type: " + genericArguments[0]);
                }
                output.WriteTypeCodeIfTrue(typeOfValue, true);
                if (typeOfValue == Protocol16Type.Dictionary)
                {
                    throw new Exception("TODO: Nested Dictionaries");
                }
            }
        }

        private static void SerializeDictionaryElements(Protocol16Stream output, IDictionary data, bool writeKeyCode, bool writeValueCode)
        {
            if (data.Count > short.MaxValue)
            {
                throw new NotSupportedException($"Dictionaries can only have a maximum size of {short.MaxValue} (short.MaxValue). Yours is: {data.Count}");
            }
            SerializeShort(output, (short)data.Count, false);

            foreach (DictionaryEntry entry in data)
            {
                if (!writeKeyCode && entry.Key == null)
                {
                    throw new Exception("This should never happen. Cannot serialize the null(key) object in Dictionary when writing the key code is disabled.");
                }

                if (!writeValueCode && entry.Value == null)
                {
                    throw new Exception("This should never happen. Cannot serialize the null(value) object in Dictionary when writing the value code is disabled.");
                }
                Serialize(output, entry.Key, writeKeyCode);
                Serialize(output, entry.Value, writeValueCode);
            }
        }


        private static Protocol16Type TypeCodeToProtocol16Type(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return Protocol16Type.Boolean;
                case TypeCode.Byte:
                    return Protocol16Type.Byte;
                case TypeCode.Int16:
                    return Protocol16Type.Short;
                case TypeCode.Int32:
                    return Protocol16Type.Integer;
                case TypeCode.Int64:
                    return Protocol16Type.Long;
                case TypeCode.Single:
                    return Protocol16Type.Float;
                case TypeCode.String:
                    return Protocol16Type.String;
            }

            if (type.IsArray)
            {
                Type elementType = type.GetElementType();

                if (elementType == typeof(byte))
                {
                    return Protocol16Type.ByteArray;
                }

                if (elementType == typeof(string))
                {
                    return Protocol16Type.StringArray;
                }

                if (elementType == typeof(int))
                {
                    return Protocol16Type.IntegerArray;
                }

                return Protocol16Type.Array;
            }

            if (type == typeof(Hashtable))
            {
                throw new NotSupportedException("Hashtables are currently not supported");
            }

            if (type == typeof(ArraySegment<byte>))
            {
                throw new NotSupportedException("Byte array segment are currently not supported");
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return Protocol16Type.Dictionary;
            }

            if (type == typeof(EventData))
            {
                return Protocol16Type.EventData;
            }

            if (type == typeof(OperationRequest))
            {
                return Protocol16Type.OperationRequest;
            }

            if (type == typeof(OperationResponse))
            {
                return Protocol16Type.OperationResponse;
            }

            return Protocol16Type.Unknown;
        }
    }
}
