namespace Photon
{
    public class SupportClass
    {
        #region methods
        public static uint CalculateCrc(byte[] buffer, int length)
        {
            uint maxValue = uint.MaxValue;
            uint num2 = 3988292384u;
            for (int i = 0; i < length; i++)
            {
                byte b = buffer[i];
                maxValue ^= b;
                for (int j = 0; j < 8; j++)
                {
                    bool flag = (maxValue & 1u) > 0u;
                    if (flag)
                    {
                        maxValue = (maxValue >> 1 ^ num2);
                    }
                    else
                    {
                        maxValue >>= 1;
                    }
                }
            }

            return maxValue;
        }
        #endregion
    }
}
