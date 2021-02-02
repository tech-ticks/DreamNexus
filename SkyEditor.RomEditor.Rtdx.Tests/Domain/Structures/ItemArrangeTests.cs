using FluentAssertions;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class ItemArrangeTests
    {
        [Fact]
        public void CanBuildItemArrange()
        {
            // Arrange
            var itemArrange = new ItemArrange();

            var entry1 = new ItemArrange.Entry();

            var kindWeights1 = new ushort[(int) ItemKind.MAX];
            kindWeights1[0] = 100;
            kindWeights1[5] = 300;
            var itemSet1 = new ItemArrange.Entry.ItemSet(
                kindWeights1,
                new List<ItemArrange.Entry.ItemWeightEntry>
                {
                    new ItemArrange.Entry.ItemWeightEntry() { Index = ItemIndex.SEED_OREN, Weight = 200 },
                    new ItemArrange.Entry.ItemWeightEntry() { Index = ItemIndex.SEED_FUKKATSU, Weight = 10 },
                    new ItemArrange.Entry.ItemWeightEntry() { Index = ItemIndex.SEED_BLIND, Weight = 50 },
                }
            );

            var kindWeights2 = new ushort[(int) ItemKind.MAX];
            kindWeights2[1] = 10;
            kindWeights2[((int) ItemKind.MAX - 1)] = 500;
            var itemSet2 = new ItemArrange.Entry.ItemSet(
                kindWeights2,
                new List<ItemArrange.Entry.ItemWeightEntry>
                {
                    new ItemArrange.Entry.ItemWeightEntry() { Index = ItemIndex.ARROW_SILVER, Weight = 100 },
                    new ItemArrange.Entry.ItemWeightEntry() { Index = ItemIndex.EVOLUTION_COMMON, Weight = 5 },
                }
            );

            entry1.ItemSets.Add(itemSet1);
            entry1.ItemSets.Add(itemSet2);

            var entry2 = new ItemArrange.Entry();

            itemArrange.Entries = new ItemArrange.Entry[] { entry1, entry2 };

            // Act
            var (bin, ent) = itemArrange.Build();
            var rebuiltItemArrange = new ItemArrange(bin, ent);

            // Assert
            rebuiltItemArrange.Entries.Should().HaveCount(2);

            var entry = rebuiltItemArrange.Entries[0];
            entry.ItemSets.Should().HaveCount(2);
            entry.ItemSets[0].ItemKindWeights.Should().HaveCount((int) ItemKind.MAX);
            entry.ItemSets[0].ItemKindWeights[0].Should().Be(100);
            entry.ItemSets[0].ItemKindWeights[5].Should().Be(300);
            entry.ItemSets[0].ItemWeights.Should().HaveCount(3);
            entry.ItemSets[0].ItemWeights[0].Index.Should().Be(ItemIndex.SEED_OREN);
            entry.ItemSets[0].ItemWeights[2].Weight.Should().Be(50);

            entry.ItemSets[1].ItemKindWeights[1].Should().Be(10);
            entry.ItemSets[1].ItemKindWeights[((int) ItemKind.MAX - 1)].Should().Be(500);
            entry.ItemSets[1].ItemWeights.Should().HaveCount(2);
            entry.ItemSets[1].ItemWeights[0].Index.Should().Be(ItemIndex.ARROW_SILVER);
            entry.ItemSets[1].ItemWeights[1].Weight.Should().Be(5);
        }
    }
}
