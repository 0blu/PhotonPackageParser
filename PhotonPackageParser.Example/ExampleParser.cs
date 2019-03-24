using System;
using System.Collections.Generic;

namespace PhotonPackageParser.Example
{
    public class ExampleParser : PhotonParser
    {
        protected override void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            Console.WriteLine("OnEvent");
        }

        protected override void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            Console.WriteLine("OnRequest");
        }

        protected override void OnResponse(byte operationCode, short returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            Console.WriteLine("OnResponse");
        }
    }
}
