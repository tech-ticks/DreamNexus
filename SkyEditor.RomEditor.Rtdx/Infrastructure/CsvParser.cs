using System.Collections.Generic;
using System.Text;

public class CsvParser
{
    private string data;
    private char delimiter;
    private StringBuilder builder = new StringBuilder();

    public CsvParser(string data, char delimiter = ',')
    {
        this.data = data;
        this.delimiter = delimiter;
    }

    public IEnumerable<IList<string>> GetLines()
    {
        int currentPos = 0;
        var items = new List<string>();
        bool inEscapedSection = false;
        while (currentPos < data.Length)
        {
            var c = data[currentPos++];
            if (c == '"' && currentPos < data.Length && data[currentPos] != '"') {
                // A single " character escapes ','
                inEscapedSection = !inEscapedSection;
                continue;
            }

            if (c == '"' && inEscapedSection) {
                builder.Append('"'); // Escaped ""
                continue;
            }

            if (c == delimiter && !inEscapedSection) {
                items.Add(builder.ToString());
                builder.Clear();
                continue;
            }

            if (c == '\n' && !inEscapedSection)
            {
                items.Add(builder.ToString());
                builder.Clear();

                yield return items;
                items = new List<string>();
                inEscapedSection = false;
                continue;
            }

            builder.Append(c);
        }

        if (builder.Length > 0)
        {
            items.Add(builder.ToString());
        }
        if (items.Count > 0)
        {
            yield return items;
        }
    }
}
