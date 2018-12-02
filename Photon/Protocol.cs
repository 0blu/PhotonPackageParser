using System;
using System.Collections.Generic;

namespace Photon
{
    public class Protocol
    {
        // Token: 0x060001CB RID: 459 RVA: 0x0000F378 File Offset: 0x0000D578
        //public static bool TryRegisterType(Type type, byte typeCode, SerializeMethod serializeFunction, DeserializeMethod deserializeFunction)
        //{
        //    bool flag = Protocol.CodeDict.ContainsKey(typeCode) || Protocol.TypeDict.ContainsKey(type);
        //    bool result;
        //    if (flag)
        //    {
        //        result = false;
        //    }
        //    else
        //    {
        //        CustomType value = new CustomType(type, typeCode, serializeFunction, deserializeFunction);
        //        Protocol.CodeDict.Add(typeCode, value);
        //        Protocol.TypeDict.Add(type, value);
        //        result = true;
        //    }
        //    return result;
        //}

        // Token: 0x060001CC RID: 460 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
        //public static bool TryRegisterType(Type type, byte typeCode, SerializeStreamMethod serializeFunction, DeserializeStreamMethod deserializeFunction)
        //{
        //    bool flag = Protocol.CodeDict.ContainsKey(typeCode) || Protocol.TypeDict.ContainsKey(type);
        //    bool result;
        //    if (flag)
        //    {
        //        result = false;
        //    }
        //    else
        //    {
        //        CustomType value = new CustomType(type, typeCode, serializeFunction, deserializeFunction);
        //        Protocol.CodeDict.Add(typeCode, value);
        //        Protocol.TypeDict.Add(type, value);
        //        result = true;
        //    }
        //    return result;
        //}

        // Token: 0x060001CD RID: 461 RVA: 0x0000F430 File Offset: 0x0000D630
        //[Obsolete]
        //public static byte[] Serialize(object obj)
        //{
        //    bool flag = Protocol.ProtocolDefault == null;
        //    if (flag)
        //    {
        //        Protocol.ProtocolDefault = new Protocol16();
        //    }
        //    IProtocol protocolDefault = Protocol.ProtocolDefault;
        //    byte[] result;
        //    lock (protocolDefault)
        //    {
        //        result = Protocol.ProtocolDefault.Serialize(obj);
        //    }
        //    return result;
        //}

        // Token: 0x060001CE RID: 462 RVA: 0x0000F48C File Offset: 0x0000D68C
        //[Obsolete]
        //public static object Deserialize(byte[] serializedData)
        //{
        //    bool flag = Protocol.ProtocolDefault == null;
        //    if (flag)
        //    {
        //        Protocol.ProtocolDefault = new Protocol16();
        //    }
        //    Protocol16 protocolDefault = Protocol.ProtocolDefault;
        //    object result;
        //    lock (protocolDefault)
        //    {
        //        result = Protocol.ProtocolDefault.Deserialize(serializedData);
        //    }
        //    return result;
        //}

        // Token: 0x060001CF RID: 463 RVA: 0x0000F4E8 File Offset: 0x0000D6E8
        public static void Serialize(short value, byte[] target, ref int targetOffset)
        {
            int num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)(value >> 8);
            num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)value;
        }

        // Token: 0x060001D0 RID: 464 RVA: 0x0000F514 File Offset: 0x0000D714
        public static void Serialize(int value, byte[] target, ref int targetOffset)
        {
            int num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)(value >> 24);
            num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)(value >> 16);
            num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)(value >> 8);
            num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)value;
        }

        // Token: 0x060001D1 RID: 465 RVA: 0x0000F560 File Offset: 0x0000D760
        public static void Serialize(float value, byte[] target, ref int targetOffset)
        {
            float[] obj = Protocol.memFloatBlock;
            lock (obj)
            {
                Protocol.memFloatBlock[0] = value;
                Buffer.BlockCopy(Protocol.memFloatBlock, 0, target, targetOffset, 4);
            }
            bool isLittleEndian = BitConverter.IsLittleEndian;
            if (isLittleEndian)
            {
                byte b = target[targetOffset];
                byte b2 = target[targetOffset + 1];
                target[targetOffset] = target[targetOffset + 3];
                target[targetOffset + 1] = target[targetOffset + 2];
                target[targetOffset + 2] = b2;
                target[targetOffset + 3] = b;
            }
            targetOffset += 4;
        }

        // Token: 0x060001D2 RID: 466 RVA: 0x0000F5F0 File Offset: 0x0000D7F0
        public static void Deserialize(out int value, byte[] source, ref int offset)
        {
            int num = offset;
            offset = num + 1;
            int num2 = (int)source[num] << 24;
            num = offset;
            offset = num + 1;
            int num3 = num2 | (int)source[num] << 16;
            num = offset;
            offset = num + 1;
            int num4 = num3 | (int)source[num] << 8;
            num = offset;
            offset = num + 1;
            value = (num4 | (int)source[num]);
        }

        // Token: 0x060001D3 RID: 467 RVA: 0x0000F638 File Offset: 0x0000D838
        public static void Deserialize(out short value, byte[] source, ref int offset)
        {
            int num = offset;
            offset = num + 1;
            byte b = (byte)(source[num] << 8);
            num = offset;
            offset = num + 1;
            value = (short)(b | source[num]);
        }

        // Token: 0x060001D4 RID: 468 RVA: 0x0000F664 File Offset: 0x0000D864
        public static void Deserialize(out float value, byte[] source, ref int offset)
        {
            bool isLittleEndian = BitConverter.IsLittleEndian;
            if (isLittleEndian)
            {
                byte[] obj = Protocol.memDeserialize;
                lock (obj)
                {
                    byte[] array = Protocol.memDeserialize;
                    byte[] array2 = array;
                    int num = 3;
                    int num2 = offset;
                    offset = num2 + 1;
                    array2[num] = source[num2];
                    byte[] array3 = array;
                    int num3 = 2;
                    num2 = offset;
                    offset = num2 + 1;
                    array3[num3] = source[num2];
                    byte[] array4 = array;
                    int num4 = 1;
                    num2 = offset;
                    offset = num2 + 1;
                    array4[num4] = source[num2];
                    byte[] array5 = array;
                    int num5 = 0;
                    num2 = offset;
                    offset = num2 + 1;
                    array5[num5] = source[num2];
                    value = BitConverter.ToSingle(array, 0);
                }
            }
            else
            {
                value = BitConverter.ToSingle(source, offset);
                offset += 4;
            }
        }

        // Token: 0x0400011B RID: 283
        internal static readonly Dictionary<Type, CustomType> TypeDict = new Dictionary<Type, CustomType>();

        // Token: 0x0400011C RID: 284
        internal static readonly Dictionary<byte, CustomType> CodeDict = new Dictionary<byte, CustomType>();

        // Token: 0x0400011D RID: 285
        private static Protocol16 ProtocolDefault;

        // Token: 0x0400011E RID: 286
        private static readonly float[] memFloatBlock = new float[1];

        // Token: 0x0400011F RID: 287
        private static readonly byte[] memDeserialize = new byte[4];
    }
}
