using System;

namespace Photon
{
    internal class CustomType
    {
        // Token: 0x060001C9 RID: 457 RVA: 0x0000F32A File Offset: 0x0000D52A
        public CustomType(Type type, byte code, SerializeMethod serializeFunction, DeserializeMethod deserializeFunction)
        {
            this.Type = type;
            this.Code = code;
            this.SerializeFunction = serializeFunction;
            this.DeserializeFunction = deserializeFunction;
        }

        // Token: 0x060001CA RID: 458 RVA: 0x0000F351 File Offset: 0x0000D551
        public CustomType(Type type, byte code, SerializeStreamMethod serializeFunction, DeserializeStreamMethod deserializeFunction)
        {
            this.Type = type;
            this.Code = code;
            this.SerializeStreamFunction = serializeFunction;
            this.DeserializeStreamFunction = deserializeFunction;
        }

        // Token: 0x04000115 RID: 277
        public readonly byte Code;

        // Token: 0x04000116 RID: 278
        public readonly Type Type;

        // Token: 0x04000117 RID: 279
        public readonly SerializeMethod SerializeFunction;

        // Token: 0x04000118 RID: 280
        public readonly DeserializeMethod DeserializeFunction;

        // Token: 0x04000119 RID: 281
        public readonly SerializeStreamMethod SerializeStreamFunction;

        // Token: 0x0400011A RID: 282
        public readonly DeserializeStreamMethod DeserializeStreamFunction;
    }
}