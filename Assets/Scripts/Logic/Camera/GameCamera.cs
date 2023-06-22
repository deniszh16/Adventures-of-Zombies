using System.Collections;
using Cinemachine;
using Logic.Characters;
using UnityEngine;

namespace Logic.Camera
{
    public class GameCamera : MonoBehaviour
    {
        [Header("Виртуальная камера")]
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;

        private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

        private void Awake() =>
            _virtualCameraNoise = _cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        public void SnapCameraToTarget(Character character) =>
            _cinemachineVirtual.Follow = character.Life ? character.transform : null;
        
        public void DisableCameraLock() =>
            _cinemachineVirtual.AddCinemachineComponent<CinemachineFramingTransposer>();

        public void ShakeCamera(float duration, float amplitude, float frequency) =>
            _ = StartCoroutine(ShakeCameraCoroutine(duration, amplitude, frequency));

        private IEnumerator ShakeCameraCoroutine(float duration, float amplitude, float frequency)
        {
            _virtualCameraNoise.m_AmplitudeGain = amplitude;
            _virtualCameraNoise.m_FrequencyGain = frequency;
            
            yield return new WaitForSeconds(duration);
            
            _virtualCameraNoise.m_AmplitudeGain = 0;
            _virtualCameraNoise.m_FrequencyGain = 0;
        }
    }
}