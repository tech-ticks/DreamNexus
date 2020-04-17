using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Infrastructure
{
    public partial class RingBuffer<T> : IReadOnlyList<T>, IDisposable
    {
        private readonly T[] buffer;
        private int _position;
        private int _count;

        public int Capacity { get; }

        public int Count
        {
            get => _count;
            private set
            {
                _count = value;
                if (_count > Capacity)
                {
                    _position += _count - Capacity;
                    _position %= Capacity;
                    _count = Capacity;
                }
            }
        }

        public RingBuffer(int capacity)
        {
            buffer = ArrayPool<T>.Shared.Rent(capacity);
            Capacity = capacity;
        }

        ~RingBuffer()
        {
            Dispose(false);
        }

        public void Add(T item)
        {
            this[Count++] = item;
        }

        public void AddRange(T[] items, int offset, int length)
        {
            if (length > Capacity)
            {
                offset += length - Capacity;
                length = Capacity;
            }

            (var first, var second) = RangeIndexerInternal(..length, true);

            Array.Copy(items, offset, buffer, first.offset, first.length);
            if (second != null)
                Array.Copy(items, offset + first.length, buffer, second.Value.offset, second.Value.length);

            Count += length;
        }

        public void CopyTo(T[] dest, int offset) => CopyRangeTo(0, dest, offset, Count);

        private void CopyRangeTo(int start, T[] dest, int offset, int length)
        {
            if (dest.Length - offset < length)
                throw new ArgumentException(nameof(dest));

            (var first, var second) = RangeIndexerInternal(start..(start + length));
            Array.Copy(buffer, first.offset, dest, offset, first.length);
            if (second != null)
                Array.Copy(buffer, second.Value.offset, dest, offset + first.length, second.Value.length);
        }

        public RingBufferSegment Slice(int start, int length)
        {
            return new RingBufferSegment(this, start, length);
        }

        public T this[int index]
        {
            get => IndexerInternal(index);
            set => IndexerInternal(index) = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            (var first, var second) = RangeIndexerInternal(..);

            for (int i = first.offset; i < first.length; i++)
                yield return buffer[i];

            if (second != null)
                for (int i = second.Value.offset; i < second.Value.length; i++)
                    yield return buffer[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ArrayPool<T>.Shared.Return(buffer);
        }

        private ref T IndexerInternal(int index)
        {
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException();

            return ref buffer[(index + _position) % Capacity];
        }

        private ((int offset, int length), (int offset, int length)?) RangeIndexerInternal(Range range, bool write = false)
        {
            var maxSize = write ? Capacity : Count;
            (var offset, var length) = range.GetOffsetAndLength(maxSize);

            offset += _position;
            offset += write ? Count : 0;
            offset %= Capacity;

            // No more references to _position or Count past this point
            // We don't know if the callee will add any new items
            if (offset + length <= Capacity)
                return ((offset, length), null);
            else
                return ((offset, Capacity - offset), (0, length - (Capacity - offset)));
        }
    }
}