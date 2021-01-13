using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class FixedMapTests
    {
        [Fact]
        public void CanBuildFixedMap()
        {
            // Arrange
            var fixedMap = new FixedMap();
            var entry = new FixedMap.Entry(16, 12);
            fixedMap.Entries.Add(entry);
            for (var i = 0; i < 16 * 12; i++)
            {
                entry.Tiles[i].RoomId = (byte)i;
                entry.Tiles[i].Type = FixedMap.FixedMapTile.TileType.Floor;
                entry.Tiles[i].Byte01 = (byte)~i;
            }

            var creature = new FixedMap.FixedMapCreature();
            entry.Creatures.Add(creature);
            creature.XPos = 10;
            creature.YPos = 20;
            creature.Byte02 = 2;
            creature.Byte03 = 3;
            creature.Byte07 = 7;
            creature.Index = FixedCreatureIndex.FIXEDMAP_PARTNER;
            creature.Direction = FixedMap.EntityDirection.Left;
            creature.Faction = FixedMap.FixedMapCreature.CreatureFaction.Ally;

            var item = new FixedMap.FixedMapItem();
            entry.Items.Add(item);
            item.XPos = 11;
            item.YPos = 22;
            item.Byte02 = 12;
            item.Byte03 = 13;
            item.Byte07 = 17;
            item.FixedItemIndex = 234;
            item.Variation = FixedMap.FixedMapItem.ItemVariation.Item;

            // Act
            var (bin, ent) = fixedMap.Build();
            var rebuiltMap = new FixedMap(bin, ent);

            // Assert
            rebuiltMap.Entries.Should().HaveCount(1);
            var rebuiltEntry = rebuiltMap.Entries[0];
            rebuiltEntry.Width.Should().Be(16);
            rebuiltEntry.Height.Should().Be(12);
            for (var i = 0; i < 16 * 12; i++)
            {
                rebuiltEntry.Tiles[i].RoomId.Should().Be((byte)i);
                rebuiltEntry.Tiles[i].Type.Should().Be(FixedMap.FixedMapTile.TileType.Floor);
                rebuiltEntry.Tiles[i].Byte01.Should().Be((byte)~i);
            }

            rebuiltEntry.Creatures.Should().HaveCount(1);
            var rebuiltCreature = rebuiltEntry.Creatures[0];
            rebuiltCreature.XPos.Should().Be(10);
            rebuiltCreature.YPos.Should().Be(20);
            rebuiltCreature.Byte02.Should().Be(2);
            rebuiltCreature.Byte03.Should().Be(3);
            rebuiltCreature.Byte07.Should().Be(7);
            rebuiltCreature.Index.Should().Be(FixedCreatureIndex.FIXEDMAP_PARTNER);
            rebuiltCreature.Direction.Should().Be(FixedMap.EntityDirection.Left);
            rebuiltCreature.Faction.Should().Be(FixedMap.FixedMapCreature.CreatureFaction.Ally);

            rebuiltEntry.Items.Should().HaveCount(1);
            var rebuiltItem = rebuiltEntry.Items[0];
            rebuiltItem.XPos.Should().Be(11);
            rebuiltItem.YPos.Should().Be(22);
            rebuiltItem.Byte02.Should().Be(12);
            rebuiltItem.Byte03.Should().Be(13);
            rebuiltItem.Byte07.Should().Be(17);
            rebuiltItem.FixedItemIndex.Should().Be(234);
            rebuiltItem.Variation.Should().Be(FixedMap.FixedMapItem.ItemVariation.Item);
        }
    }
}
