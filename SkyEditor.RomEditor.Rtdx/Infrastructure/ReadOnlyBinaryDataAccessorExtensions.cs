using SkyEditor.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Infrastructure
{
    public static class ReadOnlyBinaryDataAccessorExtensions
    {
        public static string ReadNullTerminatedUtf16String(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var encoding = Encoding.Unicode;
            var output = new StringBuilder();
            var index = offset;
            Span<char> chars = stackalloc char[1];
            do
            {
                var length = encoding.GetChars(accessor.ReadSpan(index, 2), chars);
                if (length > 0)
                {
                    output.Append(chars);
                }
                index += 2;
            } while (chars[0] != '\0');
            output.Length -= 1; // Trim the null char
            return output.ToString();
        }
    }
}
