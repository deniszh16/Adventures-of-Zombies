using Cubra.Controllers;

namespace Cubra {
    public class PlayingSound : Sound
    {
        /// <summary>
        /// Воспроизведение звука
        /// </summary>
        public void PlaySound()
        {
            if (SoundController.PlayingSounds)
            {
                _audioSource.Play();
            }
        }
    }
}