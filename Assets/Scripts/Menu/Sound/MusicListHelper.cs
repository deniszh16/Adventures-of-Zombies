using UnityEngine;

namespace Cubra.Helpers
{
    public class MusicListHelper : MonoBehaviour
    {
        [Header("Фоновая музыка")]
        [SerializeField] private AudioClip[] _audioClips;

        public AudioClip[] AudioClips => _audioClips;
    }
}