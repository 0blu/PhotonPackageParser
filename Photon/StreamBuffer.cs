using System;
using System.IO;

namespace Photon
{
    public class StreamBuffer : Stream
    {
        #region fields
        private readonly byte[] buffer;
        private int length;
        private int position;
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

        #region props
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
                return position;
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
            int num = length - position;
            int result;
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
                Buffer.BlockCopy(buffer, position, buffer, offset, count);
                position += count;
                result = count;
            }

            return result;
        }

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
                    num = position + (int)offset;
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

            return position;
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
            Buffer.BlockCopy(buffer, offset, buffer, position, count);
            position = num;
        }
        #endregion

        #region private methods
        private bool CheckSize(int size)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
