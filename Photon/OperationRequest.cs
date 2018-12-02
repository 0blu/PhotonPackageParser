using System.Collections.Generic;

namespace Photon
{
    public class OperationRequest
    {
        public byte OperationCode;
        public Dictionary<byte, object> Parameters;
    }
}
