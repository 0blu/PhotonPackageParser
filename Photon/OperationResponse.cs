using System.Collections.Generic;

namespace Photon
{
    public class OperationResponse
    {
        public byte OperationCode;
        public short ReturnCode;
        public string DebugMessage;
        public Dictionary<byte, object> Parameters;

        public object this[byte parameterCode]
        {
            get
            {
                Parameters.TryGetValue(parameterCode, out object result);
                return result;
            }
            set
            {
                Parameters[parameterCode] = value;
            }
        }
    }
}
