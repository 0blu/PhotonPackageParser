namespace Protocol16
{
    public class Deserializer
    {
        public static void Deserialize(out short value, byte[] source, ref int offset)
        {
            int num = offset;
            offset = num + 1;
            short b = (short)(source[num] << 8);
            num = offset;
            offset = num + 1;
            value = (short)(b | source[num]);
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

        //public static void Deserialize(out int value, byte[] source, ref int offset)
        //{
        //    int v1 = source[offset] << 24;
        //    offset++;
        //    int v2 = v1 | source[offset] << 16;
        //    offset++;
        //    int v3 = v2 | source[offset] << 8;
        //    offset++;
        //    value = (v3 | source[offset]);
        //}

        //public static void Deserialize(out short value, byte[] source, ref int offset)
        //{
        //    byte byte1 = (byte)(source[offset] << 8);
        //    offset++;
        //    value = (short)(byte1 | source[offset]);
        //    offset++;
        //}
    }
}
