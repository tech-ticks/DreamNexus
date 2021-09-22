using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class ExtraLargeMoveModel
    {
        public WazaIndex BaseMove { get; set; }
        public ushort BaseAction { get; set; }
        public WazaIndex LargeMove { get; set; }
        public ushort LargeAction { get; set; }
    }
}
