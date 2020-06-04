using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class NatureDiagnosisConfiguration
    {
#pragma warning disable IDE1006 // Naming Styles
        public List<DiagnosisStrage> m_diagnosisStrageList { get; set; } = default!;
        public List<PokemonStrage> m_pokemonNatureAndTypeList { get; set; } = default!;

        [Serializable]
        public class DiagnosisStrage
        {
            public List<AnswerStrage> m_answerStrageList { get; set; } = default!;
            public string m_question { get; set; } = default!;
            public string m_questionType { get; set; } = default!;
        }

        [Serializable]
        public class AnswerStrage
        {
            public string m_answer { get; set; } = default!;
            public List<NaturePoint> m_addNatureList { get; set; } = default!;
        }

        [Serializable]
        public class NaturePoint
        {
            public string m_nature { get; set; } = default!;
            public int m_addPoint { get; set; }
        }

        [Serializable]
        public class PokemonStrage
        {
            public string m_name { get; set; } = default!;

            [JsonConverter(typeof(StringEnumConverter))]
            public CreatureIndex m_nameLabel { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public NatureDiagnosisNatureType m_maleNature { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public NatureDiagnosisNatureType m_femaleNature { get; set; }
            public string m_typeA { get; set; } = default!;
            public string m_symbolName { get; set; } = default!;
            public string m_symbolNameFemale { get; set; } = default!;
            public int m_defaultPos { get; set; } = default!;
            public int m_pos1 { get; set; }
            public int m_pos2 { get; set; }
            public int m_pos3 { get; set; }
#pragma warning restore IDE1006 // Naming Styles

            public PokemonStrage()
            {
            }

            public PokemonStrage(PokemonStrage strage)
            {
                this.m_name = strage.m_name;
                this.m_nameLabel = strage.m_nameLabel;
                this.m_maleNature = strage.m_maleNature;
                this.m_femaleNature = strage.m_femaleNature;
                this.m_typeA = strage.m_typeA;
                this.m_symbolName = strage.m_symbolName;
                this.m_symbolNameFemale = strage.m_symbolNameFemale;
                this.m_defaultPos = strage.m_defaultPos;
                this.m_pos1 = strage.m_pos1;
                this.m_pos2 = strage.m_pos2;
                this.m_pos3 = strage.m_pos3;
            }
        }
    }
}
