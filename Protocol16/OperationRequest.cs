using System.Collections.Generic;

namespace Protocol16
{
    public class OperationRequest
    {
        #region ctors
        internal OperationRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            OperationCode = operationCode;
            Parameters = parameters;
        }
        #endregion

        #region properties
        public byte OperationCode { get; }
        public Dictionary<byte, object> Parameters { get; }
        #endregion
    }
}
