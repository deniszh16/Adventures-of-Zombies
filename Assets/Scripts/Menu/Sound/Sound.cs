using UnityEngine;

namespace Cubra
{
    public abstract class Sound : MonoBehaviour
    {
        protected AudioSource _audioSource;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}