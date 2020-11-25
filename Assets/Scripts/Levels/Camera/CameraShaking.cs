using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Cubra
{
    public class CameraShaking : MonoBehaviour
    {
        [Header("Виртуальная камера")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        
        private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

        private void Awake()
        {
            _virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        /// <summary>
        /// Дрожание камеры
        /// </summary>
        /// <param name="duration">продолжительность</param>
        /// <param name="amplitude">амплитуда</param>
        /// <param name="frequency">частота</param>
        public IEnumerator ShakeCamera(float duration, float amplitude, float frequency)
        {
            // Устанавливаем амплитуду и частоту дрожания
            _virtualCameraNoise.m_AmplitudeGain = amplitude;
            _virtualCameraNoise.m_FrequencyGain = frequency;

            yield return new WaitForSeconds(duration);

            _virtualCameraNoise.m_AmplitudeGain = 0;
            _virtualCameraNoise.m_FrequencyGain = 0;
        }
    }
}