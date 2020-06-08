using UnityEngine;

namespace Cubra
{
    public class Barrel : CollisionObjects
    {
        // Активность бочки
        private bool _active;

        [Header("Эффект взрыва")]
        [SerializeField] private Animator _destruction;

        // Ссылки на компоненты
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                if (_active == false)
                {
                    // Активируем анимацию
                    _animator.enabled = true;
                    _active = true;
                }
            }
        }

        /// <summary>
        /// Уничтожение бочки
        /// </summary>
        public void DestroyBarrel()
        {
            // Перемещаем эффект взрыва к бочке
            _destruction.transform.position = Transform.position;

            if (_destruction.enabled == false) _destruction.enabled = true;
            // Перезапускаем анимацию
            _destruction.Rebind();

            // Увеличиваем число уничтоженных бочек
            PlayerPrefs.SetInt("barrel", PlayerPrefs.GetInt("barrel") + 1);

            InstanseObject.SetActive(false);
        }
    }
}