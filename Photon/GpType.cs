namespace Photon
{
    internal enum GpType : byte
    {
        Unknown,
        Array = 121,
        Boolean = 111,
        Byte = 98,
        ByteArray = 120,
        ObjectArray = 122,
        Short = 107,
        Float = 102,
        Dictionary = 68,
        Double = 100,
        Hashtable = 104,
        Integer,
        IntegerArray = 110,
        Long = 108,
        String = 115,
        StringArray = 97,
        Custom = 99,
        Null = 42,
        EventData = 101,
        OperationRequest = 113,
        OperationResponse = 112
    }
}
