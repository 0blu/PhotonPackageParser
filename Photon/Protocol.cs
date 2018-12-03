namespace Photon
{
    public class Protocol
    {
        #region fields
        private static readonly float[] memoryFloatBlock = new float[1];
        private static readonly byte[] memoryDeserialize = new byte[4];
        #endregion

        #region methods
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

        public static void Deserialize(out short value, byte[] source, ref int offset)
        {
            int num = offset;
            offset = num + 1;
            byte b = (byte)(source[num] << 8);
            num = offset;
            offset = num + 1;
            value = (short)(b | source[num]);
        }
        #endregion
    }
}
