using Cubra.Controllers;

namespace Cubra {
    public class PlayingSound : Sound
    {
        public void PlaySound()
        {
            if (SoundController.PlayingSounds)
            {
                _audioSource.Play();
            }
        }
    }
}