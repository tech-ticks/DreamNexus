using System.Collections.Generic;

namespace SkyEditor.RomEditor.Infrastructure
{
    public static class EnumerableExtensions
    {
#if NETSTANDARD2_0
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int n)
        {
            var it = source.GetEnumerator();
            bool hasRemainingItems = false;
            var queue = new Queue<T>(n + 1);

            do
            {
                if (hasRemainingItems = it.MoveNext())
                {
                    queue.Enqueue(it.Current);
                    if (queue.Count > n)
                        yield return queue.Dequeue();
                }
            }
            while (hasRemainingItems);
        }
#endif
    }
}
