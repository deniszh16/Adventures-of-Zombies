using UnityEngine;

public class DamageObjects : MonoBehaviour
{
    [Header("Уничтожение стрел")]
    public bool destroyArrow = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.gameObject.GetComponent<Character>();

        // Если персонаж живой
        if (character && character.Life)
        {
            // Если звуки не отключены и персонаж жив, проигрываем звук
            if (Options.sound) character.AudioSource.Play();

            // Перемещаем эффект урона к персонажу и воспроизводим
            character.blood.transform.position = character.transform.position;
            character.blood.Play();

            // Наносим урон персонажу с отскоком и анимацией смерти
            character.RecieveDamageCharacter(true, true, 1.5f);
        }
    }
}