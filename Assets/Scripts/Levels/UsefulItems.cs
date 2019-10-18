using UnityEngine;

public class UsefulItems : MonoBehaviour
{
    [Header("Тип объекта")]
    [SerializeField] private string type;

    // Ссылки на компоненты
    private AudioSource audioSource;
    private SpriteRenderer sprite;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.GetComponent<Character>();

        // Если персонаж живой
        if (character.Life)
        {
            switch (type)
            {
                // Монета
                case "coin":
                    // Увеличиваем количество монет
                    character.Parameters.Coins++;
                    break;
                // Мозги
                case "brain":
                    // Уменьшаем количество мозгов
                    character.Parameters.Brains--;
                    // Обновляем статистику
                    character.Parameters.RefreshQuantityBrains();
                    break;
            }

            // Если звуки не отключены, проигрываем
            if (Options.sound) audioSource.Play();

            // Отключаем коллайдер объекта
            circleCollider.enabled = false;
            // Скрываем изображение
            sprite.enabled = false;
        }
    }
}