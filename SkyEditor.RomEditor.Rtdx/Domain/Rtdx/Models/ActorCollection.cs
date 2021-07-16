using System;
using System.Collections.Generic;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable.PegasusActDatabase;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IActorCollection
    {
        List<ActorData> Actors { get; }
        void Flush(IRtdxRom rom);
    }

    public class ActorCollection : IActorCollection
    {
        public ActorCollection()
        {
            Actors = new List<ActorData>();
        }

        public ActorCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            this.Actors = rom.GetMainExecutable().ActorDatabase.ActorDataList;
        }

        public List<ActorData> Actors { get; set; }

        public void Flush(IRtdxRom rom)
        {
            rom.GetMainExecutable().ActorDatabase.ActorDataList = this.Actors;
        }
    }    
}
