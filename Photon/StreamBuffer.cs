using System;
using System.IO;

namespace Photon
{
    public class StreamBuffer : Stream
    {
        #region constants
        private const int DefaultInitialSize = 0;
        #endregion

        #region fields
        private int position;
        private int length;
        private byte[] buffer;
        #endregion

        #region ctors
        public StreamBuffer(int size = 0)
        {
            buffer = new byte[size];
        }

        public StreamBuffer(byte[] buffer)
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
                return (long)length;
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

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int num;

            switch (origin)
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

        public override void Write(byte[] buffer, int srcOffset, int count)
        {
            int num = position + count;
            CheckSize(num);
            if (num > length)
            {
                length = num;
            }
            Buffer.BlockCopy(buffer, srcOffset, this.buffer, position, count);
            position = num;
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

        public override void WriteByte(byte value)
        {
            if (position >= length)
            {
                length = position++;
                CheckSize(length);
            }
            byte[] array = buffer;
            position += 1;
            array[position] = value;
        }

        private bool CheckSize(int size)
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
    }
}
