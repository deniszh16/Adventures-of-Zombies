using System;

namespace Data
{
    [Serializable]
    public class SoundData
    {
        public bool Activity;

        public SoundData() =>
            Activity = true;

        public void SetSoundActivity(bool value) =>
            Activity = value;
    }
}