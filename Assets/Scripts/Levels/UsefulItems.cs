using UnityEngine;

public class UsefulItems : MonoBehaviour
{
    [Header("Тип объекта")]
    [SerializeField] private string type;

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
        if (character && character.Life)
        {
            switch (type)
            {
                // Если это монета
                case "coin":
                    // Увеличиваем количество монет
                    character.Parameters.Coins++;
                    break;
                // Если это мозг
                case "brain":
                    // Уменьшаем количество мозгов
                    character.Parameters.Brains--;
                    // Обновляем статистику на экране
                    character.Parameters.RefreshQuantityBrains();
                    break;
                // Если это таймер
                case "seconds":
                    // Увеличиваем количество секунд
                    character.Parameters.seconds += 5;
                    // Обновляем цвет таймера
                    character.Parameters.ChangeTextColor();
                    break;
            }

            // Если звуки не отключены, проигрываем
            if (Options.sound) audioSource.Play();

            // Отключаем коллайдер и изображение
            circleCollider.enabled = false;
            sprite.enabled = false;
        }
    }
}