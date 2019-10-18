using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.GetComponent<Character>();

        if (character)
        {
            // Если персонаж жив и собраны все мозги
            if (character.Life && character.Parameters.Brains == 0)
            {
                // Отображаем финишный текст
                character.Parameters.TextFinish.color = Color.white;
                // Отображаем кнопку завершения уровня
                character.Control.ButtonAction(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();

        if (character)
        {
            // Скрываем финишный текст
            character.Parameters.TextFinish.color = new Color(1, 1, 1, 0);
            // Скрываем кнопку действия
            character.Control.ButtonAction(false);
        }
    }
}