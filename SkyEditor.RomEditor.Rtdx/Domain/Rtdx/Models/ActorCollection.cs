using System;
using System.Collections.Generic;
using System.Linq;
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

            var actors = new List<ActorData>();
            var romActors = rom.GetMainExecutable().ActorDatabase.ActorDataList;
            foreach (var romActor in romActors)
            {
                actors.Add(new ActorData
                {
                    SymbolName = romActor.SymbolName,
                    PokemonIndex = romActor.PokemonIndex,
                    FormType = romActor.FormType,
                    IsFemale = romActor.IsFemale,
                    PartyId = romActor.PartyId,
                    WarehouseId = romActor.WarehouseId,
                    SpecialName = romActor.SpecialName,
                    DebugName = romActor.DebugName
                });
            }
            Actors = actors;
        }

        public List<ActorData> Actors { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romActors = rom.GetMainExecutable().ActorDatabase.ActorDataList;
            romActors.Clear();
            foreach (var actor in Actors)
            {
                romActors.Add(new ActorData
                {
                    SymbolName = actor.SymbolName,
                    PokemonIndex = actor.PokemonIndex,
                    FormType = actor.FormType,
                    IsFemale = actor.IsFemale,
                    PartyId = actor.PartyId,
                    WarehouseId = actor.WarehouseId,
                    SpecialName = actor.SpecialName,
                    DebugName = actor.DebugName
                });
            }
        }
    }    
}
