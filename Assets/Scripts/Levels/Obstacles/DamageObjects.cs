using UnityEngine;

public class DamageObjects : MonoBehaviour
{
    [Header("Уничтожение стрел")]
    [SerializeField] private bool destroyArrow = true;

    public bool DestroyArrow { get { return destroyArrow; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.gameObject.GetComponent<Character>();

        // Если персонаж живой
        if (character && character.Life)
        {
            // Если звуки не отключены, проигрываем звук
            if (Options.sound) character.AudioSource.Play();

            // Перемещаем эффект урона к персонажу и воспроизводим
            character.Blood.transform.position = character.transform.position;
            character.Blood.Play();

            // Наносим урон персонажу с отскоком и анимацией смерти
            character.RecieveDamageCharacter(true, true, 1.5f);
        }
    }
}