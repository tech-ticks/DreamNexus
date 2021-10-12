using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Linq;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable.PegasusActDatabase;

namespace SkyEditorUI.Controllers
{
    class ActorListController : Stack
    {
        [UI] private ListStore? actorsStore;
        [UI] private TreeView? actorsTree;

        [UI] private ListStore? creaturesStore;
        [UI] private ListStore? formTypesStore;
        [UI] private EntryCompletion? creaturesCompletion;

        private IRtdxRom? rom;
        private IActorCollection? actors;

        private const int NameColumn = 1;
        private const int CreatureIdColumn = 2;
        private const int FormTypeColumn = 3;
        private const int IsFemaleColumn = 4;
        private const int PartyIdColumn = 5;
        private const int WarehouseIdColumn = 6;
        private const int SpecialNameColumn = 7;
        private const int DebugNameColumn = 8;

        public ActorListController(IRtdxRom rom, Modpack modpack) : this(new Builder("ActorList.glade"), rom, modpack)
        {
        }

        private ActorListController(Builder builder, IRtdxRom rom, Modpack modpack) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            VisibleChild = (Widget) builder.GetObject(modpack.Metadata.EnableCodeInjection ? "box_list" : "box_na");
            if (!modpack.Metadata.EnableCodeInjection)
            {
                return;
            }

            this.rom = rom;
            this.actors = rom.GetActors();

            for (int i = 0; i < actors.Actors.Count; i++)
            {
                var actor = actors.Actors[i];
                AddActorToStore(actor, i);
            }

            creaturesStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));
            formTypesStore!.AppendAll(Enum.GetNames<PokemonFormType>().SkipLast(1));
        }

        private void AddActorToStore(ActorData actor, int index)
        {
            actorsStore!.AppendValues(index,
                actor.SymbolName,
                AutocompleteHelpers.FormatPokemon(rom!, actor.PokemonIndex),
                actor.FormType.ToString(),
                actor.IsFemale,
                (int) actor.PartyId,
                FormatWarehouseId(actor.WarehouseId),
                (int) actor.SpecialName,
                actor.DebugName
            );
        }

        private void OnNameEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (actorsStore!.GetIter(out var iter, path) && !string.IsNullOrWhiteSpace(args.NewText))
            {
                actorsStore.SetValue(iter, NameColumn, args.NewText);
                actors!.Actors[path.Indices[0]].SymbolName = args.NewText;
            }
        }

        private void OnSpeciesEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (actorsStore!.GetIter(out var iter, path))
            {
                var creatureIndex = AutocompleteHelpers.ExtractPokemon(args.NewText);
                if (creatureIndex.HasValue)
                {
                    actorsStore.SetValue(iter, CreatureIdColumn,
                    AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value));
                    actors!.Actors[path.Indices[0]].PokemonIndex = creatureIndex.Value;
                }
            }
        }

        private void OnSpeciesEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = creaturesCompletion;
        }

        private void OnFormTypeChanged(object sender, ChangedArgs args)
        {
            var path = new TreePath((string) args.Args[0]);

            if (actorsStore!.GetIter(out var iter, path))
            {
                var formTypeIter = (TreeIter) args.Args[1];
                var formTypePath = formTypesStore!.GetPath(formTypeIter);
                var formType = (PokemonFormType) formTypePath.Indices[0];

                actorsStore.SetValue(iter, FormTypeColumn, formType.ToString());
                actors!.Actors[formTypePath.Indices[0]].FormType = formType;
            }
        }

        private void OnFemaleToggled(object sender, ToggledArgs args)
        {
            var path = new TreePath(args.Path);
            if (actorsStore!.GetIter(out var iter, path))
            {
                var actor = actors!.Actors[path.Indices[0]];
                actor.IsFemale = !actor.IsFemale;
                actorsStore.SetValue(iter, IsFemaleColumn, actor.IsFemale);
            }
        }

        private void OnPartyIdEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (actorsStore!.GetIter(out var iter, path))
            {
                var actor = actors!.Actors[path.Indices[0]];
                if (int.TryParse(args.NewText, out int value))
                {
                    var partyId = (PegasusActorDataPartyId) value;
                    if (partyId >= PegasusActorDataPartyId.NONE && partyId <= PegasusActorDataPartyId.PARTY3)
                    {
                        actor.PartyId = partyId;
                    }
                }
                actorsStore.SetValue(iter, PartyIdColumn, (int) actor.PartyId);
            }
        }

        private void OnWarehouseIdEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (actorsStore!.GetIter(out var iter, path))
            {
                var actor = actors!.Actors[path.Indices[0]];
                if (string.IsNullOrWhiteSpace(args.NewText)
                    || args.NewText.ToLower() == "none" || args.NewText.ToLower() == "null")
                {
                    actor.WarehouseId = PokemonFixedWarehouseId.NONE;
                }
                else if (int.TryParse(args.NewText, out int value))
                {
                    actor.WarehouseId = (PokemonFixedWarehouseId) value;
                }
                actorsStore.SetValue(iter, WarehouseIdColumn, FormatWarehouseId(actor.WarehouseId));
            }
        }

        private void OnSpecialNameEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (actorsStore!.GetIter(out var iter, path))
            {
                var actor = actors!.Actors[path.Indices[0]];
                if (int.TryParse(args.NewText, out int value))
                {
                    actor.SpecialName = (TextIDHash) value;
                }
                actorsStore.SetValue(iter, SpecialNameColumn, (int) actor.SpecialName);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var actor = new ActorData { WarehouseId = PokemonFixedWarehouseId.NONE };
            actors!.Actors.Add(actor);
            AddActorToStore(actor, actors.Actors.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (actorsTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                actors!.Actors.RemoveAt(index);
                (model as ListStore)!.Remove(ref iter);
            }
        }

        private string FormatWarehouseId(PokemonFixedWarehouseId id)
        {
            return id == PokemonFixedWarehouseId.NONE ? "None" : ((int) id).ToString();
        }
    }
}
