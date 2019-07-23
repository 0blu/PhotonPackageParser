using System;
using System.IO;

namespace Protocol16
{
    public class Protocol16Stream : Stream
    {
        #region fields
        private int _position;
        private int _length;
        #endregion

        #region ctors
        public Protocol16Stream(int size = 0)
        {
            GetBuffer = new byte[size];
        }

        public Protocol16Stream(byte[] buffer)
        {
            GetBuffer = buffer;
            _length = buffer.Length;
        }
        #endregion

        #region properties
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => _length;

        public override long Position
        {
            get => _position;
            set
            {
                _position = (int)value;
                if (_length < _position)
                {
                    _length = _position;
                    ExpandIfNeeded(_length);
                }
            }
        }

        public byte[] GetBuffer { get; private set; }
        #endregion

        #region methods
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int dif = _length - _position;
            if (dif <= 0)
            {
                return 0;
            }
            if (count > dif)
            {
                count = dif;
            }
            Buffer.BlockCopy(GetBuffer, _position, buffer, offset, count);
            _position += count;

            return count;
        }

        public override int ReadByte()
        {
            if (_position >= _length)
            {
                return -1;
            }
            byte[] array = GetBuffer;
            int result = array[_position];
            _position++;

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
                    newLength = _position + (int)offset;
                    break;
                case SeekOrigin.End:
                    newLength = _length + (int)offset;
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin");
            }
            if (newLength < 0)
            {
                throw new ArgumentException("Seek before begin");
            }
            if (newLength > _length)
            {
                throw new ArgumentException("Seek after end");
            }
            _position = newLength;

            return _position;
        }

        public override void SetLength(long value)
        {
            _length = (int)value;
            ExpandIfNeeded(_length);
            if (_position > _length)
            {
                _position = _length;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            int sum = _position + count;
            ExpandIfNeeded(sum);
            if (sum > _length)
            {
                _length = sum;
            }
            Buffer.BlockCopy(buffer, offset, GetBuffer, _position, count);
            _position = sum;
        }
        #endregion

        #region private methods
        private bool ExpandIfNeeded(long size)
        {
            if (size <= GetBuffer.Length)
            {
                return false;
            }
            int length = GetBuffer.Length;
            if (length == 0)
            {
                length = 1;
            }
            while (size > length)
            {
                length *= 2;
            }
            byte[] dst = new byte[length];
            Buffer.BlockCopy(GetBuffer, 0, dst, 0, GetBuffer.Length);
            GetBuffer = dst;

            return true;
        }
        #endregion
    }
}
