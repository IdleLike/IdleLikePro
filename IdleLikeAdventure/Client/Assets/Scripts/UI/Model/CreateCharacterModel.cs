using System;
using System.Collections;
using System.Collections.Generic;

namespace UI.Model
{
    public class CreateCharacterViewModel
    {
        public struct CharacterViewModel
        {
            public uint raceID { get; set; }
            public string raceName { get; set; }
            public string raceDes { get; set; }
            public uint initHP { get; set; }
            public uint initCon { get; set; }
            public uint initPow { get; set; }
            public uint initDex { get; set; }
            public uint growthHP { get; set; }
            public uint growthCon { get; set; }
            public uint growthPow { get; set; }
            public uint growthDex { get; set; }
            public uint raceAbilityOne { get; set; }
            public uint raceAbilityTwo { get; set; }
        }
        public Action<CreateCharacterPanel.CreateData> CreateCharacterCallback;

        public Func<string, bool> NameIsRepeatCallback;

        private List<CharacterViewModel> m_createCharacterViewModels;

        public List<CharacterViewModel> createCharacterViewModels
        {
            get
            {
                return m_createCharacterViewModels;
            }

            set
            {
                m_createCharacterViewModels = value;
            }
        }

    }
    

}
