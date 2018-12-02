using System;
using System.Collections;
using System.Collections.Generic;

namespace Photon
{
    public class DictionaryEntryEnumerator : IEnumerator<DictionaryEntry>, IEnumerator, IDisposable
    {
        // Token: 0x0600006B RID: 107 RVA: 0x00006DC4 File Offset: 0x00004FC4
        public DictionaryEntryEnumerator(IDictionaryEnumerator original)
        {
            this.enumerator = original;
        }

        // Token: 0x0600006C RID: 108 RVA: 0x00006DD8 File Offset: 0x00004FD8
        public bool MoveNext()
        {
            return this.enumerator.MoveNext();
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00006DF5 File Offset: 0x00004FF5
        public void Reset()
        {
            this.enumerator.Reset();
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600006E RID: 110 RVA: 0x00006E04 File Offset: 0x00005004
        object IEnumerator.Current
        {
            get
            {
                return (DictionaryEntry)this.enumerator.Current;
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600006F RID: 111 RVA: 0x00006E2C File Offset: 0x0000502C
        public DictionaryEntry Current
        {
            get
            {
                return (DictionaryEntry)this.enumerator.Current;
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000070 RID: 112 RVA: 0x00006E50 File Offset: 0x00005050
        public object Key
        {
            get
            {
                return this.enumerator.Key;
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000071 RID: 113 RVA: 0x00006E70 File Offset: 0x00005070
        public object Value
        {
            get
            {
                return this.enumerator.Value;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000072 RID: 114 RVA: 0x00006E90 File Offset: 0x00005090
        public DictionaryEntry Entry
        {
            get
            {
                return this.enumerator.Entry;
            }
        }

        // Token: 0x06000073 RID: 115 RVA: 0x00006EAD File Offset: 0x000050AD
        public void Dispose()
        {
            this.enumerator = null;
        }

        // Token: 0x04000010 RID: 16
        private IDictionaryEnumerator enumerator;
    }
}
