using System.Collections;
using System.Collections.Generic;

namespace Photon
{
    public class Hashtable : Dictionary<object, object>
    {
        // Token: 0x06000064 RID: 100 RVA: 0x00006C48 File Offset: 0x00004E48
        public Hashtable()
        {
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00006C52 File Offset: 0x00004E52
        public Hashtable(int x) : base(x)
        {
        }

        // Token: 0x17000005 RID: 5
        public new object this[object key]
        {
            get
            {
                object result = null;
                base.TryGetValue(key, out result);
                return result;
            }
            set
            {
                base[key] = value;
            }
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00006C8C File Offset: 0x00004E8C
        public new IEnumerator<DictionaryEntry> GetEnumerator()
        {
            return new DictionaryEntryEnumerator(((IDictionary)this).GetEnumerator());
        }

        // Token: 0x06000069 RID: 105 RVA: 0x00006CAC File Offset: 0x00004EAC
        public override string ToString()
        {
            List<string> list = new List<string>();
            foreach (object obj in base.Keys)
            {
                bool flag = obj == null || this[obj] == null;
                if (flag)
                {
                    list.Add(obj + "=" + this[obj]);
                }
                else
                {
                    list.Add(string.Concat(new object[]
                    {
                        "(",
                        obj.GetType(),
                        ")",
                        obj,
                        "=(",
                        this[obj].GetType(),
                        ")",
                        this[obj]
                    }));
                }
            }
            return string.Join(", ", list.ToArray());
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00006DAC File Offset: 0x00004FAC
        public object Clone()
        {
            return new Dictionary<object, object>(this);
        }
    }
}
