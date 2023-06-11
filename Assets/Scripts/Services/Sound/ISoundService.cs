using System;

namespace Services.Sound
{
    public interface ISoundService
    {
        public bool SoundActivity { get; set; }
        
        public event Action SoundChanged;

        public void SwitchSound();
        public void StartBackgroundMusicInMenu();
        public void StartBackgroundMusicOnLevels();
        public void ChangeVolume();
        public void SetPause(bool pause);
    }
}