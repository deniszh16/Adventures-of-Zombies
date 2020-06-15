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
                if (thing.SpriteRenderer) thing.SpriteRenderer.sortingOrder = 0;
                thing.Rigidbody.mass = thing.MassAfloat;

                _spray.transform.position = new Vector3(thing.Transform.position.x, thing.Transform.position.y - 0.5f, 0);
                _spray.Play();

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
                Main.Instance.CharacterController.DamageToCharacter(false, false);

            character.SpriteRenderer.sortingOrder = 0;

            _spray.transform.position = character.Transform.position + Vector3.down / 1.5f;
            _spray.Play();

            _splashingSound.PlaySound();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var thing = collision.gameObject.GetComponent<ObjectInRiver>();

            if (thing) thing.InstanseObject.SetActive(false);
        }
    }
}