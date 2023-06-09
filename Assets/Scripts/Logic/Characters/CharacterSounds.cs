using UnityEngine;

namespace Logic.Characters
{
    public class CharacterSounds : MonoBehaviour
    {
        [Header("Компонент звука")]
        [SerializeField] private AudioSource _audioSource;
        
        [Header("Звуки персонажа")]
        [SerializeField] private AudioClip[] _audioClips;

        public void SetSound(Sounds sound) =>
            _audioSource.clip = _audioClips[(int)sound];

        public void PlaySound() =>
            _audioSource.Play();
    }
}