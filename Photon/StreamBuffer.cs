using System;
using System.IO;

namespace Photon
{
    public class StreamBuffer : Stream
    {
        // Token: 0x06000087 RID: 135 RVA: 0x00007401 File Offset: 0x00005601
        public StreamBuffer(int size = 0)
        {
            this.buf = new byte[size];
        }

        // Token: 0x06000088 RID: 136 RVA: 0x00007417 File Offset: 0x00005617
        public StreamBuffer(byte[] buf)
        {
            this.buf = buf;
            this.len = buf.Length;
        }

        // Token: 0x06000089 RID: 137 RVA: 0x00007434 File Offset: 0x00005634
        public byte[] ToArray()
        {
            byte[] array = new byte[this.len];
            Buffer.BlockCopy(this.buf, 0, array, 0, this.len);
            return array;
        }

        // Token: 0x0600008A RID: 138 RVA: 0x00007468 File Offset: 0x00005668
        public byte[] ToArrayFromPos()
        {
            int num = this.len - this.pos;
            bool flag = num <= 0;
            byte[] result;
            if (flag)
            {
                result = new byte[0];
            }
            else
            {
                byte[] array = new byte[num];
                Buffer.BlockCopy(this.buf, this.pos, array, 0, num);
                result = array;
            }
            return result;
        }

        // Token: 0x0600008B RID: 139 RVA: 0x000074BC File Offset: 0x000056BC
        public void Compact()
        {
            long num = this.Length - this.Position;
            bool flag = num > 0L;
            if (flag)
            {
                Buffer.BlockCopy(this.buf, (int)this.Position, this.buf, 0, (int)num);
            }
            this.Position = 0L;
            this.SetLength(num);
        }

        // Token: 0x0600008C RID: 140 RVA: 0x00007510 File Offset: 0x00005710
        public byte[] GetBuffer()
        {
            return this.buf;
        }

        // Token: 0x0600008D RID: 141 RVA: 0x00007528 File Offset: 0x00005728
        public byte[] GetBufferAndAdvance(int length, out int offset)
        {
            offset = (int)this.Position;
            this.Position += (long)length;
            return this.buf;
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600008E RID: 142 RVA: 0x0000755C File Offset: 0x0000575C
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x0600008F RID: 143 RVA: 0x00007570 File Offset: 0x00005770
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000090 RID: 144 RVA: 0x00007584 File Offset: 0x00005784
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000091 RID: 145 RVA: 0x00007598 File Offset: 0x00005798
        public override long Length
        {
            get
            {
                return (long)this.len;
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000092 RID: 146 RVA: 0x000075B4 File Offset: 0x000057B4
        // (set) Token: 0x06000093 RID: 147 RVA: 0x000075D0 File Offset: 0x000057D0
        public override long Position
        {
            get
            {
                return (long)this.pos;
            }
            set
            {
                this.pos = (int)value;
                bool flag = this.len < this.pos;
                if (flag)
                {
                    this.len = this.pos;
                    this.CheckSize(this.len);
                }
            }
        }

        // Token: 0x06000094 RID: 148 RVA: 0x00007613 File Offset: 0x00005813
        public override void Flush()
        {
        }

        // Token: 0x06000095 RID: 149 RVA: 0x00007618 File Offset: 0x00005818
        public override long Seek(long offset, SeekOrigin origin)
        {
            int num;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    num = (int)offset;
                    break;
                case SeekOrigin.Current:
                    num = this.pos + (int)offset;
                    break;
                case SeekOrigin.End:
                    num = this.len + (int)offset;
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin");
            }
            bool flag = num < 0;
            if (flag)
            {
                throw new ArgumentException("Seek before begin");
            }
            bool flag2 = num > this.len;
            if (flag2)
            {
                throw new ArgumentException("Seek after end");
            }
            this.pos = num;
            return (long)this.pos;
        }

        // Token: 0x06000096 RID: 150 RVA: 0x000076A8 File Offset: 0x000058A8
        public override void SetLength(long value)
        {
            this.len = (int)value;
            this.CheckSize(this.len);
            bool flag = this.pos > this.len;
            if (flag)
            {
                this.pos = this.len;
            }
        }

        // Token: 0x06000097 RID: 151 RVA: 0x000076EB File Offset: 0x000058EB
        public void SetCapacityMinimum(int neededSize)
        {
            this.CheckSize(neededSize);
        }

        // Token: 0x06000098 RID: 152 RVA: 0x000076F8 File Offset: 0x000058F8
        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = this.len - this.pos;
            bool flag = num <= 0;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                bool flag2 = count > num;
                if (flag2)
                {
                    count = num;
                }
                Buffer.BlockCopy(this.buf, this.pos, buffer, offset, count);
                this.pos += count;
                result = count;
            }
            return result;
        }

