using UnityEngine;

namespace Cubra
{
    public class River : CollisionObjects
    {
        [Header("Брызги воды")]
        [SerializeField] private ParticleSystem _spray;

        // Ссылка на компонент звука брызг
        private PlayingSound _splashingSound;

        protected override void Awake()
        {
            base.Awake();
            _splashingSound = _spray.gameObject.GetComponent<PlayingSound>();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            // Получаем компонент объекта в воде
            var thing = collision.gameObject.GetComponent<ObjectInRiver>();

            if (thing)
            {
                // Перемещаем объект вних слоя
                thing.SpriteRenderer.sortingOrder = 0;
                // Обновляем массу объекта в воде
                thing.Rigidbody.mass = thing.MassAfloat;

                // Перемещаем эффект брызг к объекту и воспроизводим
                _spray.transform.position = new Vector3(thing.Transform.position.x, thing.Transform.position.y - 0.5f, 0);
                _spray.Play();

                // Воспроизводим звук
                _splashingSound.PlaySound();
            }
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
                // Наносим урон персонажу без анимации смерти
                Main.Instance.CharacterController.DamageToCharacter(false, false);

            // Переносим персонажа вниз слоя
            character.SpriteRenderer.sortingOrder = 0;
            // Перемещаем эффект брызг к персонажу и воспроизводим
            _spray.transform.position = character.Transform.position + Vector3.down / 1.5f;
            _spray.Play();

            // Воспроизводим звук
            _splashingSound.PlaySound();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // Получаем компонент объекта в воде
            var thing = collision.gameObject.GetComponent<ObjectInRiver>();

            if (thing)
            {
                thing.InstanseObject.SetActive(false);
            }
        }
    }
}