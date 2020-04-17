using System;
using System.Collections;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Rtdx.Infrastructure
{
    public partial class RingBuffer<T>
    {
        public class RingBufferSegment : IReadOnlyList<T>
        {
            private readonly RingBuffer<T> root;

            protected int Position { get; }

            public int Count { get; }

            public RingBufferSegment(RingBuffer<T> root, int position, int count)
            {
                if (root.Count < position)
                    throw new ArgumentOutOfRangeException(nameof(position));
                else if (root.Count < count)
                    throw new ArgumentOutOfRangeException(nameof(count));

                this.root = root;
                Position = position;
                Count = count;
            }

            public void CopyTo(T[] dest, int offset) => root.CopyRangeTo(Position, dest, offset, Count);

            public RingBufferSegment Slice(int start, int length) => new RingBufferSegment(root, start, length);

            public T this[int index] => root.IndexerInternal(Position + index);

            public IEnumerator<T> GetEnumerator()
            {
                (var first, var second) = root.RangeIndexerInternal(Position..Count);

                for (int i = first.offset; i < first.length; i++)
                    yield return root.buffer[i];

                if (second != null)
                    for (int i = second.Value.offset; i < second.Value.length; i++)
                        yield return root.buffer[i];
            }
            
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}