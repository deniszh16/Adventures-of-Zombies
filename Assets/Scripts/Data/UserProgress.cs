﻿using System;

namespace Data
{
    [Serializable]
    public class UserProgress
    {
        public int Progress;
        public int TotalScore;
        public int[] Stars;

        public int Bones;
        public int Brains;

        public int Played;
        public int Deaths;

        public int DestroyedBarrel;

        public LanguageData LanguageData;
        public SoundData SoundData;

        public UserProgress()
        {
            Progress = 1;
            Stars = new int[11];
            LanguageData = new LanguageData();
            SoundData = new SoundData();
        }
    }
}