        // Token: 0x06000099 RID: 153 RVA: 0x0000775C File Offset: 0x0000595C
        public override void Write(byte[] buffer, int srcOffset, int count)
        {
            int num = this.pos + count;
            this.CheckSize(num);
            bool flag = num > this.len;
            if (flag)
            {
                this.len = num;
            }
            Buffer.BlockCopy(buffer, srcOffset, this.buf, this.pos, count);
            this.pos = num;
        }

        // Token: 0x0600009A RID: 154 RVA: 0x000077B0 File Offset: 0x000059B0
        public override int ReadByte()
        {
            bool flag = this.pos >= this.len;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                byte[] array = this.buf;
                int num = this.pos;
                this.pos = num + 1;
                result = array[num];
            }
            return result;
        }

        // Token: 0x0600009B RID: 155 RVA: 0x000077F4 File Offset: 0x000059F4
        public override void WriteByte(byte value)
        {
            bool flag = this.pos >= this.len;
            if (flag)
            {
                this.len = this.pos + 1;
                this.CheckSize(this.len);
            }
            byte[] array = this.buf;
            int num = this.pos;
            this.pos = num + 1;
            array[num] = value;
        }

        // Token: 0x0600009C RID: 156 RVA: 0x00007850 File Offset: 0x00005A50
        public void WriteBytes(byte v0, byte v1)
        {
            int num = this.pos + 2;
            bool flag = this.len < num;
            if (flag)
            {
                this.len = num;
                this.CheckSize(this.len);
            }
            byte[] array = this.buf;
            int num2 = this.pos;
            this.pos = num2 + 1;
            array[num2] = v0;
            byte[] array2 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array2[num2] = v1;
        }

        // Token: 0x0600009D RID: 157 RVA: 0x000078BC File Offset: 0x00005ABC
        public void WriteBytes(byte v0, byte v1, byte v2)
        {
            int num = this.pos + 3;
            bool flag = this.len < num;
            if (flag)
            {
                this.len = num;
                this.CheckSize(this.len);
            }
            byte[] array = this.buf;
            int num2 = this.pos;
            this.pos = num2 + 1;
            array[num2] = v0;
            byte[] array2 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array2[num2] = v1;
            byte[] array3 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array3[num2] = v2;
        }

        // Token: 0x0600009E RID: 158 RVA: 0x00007944 File Offset: 0x00005B44
        public void WriteBytes(byte v0, byte v1, byte v2, byte v3)
        {
            int num = this.pos + 4;
            bool flag = this.len < num;
            if (flag)
            {
                this.len = num;
                this.CheckSize(this.len);
            }
            byte[] array = this.buf;
            int num2 = this.pos;
            this.pos = num2 + 1;
            array[num2] = v0;
            byte[] array2 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array2[num2] = v1;
            byte[] array3 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array3[num2] = v2;
            byte[] array4 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array4[num2] = v3;
        }

        // Token: 0x0600009F RID: 159 RVA: 0x000079E4 File Offset: 0x00005BE4
        public void WriteBytes(byte v0, byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7)
        {
            int num = this.pos + 8;
            bool flag = this.len < num;
            if (flag)
            {
                this.len = num;
                this.CheckSize(this.len);
            }
            byte[] array = this.buf;
            int num2 = this.pos;
            this.pos = num2 + 1;
            array[num2] = v0;
            byte[] array2 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array2[num2] = v1;
            byte[] array3 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array3[num2] = v2;
            byte[] array4 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array4[num2] = v3;
            byte[] array5 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array5[num2] = v4;
            byte[] array6 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array6[num2] = v5;
            byte[] array7 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array7[num2] = v6;
            byte[] array8 = this.buf;
            num2 = this.pos;
            this.pos = num2 + 1;
            array8[num2] = v7;
        }

        // Token: 0x060000A0 RID: 160 RVA: 0x00007AEC File Offset: 0x00005CEC
        private bool CheckSize(int size)
        {
            bool flag = size <= this.buf.Length;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                int num = this.buf.Length;
                bool flag2 = num == 0;
                if (flag2)
                {
                    num = 1;
                }
                while (size > num)
                {
                    num *= 2;
                }
                byte[] dst = new byte[num];
                Buffer.BlockCopy(this.buf, 0, dst, 0, this.buf.Length);
                this.buf = dst;
                result = true;
            }
            return result;
        }

        // Token: 0x04000027 RID: 39
        private const int DefaultInitialSize = 0;

        // Token: 0x04000028 RID: 40
        private int pos;

        // Token: 0x04000029 RID: 41
        private int len;

        // Token: 0x0400002A RID: 42
        private byte[] buf;
    }
}
