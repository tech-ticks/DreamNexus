using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class NatureDiagnosis
    {
        public List<DiagnosisStrage> m_diagnosisStrageList { get; set; } = default!;
        public List<PokemonNatureAndType> m_pokemonNatureAndTypeList { get; set; } = default!;

        public class DiagnosisStrage 
        {
            public List<AnswerStrage> m_answerStrageList { get; set; } = default!;
            public string m_question { get; set; } = default!;
            public string m_questionType { get; set; } = default!;

            public class AnswerStrage
            {
                public string m_answer { get; set; } = default!;
                public List<AddNature> m_addNatureList { get; set; } = default!;

                public class AddNature
                {
                    public string m_nature { get; set; } = default!;
                    public int m_addPoint { get; set; }
                }
            }
        }

        public class PokemonNatureAndType
        {
            public string m_name { get; set; } = default!;
            public string m_nameLabel { get; set; } = default!;
            public string m_maleNature { get; set; } = default!;
            public string m_femaleNature { get; set; } = default!;
            public string m_typeA { get; set; } = default!;
            public string m_symbolName { get; set; } = default!;
            public string m_symbolNameFemale { get; set; } = default!;
            public int m_defaultPos { get; set; }
            public int m_pos1 { get; set; }
            public int m_pos2 { get; set; }
            public int m_pos3 { get; set; }
        }
    }
}
