using System;
using System.IO;

namespace Protocol16
{
    public class Protocol16Stream : Stream
    {
        #region fields
        private int position;
        private int length;
        private byte[] buffer;
        #endregion

        #region ctors
        public Protocol16Stream(int size = 0)
        {
            buffer = new byte[size];
        }

        public Protocol16Stream(byte[] buffer)
        {
            this.buffer = buffer;
            length = buffer.Length;
        }
        #endregion

        #region properties
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return length;
            }
        }

        public override long Position
        {
            get
            {
                return (long)position;
            }
            set
            {
                position = (int)value;
                if (length < position)
                {
                    length = position;
                    CheckSize(length);
                }
            }
        }
        #endregion

        #region methods
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int dif = length - position;
            if (dif <= 0)
            {
                return 0;
            }
            if (count > dif)
            {
                count = dif;
            }
            Buffer.BlockCopy(this.buffer, position, buffer, offset, count);
            position += count;

            return count;
        }

        public override int ReadByte()
        {
            if (position >= length)
            {
                return -1;
            }
            byte[] array = buffer;
            int result = array[position];
            position++;

            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int newLength;

            switch (origin)
            {
                case SeekOrigin.Begin:
                    newLength = (int)offset;
                    break;
                case SeekOrigin.Current:
                    newLength = position + (int)offset;
                    break;
                case SeekOrigin.End:
                    newLength = length + (int)offset;
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin");
            }
            if (newLength < 0)
            {
                throw new ArgumentException("Seek before begin");
            }
            if (newLength > length)
            {
                throw new ArgumentException("Seek after end");
            }
            position = newLength;

            return (long)position;
        }

        public override void SetLength(long value)
        {
            length = (int)value;
            CheckSize(length);
            if (position > length)
            {
                position = length;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            int sum = position + count;
            CheckSize(sum);
            if (sum > length)
            {
                length = sum;
            }
            Buffer.BlockCopy(buffer, offset, this.buffer, position, count);
            position = sum;
        }
        #endregion

        #region private methods
        private bool CheckSize(long size)
        {
            if (size <= buffer.Length)
            {
                return false;
            }
            int length = buffer.Length;
            if (length == 0)
            {
                length = 1;
            }
            while (size > length)
            {
                length *= 2;
            }
            byte[] dst = new byte[length];
            Buffer.BlockCopy(buffer, 0, dst, 0, buffer.Length);
            buffer = dst;

            return true;
        }
        #endregion
    }
}
