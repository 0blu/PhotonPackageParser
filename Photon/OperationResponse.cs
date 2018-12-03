using System.Collections.Generic;

namespace Photon
{
    public class OperationResponse
    {
        #region ctors
        public OperationResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            OperationCode = operationCode;
            ReturnCode = returnCode;
            DebugMessage = debugMessage;
            Parameters = parameters;
        }
        #endregion

        #region properties
        public byte OperationCode { get; }
        public short ReturnCode { get; }
        public string DebugMessage { get; }
        public Dictionary<byte, object> Parameters { get; }
        #endregion
    }
}
