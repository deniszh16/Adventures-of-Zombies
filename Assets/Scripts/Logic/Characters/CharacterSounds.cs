using Services.Sound;
using UnityEngine;

namespace Logic.Characters
{
    public class CharacterSounds : MonoBehaviour
    {
        [Header("Компоненты звука")]
        [SerializeField] private PlayingSound _playingSound;
        [SerializeField] private AudioSource _audioSource;

        [Header("Звуки персонажа")]
        [SerializeField] private AudioClip[] _audioClips;

        public void SetSound(Sounds sound) =>
            _audioSource.clip = _audioClips[(int)sound];

        public void PlaySound() =>
            _playingSound.PlaySound();
    }
}