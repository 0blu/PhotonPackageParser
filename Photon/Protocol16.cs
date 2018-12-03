using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Photon
{
    public class Protocol16
    {
        #region fields
        private readonly byte[] memShort = new byte[2];
        private readonly byte[] memInteger = new byte[4];
        private readonly byte[] memLong = new byte[8];
        private readonly byte[] memFloat = new byte[4];
        private readonly byte[] memDouble = new byte[8];
        private byte[] memString;
        #endregion

        private Type GetTypeOfCode(byte typeCode)
        {
            if (typeCode <= 42)
            {
                if (typeCode == 0 || typeCode == 42)
                {
                    return typeof(object);
                }
            }
            else
            {
                if (typeCode == 68)
                {
                    return typeof(IDictionary);
                }
                switch (typeCode)
                {
                    case 97:
                        return typeof(string[]);
                    case 98:
                        return typeof(byte);
                    case 100:
                        return typeof(double);
                    case 101:
                        return typeof(EventData);
                    case 102:
                        return typeof(float);
                    case 105:
                        return typeof(int);
                    case 107:
                        return typeof(short);
                    case 108:
                        return typeof(long);
                    case 110:
                        return typeof(int[]);
                    case 111:
                        return typeof(bool);
                    case 112:
                        return typeof(OperationResponse);
                    case 113:
                        return typeof(OperationRequest);
                    case 115:
                        return typeof(string);
                    case 120:
                        return typeof(byte[]);
                    case 121:
                        return typeof(Array);
                    case 122:
                        return typeof(object[]);
                }
            }

            throw new Exception($"GetTypeOfCode() typeCode: {typeCode}");
        }

        private Array CreateArrayByType(byte arrayType, short length)
        {
            return Array.CreateInstance(GetTypeOfCode(arrayType), (int)length);
        }

        public OperationRequest DeserializeOperationRequest(StreamBuffer din)
        {
            byte operationCode = DeserializeByte(din);
            Dictionary<byte, object> parameters = DeserializeParameterTable(din);

            return new OperationRequest(operationCode, parameters);
        }

        public OperationResponse DeserializeOperationResponse(StreamBuffer stream)
        {
            byte operationCode = DeserializeByte(stream);
            short returnCode = DeserializeShort(stream);
            string debugMessage = (Deserialize(stream, DeserializeByte(stream)) as string);
            Dictionary<byte, object> parameters = DeserializeParameterTable(stream);

            return new OperationResponse(operationCode, returnCode, debugMessage, parameters);
        }

        public EventData DeserializeEventData(StreamBuffer din)
        {
            byte code = DeserializeByte(din);
            Dictionary<byte, object> parameters = DeserializeParameterTable(din);

            return new EventData(code, parameters);
        }

        private Dictionary<byte, object> DeserializeParameterTable(StreamBuffer stream)
        {
            short num = DeserializeShort(stream);
            Dictionary<byte, object> dictionary = new Dictionary<byte, object>((int)num);
            for (int i = 0; i < (int)num; i++)
            {
                byte key = (byte)stream.ReadByte();
                object value = Deserialize(stream, (byte)stream.ReadByte());
                dictionary[key] = value;
            }

            return dictionary;
        }

        public object Deserialize(StreamBuffer din, byte type)
        {
            if (type <= 42)
            {
                if (type == 0 || type == 42)
                {
                    return null;
                }
            }
            else
            {
                if (type == 68)
                {
                    return DeserializeDictionary(din);
                }
                switch (type)
                {
                    case 97:
                        return DeserializeStringArray(din);
                    case 98:
                        return DeserializeByte(din);
                    case 100:
                        return DeserializeDouble(din);
                    case 101:
                        return DeserializeEventData(din);
                    case 102:
                        return DeserializeFloat(din);
                    case 105:
                        return DeserializeInteger(din);
                    case 107:
                        return DeserializeShort(din);
                    case 108:
                        return DeserializeLong(din);
                    case 110:
                        return DeserializeIntArray(din);
                    case 111:
                        return DeserializeBoolean(din);
                    case 112:
                        return DeserializeOperationResponse(din);
                    case 113:
                        return DeserializeOperationRequest(din);
                    case 115:
                        return DeserializeString(din);
                    case 120:
                        return DeserializeByteArray(din);
                    case 121:
                        return DeserializeArray(din);
                    case 122:
                        return DeserializeObjectArray(din);
                }
            }

            throw new Exception($"Deserialize() type: {type}");
        }

        public byte DeserializeByte(StreamBuffer din)
        {
            return (byte)din.ReadByte();
        }

        private bool DeserializeBoolean(StreamBuffer din)
        {
            return din.ReadByte() != 0;
        }

        public short DeserializeShort(StreamBuffer din)
        {
            short result;

            byte[] obj = memShort;
            lock (obj)
            {
                byte[] array = memShort;
                din.Read(array, 0, 2);
                result = (short)((int)array[0] << 8 | (int)array[1]);
            }

            return result;
        }

        private int DeserializeInteger(StreamBuffer din)
        {
            int result;

            byte[] obj = memInteger;
            lock (obj)
            {
                byte[] array = memInteger;
                din.Read(array, 0, 4);
                result = ((int)array[0] << 24 | (int)array[1] << 16 | (int)array[2] << 8 | (int)array[3]);
            }

            return result;
        }

        private long DeserializeLong(StreamBuffer din)
        {
            long result;

            byte[] obj = memLong;
            lock (obj)
            {
                byte[] array = memLong;
                din.Read(array, 0, 8);
                bool isLittleEndian = BitConverter.IsLittleEndian;
                if (isLittleEndian)
                {
                    result = (long)((ulong)array[0] << 56 | (ulong)array[1] << 48 | (ulong)array[2] << 40 | (ulong)array[3] << 32 | (ulong)array[4] << 24 | (ulong)array[5] << 16 | (ulong)array[6] << 8 | (ulong)array[7]);
                }
                else
                {
                    result = BitConverter.ToInt64(array, 0);
                }
            }

            return result;
        }

        private float DeserializeFloat(StreamBuffer din)
        {
            float result;

            byte[] obj = memFloat;
            lock (obj)
            {
                byte[] array = memFloat;
                din.Read(array, 0, 4);
                bool isLittleEndian = BitConverter.IsLittleEndian;
                if (isLittleEndian)
                {
                    byte b = array[0];
                    byte b2 = array[1];
                    array[0] = array[3];
                    array[1] = array[2];
                    array[2] = b2;
                    array[3] = b;
                }
                result = BitConverter.ToSingle(array, 0);
            }

            return result;
        }

        private double DeserializeDouble(StreamBuffer din)
        {
            double result;

            byte[] obj = memDouble;
            lock (obj)
            {
                byte[] array = memDouble;
                din.Read(array, 0, 8);
                bool isLittleEndian = BitConverter.IsLittleEndian;
                if (isLittleEndian)
                {
                    byte b = array[0];
                    byte b2 = array[1];
                    byte b3 = array[2];
                    byte b4 = array[3];
                    array[0] = array[7];
                    array[1] = array[6];
                    array[2] = array[5];
                    array[3] = array[4];
                    array[4] = b4;
                    array[5] = b3;
                    array[6] = b2;
                    array[7] = b;
                }
                result = BitConverter.ToDouble(array, 0);
            }

            return result;
        }

        private string DeserializeString(StreamBuffer din)
        {
            string result;

            short num = DeserializeShort(din);
            if (num == 0)
            {
                result = string.Empty;
            }
            else
            {
                if (memString == null || memString.Length < (int)num)
                {
                    memString = new byte[(int)num];
                }
                din.Read(memString, 0, (int)num);
                result = Encoding.UTF8.GetString(memString, 0, (int)num);
            }

            return result;
        }

        private Array DeserializeArray(StreamBuffer din)
        {
            Array array2;

            short num = DeserializeShort(din);
            byte b = (byte)din.ReadByte();
            if (b == 121)
            {
                Array array = DeserializeArray(din);
                Type type = array.GetType();
                array2 = Array.CreateInstance(type, (int)num);
                array2.SetValue(array, 0);
                for (short num2 = 1; num2 < num; num2 += 1)
                {
                    array = DeserializeArray(din);
                    array2.SetValue(array, (int)num2);
                }
            }
            else
            {
                if (b == 120)
                {
                    array2 = Array.CreateInstance(typeof(byte[]), (int)num);
                    for (short num3 = 0; num3 < num; num3 += 1)
                    {
                        Array value = DeserializeByteArray(din);
                        array2.SetValue(value, (int)num3);
                    }
                }
                else
                {
                    if (b == 68)
                    {
                        DeserializeDictionaryArray(din, num, out Array result);
                        return result;
                    }
                    array2 = CreateArrayByType(b, num);
                    for (short num5 = 0; num5 < num; num5 += 1)
                    {
                        array2.SetValue(Deserialize(din, b), (int)num5);
                    }
                }
            }

            return array2;
        }

        private byte[] DeserializeByteArray(StreamBuffer din)
        {
            int num = DeserializeInteger(din);
            byte[] array = new byte[num];
            din.Read(array, 0, num);

            return array;
        }

        private int[] DeserializeIntArray(StreamBuffer din)
        {
            int num = DeserializeInteger(din);
            int[] array = new int[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = DeserializeInteger(din);
            }

            return array;
        }

        private string[] DeserializeStringArray(StreamBuffer din)
        {
            int num = (int)DeserializeShort(din);
            string[] array = new string[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = DeserializeString(din);
            }

            return array;
        }

        private object[] DeserializeObjectArray(StreamBuffer din)
        {
            short num = DeserializeShort(din);
            object[] array = new object[(int)num];
            for (int i = 0; i < (int)num; i++)
            {
                byte type = (byte)din.ReadByte();
                array[i] = Deserialize(din, type);
            }

            return array;
        }

        private IDictionary DeserializeDictionary(StreamBuffer din)
        {
            byte b = (byte)din.ReadByte();
            byte b2 = (byte)din.ReadByte();
            int num = (int)DeserializeShort(din);
            Type typeOfCode = GetTypeOfCode(b);
            Type typeOfCode2 = GetTypeOfCode(b2);
            Type type = typeof(Dictionary<,>).MakeGenericType(new Type[]
            {
                typeOfCode,
                typeOfCode2
            });
            IDictionary dictionary = Activator.CreateInstance(type) as IDictionary;
            for (int i = 0; i < num; i++)
            {
                object key = Deserialize(din, (b == 0 || b == 42) ? ((byte)din.ReadByte()) : b);
                object value = Deserialize(din, (b2 == 0 || b2 == 42) ? ((byte)din.ReadByte()) : b2);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private bool DeserializeDictionaryArray(StreamBuffer din, short size, out Array arrayResult)
        {
            Type type = DeserializeDictionaryType(din, out byte b, out byte b2);
            arrayResult = Array.CreateInstance(type, (int)size);
            for (short num = 0; num < size; num += 1)
            {
                IDictionary dictionary = Activator.CreateInstance(type) as IDictionary;
                bool flag = dictionary == null;
                if (flag)
                {
                    return false;
                }
                short num2 = DeserializeShort(din);
                for (int i = 0; i < (int)num2; i++)
                {
                    object key;
                    if (b > 0)
                    {
                        key = Deserialize(din, b);
                    }
                    else
                    {
                        byte type2 = (byte)din.ReadByte();
                        key = Deserialize(din, type2);
                    }
                    object value;
                    if (b2 > 0)
                    {
                        value = Deserialize(din, b2);
                    }
                    else
                    {
                        byte type3 = (byte)din.ReadByte();
                        value = Deserialize(din, type3);
                    }
                    dictionary.Add(key, value);
                }
                arrayResult.SetValue(dictionary, (int)num);
            }

            return true;
        }

        private Type DeserializeDictionaryType(StreamBuffer reader, out byte keyTypeCode, out byte valTypeCode)
        {
            Type type;

            keyTypeCode = (byte)reader.ReadByte();
            valTypeCode = (byte)reader.ReadByte();
            GpType gpType = (GpType)keyTypeCode;
            GpType gpType2 = (GpType)valTypeCode;
            if (gpType == GpType.Unknown)
            {
                type = typeof(object);
            }
            else
            {
                type = GetTypeOfCode(keyTypeCode);
            }
            Type type2;
            if (gpType2 == GpType.Unknown)
            {
                type2 = typeof(object);
            }
            else
            {
                type2 = GetTypeOfCode(valTypeCode);
            }
            return typeof(Dictionary<,>).MakeGenericType(new Type[]
            {
                type,
                type2
            });
        }
    }
}