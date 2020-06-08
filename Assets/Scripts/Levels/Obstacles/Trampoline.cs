using UnityEngine;

namespace Cubra
{
    public class Trampoline : ReboundObject
    {
        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                // Сбрасываем скорость объекта
                character.Rigidbody.velocity = Vector2.zero;
                // Создаем импульсный отскок персонажа
                character.Rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);

                // Воспроизводим звук
                _playingSound.PlaySound();

                // Перезапускаем анимацию пружины
                _animator.enabled = true;
                _animator.Rebind();
            }
        }
    }
}