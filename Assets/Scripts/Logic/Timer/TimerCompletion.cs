using Logic.Characters;
using UnityEngine;

namespace Logic.Timer
{
    public class TimerCompletion : MonoBehaviour
    {
        [Header("Компонент персонажа")]
        [SerializeField] private Character _character;

        [Header("Компонент таймера")]
        [SerializeField] private Timer _timer;

        private void Awake()
        {
            _timer.TimerCompleted += _character.DamageToCharacter;
            _timer.TimerCompleted += _character.ShowDeathAnimation;
            _timer.TimerCompleted += _character.DisableCollider;
        }

        private void OnDestroy()
        {
            _timer.TimerCompleted -= _character.DamageToCharacter;
            _timer.TimerCompleted -= _character.ShowDeathAnimation;
            _timer.TimerCompleted -= _character.DisableCollider;
        }
    }
}