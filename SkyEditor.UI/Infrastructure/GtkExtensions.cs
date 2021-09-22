using Gtk;

namespace SkyEditorUI.Infrastructure
{
    public static class GtkExtensions
    {
        public static int ParseInt(this Entry entry, int defaultValue)
        {
            if (int.TryParse(entry!.Text, out var parsed))
            {
                return parsed;
            }
            else if (!string.IsNullOrEmpty(entry!.Text))
            {
                entry!.Text = defaultValue.ToString();
            }
            return defaultValue;
        }
        public static uint ParseUInt(this Entry entry, uint defaultValue)
        {
            if (uint.TryParse(entry!.Text, out var parsed))
            {
                return parsed;
            }
            else if (!string.IsNullOrEmpty(entry!.Text))
            {
                entry!.Text = defaultValue.ToString();
            }
            return defaultValue;
        }

        public static short ParseShort(this Entry entry, short defaultValue)
        {
            if (short.TryParse(entry!.Text, out var parsed))
            {
                return parsed;
            }
            else if (!string.IsNullOrEmpty(entry!.Text))
            {
                entry!.Text = defaultValue.ToString();
            }
            return defaultValue;
        }

        public static ushort ParseUShort(this Entry entry, ushort defaultValue)
        {
            if (ushort.TryParse(entry!.Text, out var parsed))
            {
                return parsed;
            }
            else if (!string.IsNullOrEmpty(entry!.Text))
            {
                entry!.Text = defaultValue.ToString();
            }
            return defaultValue;
        }

        public static byte ParseByte(this Entry entry, byte defaultValue)
        {
            if (byte.TryParse(entry!.Text, out var parsed))
            {
                return parsed;
            }
            else if (!string.IsNullOrEmpty(entry!.Text))
            {
                entry!.Text = defaultValue.ToString();
            }
            return defaultValue;
        }

        public static void FixIndices(this ListStore store, int indexColumn, int startIndex = 0)
        {
            if (!store.GetIterFirst(out var iter))
            {
                return;
            }

            int i = startIndex;
            do
            {
                store.SetValue(iter, indexColumn, i++);
            }
            while (store.IterNext(ref iter));
        }
    }
}
