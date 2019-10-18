using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Капли воды")]
    [SerializeField] private ParticleSystem spray;

    // Ссылка на звуковой компонент
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у коснувшегося объекта
        var character = collision.gameObject.GetComponent<Character>();

        if (character)
        {
            // Переносим персонажа вниз слоя
            character.Sprite.sortingOrder = 0;
            // Перемещаем эффект брызг к персонажу и воспроизводим
            spray.transform.position = character.transform.position + Vector3.down / 2;
            spray.Play();

            // Если персонаж живой
            if (character.Life)
                // Уничтожаем его
                character.RecieveDamageCharacter(false, false, 2.5f);

            // Если звуки не отключены, проигрываем
            if (Options.sound) audioSource.Play();
        }
        else
        {
            // Иначе пробуем получить водный компонент
            var thing = collision.gameObject.GetComponent<InWater>();

            if (thing)
            {
                // Переносим объект вниз слоя
                thing.Sprite.sortingOrder = 0;
                // Обновляем массу объекта
                thing.Rigbody.mass = thing.Mass;

                if (thing.Spray)
                {
                    // Перемещаем эффект брызг к объекту и воспроизводим его
                    spray.transform.position = new Vector2(thing.transform.position.x, thing.transform.position.y - 0.5f);
                    spray.Play();
                }

                // Если звуки не отключены, воспроизводим
                if (Options.sound && thing.PlayingSound) audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var thing = collision.gameObject.GetComponent<InWater>();

        if (thing)
            // Отключаем объект
            collision.gameObject.SetActive(false);
    }
}