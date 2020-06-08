using System.Collections;
using UnityEngine;

namespace Cubra
{
    public class Arrow : CollisionObjects
    {
        // Активность полета
        private bool _flight;

        // Скорость полета стрелы
        private float _speed;

        // Стандартная скорость полета стрелы
        private readonly float _standardSpeed = 15f;

        // Стандартный слой объекта
        private int _sortingOrder;

        // Направление стрелы
        private Vector3 _direction;

        // Ссылки на компоненты
        private BoxCollider2D _boxcollider;
        private Rigidbody2D _rigidbody;
        private FixedJoint2D _joint;

        protected override void Awake()
        {
            base.Awake();

            _boxcollider = InstanseObject.GetComponent<BoxCollider2D>();
            _rigidbody = InstanseObject.GetComponent<Rigidbody2D>();
            _joint = InstanseObject.GetComponent<FixedJoint2D>();

            // Получаем слой объекта
            _sortingOrder = SpriteRenderer.sortingOrder;
        }

        private void OnEnable()
        {
            // Преобразуем локальный вектор направления в мировой
            _direction = Transform.TransformDirection(Vector3.down);

            // Активируем полет
            _flight = true;

            // Активируем коллайдер
            _boxcollider.enabled = true;

            // Восстанавливаем слой объекта
            SpriteRenderer.sortingOrder = _sortingOrder;

            // Восстанавливаем скорость
            _speed = _standardSpeed;
        }

        private void FixedUpdate()
        {
            if (_flight)
            {
                // Если полет активен, выполняется движение в указанном направлении
                _rigidbody.MovePosition(Transform.position + (_direction * _speed * Time.fixedDeltaTime));
            }
        }

        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                // Наносим урон персонажу с анимацией смерти и отскоком
                Main.Instance.CharacterController.DamageToCharacter(true, true);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            // Если стрела касается поверхности
            if (collision.gameObject.layer == (LayerMask.GetMask("Surface") >> 5))
            {
                // Отключаем коллайдер
                _boxcollider.enabled = false;

                // Понижаем слой объекта
                SpriteRenderer.sortingOrder--;

                // Пробуем получить физический компонент у поверхности
                var physics = collision.gameObject.GetComponent<Rigidbody2D>();

                if (physics)
                {
                    // Убираем массу
                    _rigidbody.mass = 0;
                    // Указываем цель для фиксации стрелы
                    _joint.connectedBody = physics;
                }

                if (InstanseObject.activeInHierarchy)
                {
                    // Запускаем остановку стрелы
                    _ = StartCoroutine(StopFlight(0.03f, physics));
                }
                
                return;
            }

            // Если стрела попадает в другое препятствие
            if (collision.gameObject.GetComponent<SharpObstacles>())
            {
                InstanseObject.SetActive(false);
                return;
            }

            // Если стрела попала в реку
            if (collision.gameObject.GetComponent<River>())
            {
                _speed /= 2;
            }
        }

        /// <summary>
        /// Остановка полета стрелы
        /// </summary>
        /// <param name="seconds">секунды до остановки</param>
        /// <param name="fixation">фиксация стрелы на объекте</param>
        private IEnumerator StopFlight(float seconds, bool fixation)
        {
            yield return new WaitForSeconds(seconds);
            _flight = false;

            if (fixation)
            {
                // Закрепляем стрелу на объекте
                _joint.enabled = true;
            }

            _ = StartCoroutine(TurnOffObject());
        }

        /// <summary>
        /// Отключение стрелы после использования
        /// </summary>
        private IEnumerator TurnOffObject()
        {
            yield return new WaitForSeconds(1.5f);

            // Сбрасываем угол объекта
            Transform.localEulerAngles = Vector3.zero;
            // Отключаем объект
            InstanseObject.SetActive(false);
        }
    }
}