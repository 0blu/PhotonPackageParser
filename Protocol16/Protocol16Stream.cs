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
            int result;

            int num = length - position;
            if (num <= 0)
            {
                result = 0;
            }
            else
            {
                if (count > num)
                {
                    count = num;
                }
                Buffer.BlockCopy(this.buffer, position, buffer, offset, count);
                position += count;
                result = count;
            }

            return result;
        }

        public override int ReadByte()
        {
            int result;

            if (position >= length)
            {
                result = -1;
            }
            else
            {
                byte[] array = buffer;
                int num = position;
                position = num + 1;
                result = array[num];
            }

            return result;
        }

        public override long Seek(long offset, SeekOrigin seekOrigin)
        {
            int num;

            switch (seekOrigin)
            {
                case SeekOrigin.Begin:
                    num = (int)offset;
                    break;
                case SeekOrigin.Current:
                    num = position + (int)offset;
                    break;
                case SeekOrigin.End:
                    num = length + (int)offset;
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin");
            }
            if (num < 0)
            {
                throw new ArgumentException("Seek before begin");
            }
            if (num > length)
            {
                throw new ArgumentException("Seek after end");
            }
            position = num;

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
            int num = position + count;
            CheckSize(num);
            if (num > length)
            {
                length = num;
            }
            Buffer.BlockCopy(buffer, offset, this.buffer, position, count);
            position = num;
        }
        #endregion

        #region private methods
        private bool CheckSize(long size)
        {
            bool result;

            if (size <= buffer.Length)
            {
                result = false;
            }
            else
            {
                int num = buffer.Length;
                if (num == 0)
                {
                    num = 1;
                }
                while (size > num)
                {
                    num *= 2;
                }
                byte[] dst = new byte[num];
                Buffer.BlockCopy(buffer, 0, dst, 0, buffer.Length);
                buffer = dst;
                result = true;
            }

            return result;
        }
        #endregion
    }
}
