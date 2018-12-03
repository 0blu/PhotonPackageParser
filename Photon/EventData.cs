using System.Collections.Generic;

namespace Photon
{
    public class EventData
    {
        public byte Code;
        public Dictionary<byte, object> Parameters;

        public object this[byte key]
        {
            get
            {
                Parameters.TryGetValue(key, out object result);

                return result;
            }
            set
            {
                Parameters[key] = value;
            }
        }
    }
}
