using System.Collections;
using UnityEngine;

namespace Cubra
{
    public abstract class Boss : MonoBehaviour
    {
        // Режим активности
        protected string _mode;

        // Количество атакующих забегов
        protected int _quantityRuns;

        [Header("Здоровье босса")]
        [SerializeField] protected int _health;

        // Скорость бега
        protected float _speed;

        // Направление движения
        protected Vector3 _direction;

        [Header("Эффект звезд")]
        [SerializeField] protected GameObject _effectStars;

        [Header("Падающие камни")]
        [SerializeField] protected GameObject[] _stones;

        [Header("Препятствия на уровне")]
        [SerializeField] protected GameObject[] _obstacles;

        [Header("Финишные объекты")]
        [SerializeField] protected GameObject[] _objectsFinish;

        protected Animator _animator;
        protected SpriteRenderer _spriteRenderer;
        protected CapsuleCollider2D _capsuleCollider;
        protected Rigidbody2D _rigbody;

        protected Character _character;
        protected CameraShaking _cameraShaking;

        protected void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _rigbody = GetComponent<Rigidbody2D>();

            _character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            _cameraShaking = Camera.main.GetComponent<CameraShaking>();
        }

        protected virtual void Start()
        {
            _direction = Vector3.left;
        }

        protected virtual void FixedUpdate()
        {
            if (_mode == "run") Run();

            // Если персонаж умер
            if (_character.Life == false)
            {
               _mode = "none";
               // Останавливаем переключение режима
               StopAllCoroutines();
            }
        }

        /// <summary>
        /// Определение случайного количества забегов
        /// </summary>
        protected void SetQuantityRun()
        {
            _quantityRuns = Random.Range(2, 4);
        }

        /// <summary>
        /// Переключение режимов босса
        /// </summary>
        /// <param name="seconds">время до переключения</param>
        /// <param name="mode">режим активности</param>
        protected abstract IEnumerator SwitchMode(float seconds, string mode);

        /// <summary>
        /// Определение следующего режима активности босса
        /// </summary>
        protected abstract void DefineNextMode();

        /// <summary>
        /// Бег босса к персонажу
        /// </summary>
        protected void Run()
        {
            // Перемещаем босса в указанном направлении с указанной скоростью
            transform.Translate(_direction * _speed * Time.deltaTime);
        }

        /// <summary>
        /// Определение направления движения
        /// </summary>
        /// <param name="invert">горизонтальное отображение спрайта</param>
        protected void SetDirection(bool invert = false)
        {
            // Сравниваем позиции босса с персонажем и устанавливаем направление движения
            _direction = (transform.localPosition.x > _character.transform.localPosition.x) ? Vector3.left : Vector3.right;

            if (!invert)
                // Отображаем спрайт в установленном направлении
                _spriteRenderer.flipX = (_direction == Vector3.left) ? true : false;
            else
                // Если установлена инверсия, разворачиваем спрайт
                _spriteRenderer.flipX = (_direction == Vector3.left) ? false : true;
        }

        /// <summary>
        /// Падение камней на уровне
        /// </summary>
        protected void Rockfall()
        {
            if (_mode == "attack")
            {
                // Вызываем встряхивание камеры
                StartCoroutine(_cameraShaking.ShakeCamera(0.7f, 1.8f, 1.6f));
                // Активируем камни для падения
                for (int i = 0; i < _stones.Length; i++) _stones[i].SetActive(true);
            }
        }

        /// <summary>
        /// Отображение дополнительных препятствий
        /// </summary>
        /// <param name="health">здоровье босса</param>
        protected abstract void ShowObstacles(int health);

        /// <summary>
        /// Отображение финишных объектов
        /// </summary>
        protected void ShowFinish()
        {
            for (int i = 0; i < _objectsFinish.Length; i++)
                _objectsFinish[i].SetActive(true);

            // Отключаем фиксацию камеры
            GameManager.Instance.DisableCameraLock();
        }
    }
}