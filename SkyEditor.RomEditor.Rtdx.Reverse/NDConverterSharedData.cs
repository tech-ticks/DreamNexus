using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public class NDConverterSharedData
    {
        public enum NatureType
        {
            Hardly,
            Docile,
            Brave,
            Jolly,
            Implish,
            Naive,
            Timid,
            Hasty,
            Sassy,
            Calm,
            Relaxed,
            Lonely,
            Quirky,
            End
        }

        public class DataStore
        {
            public List<DiagnosisStrage> m_diagnosisStrageList { get; set; }
            public List<PokemonStrage> m_pokemonNatureAndTypeList { get; set; }
        }

        [Serializable]
        public class DiagnosisStrage
        {
            public List<AnswerStrage> m_answerStrageList { get; set; }
            public string m_question { get; set; }
            public string m_questionType { get; set; }
        }

        [Serializable]
        public class AnswerStrage
        {
            public string m_answer { get; set; }
            public List<NaturePoint> m_addNatureList { get; set; }
        }

        [Serializable]
        public class NaturePoint
        {
            public string m_nature { get; set; }
            public int m_addPoint { get; set; }
        }

        [Serializable]
        public class PokemonStrage
        {
            public string m_name { get; set; }
            public Const.creature.Index m_nameLabel { get; set; }
            public NatureType m_maleNature { get; set; }
            public NatureType m_femaleNature { get; set; }
            public string m_typeA { get; set; }
            public string m_symbolName { get; set; }
            public string m_symbolNameFemale { get; set; }
            public int m_defaultPos { get; set; }
            public int m_pos1 { get; set; }
            public int m_pos2 { get; set; }
            public int m_pos3 { get; set; }

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

        public NDConverterSharedData()
        {
        }
    }
}
