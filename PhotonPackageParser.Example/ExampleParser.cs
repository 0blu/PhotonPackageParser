using System;
using System.Collections.Generic;

namespace PhotonPackageParser.Example
{
    public class ExampleParser : PhotonParser
    {
        protected override void OnEvent(byte Code, Dictionary<byte, object> Parameters)
        {
            Console.WriteLine("OnEvent");
        }

        protected override void OnRequest(byte OperationCode, Dictionary<byte, object> Parameters)
        {
            Console.WriteLine("OnRequest");
        }

        protected override void OnResponse(byte OperationCode, short ReturnCode, string DebugMessage, Dictionary<byte, object> Parameters)
        {
            Console.WriteLine("OnResponse");
        }
    }
}
