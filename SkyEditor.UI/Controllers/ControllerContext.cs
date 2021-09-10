using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    public abstract class ControllerContext
    {
        public static ControllerContext Null { get; } = new NullControllerContext();
    }

    public class NullControllerContext : ControllerContext
    {
    }

    public class SourceFileControllerContext : ControllerContext
    {
        public SourceFile SourceFile { get; }

        public SourceFileControllerContext(SourceFile sourceFile)
        {
            SourceFile = sourceFile;
        }
    }

    public class PokemonControllerContext : ControllerContext
    {
        public CreatureIndex Index { get; set; }

        public PokemonControllerContext(CreatureIndex index)
        {
            Index = index;
        }
    }

    public class ItemControllerContext : ControllerContext
    {
        public ItemIndex Index { get; set; }
        public string InternalName { get; set; }

        public ItemControllerContext(ItemIndex index, string internalName)
        {
            Index = index;
            InternalName = internalName;
        }
    }

    public class MoveControllerContext : ControllerContext
    {
        public WazaIndex Index { get; }

        public MoveControllerContext(WazaIndex index)
        {
            Index = index;
        }
    }

    public class ActionControllerContext : ControllerContext
    {
        public int Index { get; }

        public ActionControllerContext(int index)
        {
            Index = index;
        }
    }

    public class DungeonControllerContext : ControllerContext
    {
        public DungeonIndex Index { get; }

        public DungeonControllerContext(DungeonIndex index)
        {
            Index = index;
        }
    }

    public class DungeonFloorControllerContext : ControllerContext
    {
        public DungeonIndex DungeonIndex { get; }
        public int FloorIndex { get; set; }

        public DungeonFloorControllerContext(DungeonIndex dungeonIndex, int floorNum)
        {
            DungeonIndex = dungeonIndex;
            FloorIndex = floorNum;
        }
    }

    public class StringsControllerContext : ControllerContext
    {
        public LanguageType Language { get; set; }

        public StringsControllerContext(LanguageType language)
        {
            Language = language;
        }
    }
}
