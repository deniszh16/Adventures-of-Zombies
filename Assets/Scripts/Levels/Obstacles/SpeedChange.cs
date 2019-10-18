using UnityEngine;

public class SpeedChange : MonoBehaviour
{
    [Header("Новая скорость")]
    [SerializeField] private float updateSpeed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.GetComponent<Character>();

        // Если персонаж живой
        if (character.Life)
            // Если персонаж двигается, устанавливается новая скорость
            if (character.Control.Vector.x != 0) character.Speed = updateSpeed;
    }
}