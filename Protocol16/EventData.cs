using System.Collections.Generic;

namespace Protocol16
{
    public class EventData
    {
        #region ctors
        public EventData(byte code, Dictionary<byte, object> parameters)
        {
            Code = code;
            Parameters = parameters;
        }
        #endregion

        #region properties
        public byte Code { get; }
        public Dictionary<byte, object> Parameters { get; }
        #endregion
    }
}
