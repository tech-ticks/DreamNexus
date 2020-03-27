using SkyEditor.RomEditor.Rtdx.Reverse.Const;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    // The game actually has two classes like this
    public class TextID
    {
        // Class not implemented
        // Just pointing out that TextID and TextId are different

        public enum TextMode
        {
            EditorMode,
            RomMode
        }

        private enum CastIndex
        {
            CAST_NONE,
            INDEX_HERO,
            INDEX_PARTNER,
            INDEX_PLAYER,
            INDEX_IRAI1,
            INDEX_IRAI2
        }
    }

    public class TextId
    {
        public int hashId;
        public string? debugRawText;
        private string? replacedRawText_;

        public bool IsValid => hashId != default;

        public TextId()
        {
        }

        public TextId(TextIDHash hashId_)
        {
            this.hashId = (int)hashId_;
        }

        public TextId(int hashId_)
        {
            this.hashId = hashId_;
        }

        public TextId(string label)
        {
            this.hashId = LabelToHash(label);
        }

        public static explicit operator TextId(TextIDHash hashId)
        {
            return new TextId(hashId);
        }

        public void SetReplacedRawText(string text)
        {
            replacedRawText_ = text;
        }

        public string? GetReplacedRawText()
        {
            return replacedRawText_;
        }

        public string GetText()
        {
            throw new NotImplementedException();
        }

        public static int LabelToHash(string label)
        {
            return textIdValues[label];
        }
        private static readonly Dictionary<string, int> textIdValues = Enum.GetValues(typeof(TextIDHash)).Cast<TextIDHash>().ToDictionary(h => h.ToString("f"), h => (int)h);
    }

}
