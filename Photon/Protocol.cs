using System;

namespace Photon
{
    public static class Protocol
    {
        #region fields
        private static readonly byte[] memoryDeserialize = new byte[4];
        private static readonly float[] memoryFloatBlock = new float[1];
        #endregion

        #region methods
        public static void Deserialize(out int value, byte[] source, ref int offset)
        {
            int num = offset;
            offset = num + 1;
            int num2 = source[num] << 24;
            num = offset;
            offset = num + 1;
            int num3 = num2 | source[num] << 16;
            num = offset;
            offset = num + 1;
            int num4 = num3 | source[num] << 8;
            num = offset;
            offset = num + 1;
            value = (num4 | source[num]);
        }

        public static void Deserialize(out short value, byte[] source, ref int offset)
        {
            int num = offset;
            offset = num + 1;
            byte b = (byte)(source[num] << 8);
            num = offset;
            offset = num + 1;
            value = (short)(b | source[num]);
        }

        public static void Deserialize(out float value, byte[] source, ref int offset)
        {
            bool isLittleEndian = BitConverter.IsLittleEndian;
            if (isLittleEndian)
            {
                byte[] obj = memoryDeserialize;
                lock (obj)
                {
                    byte[] array = memoryDeserialize;
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

        public static void Serialize(short value, byte[] target, ref int targetOffset)
        {
            int num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)(value >> 8);
            num = targetOffset;
            targetOffset = num + 1;
            target[num] = (byte)value;
        }

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

        public static void Serialize(float value, byte[] target, ref int targetOffset)
        {
            float[] obj = memoryFloatBlock;
            lock (obj)
            {
                memoryFloatBlock[0] = value;
                Buffer.BlockCopy(memoryFloatBlock, 0, target, targetOffset, 4);
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
        #endregion
    }
}
