namespace Protocol16
{
    public class Serializer
    {
        //public static void Serialize(int value, byte[] target, ref int offset)
        //{
        //    target[offset] = (byte)(value >> 24);
        //    offset++;
        //    target[offset] = (byte)(value >> 16);
        //    offset++;
        //    target[offset] = (byte)(value >> 8);
        //    offset++;
        //    target[offset] = (byte)value;
        //    offset++;
        //}

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
    }
}
