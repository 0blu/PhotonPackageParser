﻿namespace Protocol16
{
    internal enum Protocol16Type : byte
    {
        Unknown = 0,
        Null = 42,
        Dictionary = 68,
        StringArray = 97,
        Byte = 98,
        Double = 100,
        EventData = 101,
        Float = 102,
        Integer = 105,
        Short = 107,
        Long = 108,
        IntegerArray = 110,
        Boolean = 111,
        OperationResponse = 112,
        OperationRequest = 113,
        String = 115,
        ByteArray = 120,
        Array = 121,
        ObjectArray = 122
    }
}
