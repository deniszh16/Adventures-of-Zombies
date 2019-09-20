using UnityEngine;

public class Hook : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.GetComponent<Character>();

        // Если персонаж живой
        if (character && character.Life)
        {
            // Активируем переменную виса
            character.IsHook = true;
            // Активируем переменную зацепа
            character.CatchHook = true;

            // Назначаем крюк родительским объектом для персонажа
            character.transform.parent = transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Получаем компонент персонажа у касающегося объекта
        var character = collision.GetComponent<Character>();

        if (character && character.Life)
            // Если персонаж находится в указанном диапазоне и переменная зацепа активна
            if (character.transform.localPosition.x < 0.5f && character.transform.localPosition.y < -1f && character.CatchHook)
                // Фиксируем персонажа на крюке
                character.ClingToHook();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Получаем компонент персонажа у касающегося объекта
        var character = collision.GetComponent<Character>();

        if (character)
        {
            // Сбрасываем родительский объект для персонажа
            character.transform.parent = null;
            // Восстанавливаем стандартную гравитацию
            character.Rigbody.gravityScale = 1.5f;
        }
    }
}