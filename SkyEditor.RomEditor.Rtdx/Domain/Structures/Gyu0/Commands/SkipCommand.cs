namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private class SkipCommand : FillCommand
        {
            public SkipCommand(int byteCount)
                : base(byteCount, 0)
            {
            }
        }
    }
}