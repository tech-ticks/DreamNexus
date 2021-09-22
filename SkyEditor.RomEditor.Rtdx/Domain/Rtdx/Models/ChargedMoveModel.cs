using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class ChargedMoveModel
    {
        public WazaIndex BaseMove { get; set; }
        public ushort BaseAction { get; set; }
        public WazaIndex FinalMove { get; set; }
        public ushort FinalAction { get; set; }
        public ushort Short08 { get; set; }
    }
}
