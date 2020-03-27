using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System.Diagnostics;
using SkyEditor.RomEditor.Rtdx.Reverse.Const;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class PokemonGraphicsDatabase
    {
        const int entrySize = 0xB0;

        public PokemonGraphicsDatabase(byte[] data)
        {
			var sir0 = new Sir0(data);
            var indexOffset = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 8);
            var entryCount = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 16);
            var entries = new List<PokemonGraphicsDatabaseEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new PokemonGraphicsDatabaseEntry(data, indexOffset + (i * entrySize)));
            }
            this.Entries = entries;
        }

        public IReadOnlyList<PokemonGraphicsDatabaseEntry> Entries { get; }

        [DebuggerDisplay("PokemonGraphicsDatabaseEntry: {String1}|{String2}|{String3}|{String4}|{String5}|{String6}")]
        public class PokemonGraphicsDatabaseEntry
        {
            public PokemonGraphicsDatabaseEntry(byte[] data, int index)
            {
                this.Data = new byte[entrySize];
                Array.Copy(data, index, this.Data, 0, entrySize);

                var accessor = new BinaryFile(data);
                String1 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 0));
                String2 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 8));
                String3 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 16));
                String4 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 24));
                String5 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 32));
                String6 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 40));
            }

            public byte[] Data { get; }

            public string String1 { get; set; }
            public string String2 { get; set; }
            public string String3 { get; set; }
            public string String4 { get; set; }
            public string String5 { get; set; }
            public string String6 { get; set; }
        }

		// This is the likely structure of PokemonGraphicsDatabaseEntry, but more investigation is required
		private class CharacterDatabaseParameter
		{
			public string? stFileName;
			public string? stCommonMotionName;
			public string? stCommonMotionName2;
			public string? stTextureName;
			public string? stCutPictureName;
			public string? stCutInName;
			public string? stFaceName;
			public string? stDotName;
			public string? stDotRightName;
			public uint gfxSymbol;
			public int sheetId;
			public EvolutionCameraType evolutionCameraType;
			public GraphicsBodySizeType bodySize;
			public float fBaseScale;
			public float fDungeonBaseScale;
			public float fPGRootBoneScale;
			public float fDotOffsetX;
			public float fDotOffsetY;
			public float fMotionScale;
			public float fWalkSpeedDist;
			public float fWalkSpeedDistG;
			public float fRunSpeedRatioG;
			public float fWalkCorrectionValueG;
			public float fRunCorrectionValueG;
			public float fFlyHeight;
			public float fShadowW;
			public float fShadowH;
			public float fNullHeadOffsetX;
			public float fNullHeadOffsetY;
			public float fNullHeadOffsetZ;
			public float fNullMouthOffsetX;
			public float fNullMouthOffsetY;
			public float fNullMouthOffsetZ;
			public float fNullRightHandOffsetX;
			public float fNullRightHandOffsetY;
			public float fNullRightHandOffsetZ;
			public float fNullLeftHandOffsetX;
			public float fNullLeftHandOffsetY;
			public float fNullLeftHandOffsetZ;
			public float fBDMinX;
			public float fBDMinY;
			public float fBDMinZ;
			public float fBDMaxX;
			public float fBDMaxY;
			public float fBDMaxZ;
			public bool bEnablePGRootBoneScaleDedicatedMotion;
			public bool bFace_HANTEN;
			public bool bFace_FEMALE;
			public bool bBig;
			public bool bEffectBaseSet_CH_OffsetPokemon;
			public List<string>? animationPartsList;
		}
	}
}
