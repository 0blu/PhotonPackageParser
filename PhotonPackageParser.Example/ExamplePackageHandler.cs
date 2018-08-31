using System;
using System.Collections.Generic;

namespace PhotonPackageParser.Example
{
    internal class ExamplePackageHandler : IPhotonPackageHandler
    {
        public void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            Console.WriteLine("Event: " + code + " parameter count: " + parameters.Count);
        }

        public void OnResponse(byte operationCode, short returnCode, Dictionary<byte, object> parameters)
        {
            Console.WriteLine("Response: " + operationCode + " parameter count: " + parameters.Count);
        }

        public void OnRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            Console.WriteLine("Request: " + operationCode + " parameter count: " + parameters.Count);
        }
    }
}
