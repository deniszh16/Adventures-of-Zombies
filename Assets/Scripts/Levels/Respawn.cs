using UnityEngine;

public class Respawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажа у конувшегося объекта
        var character = collision.GetComponent<Character>();

        // Если персонаж жив
        if (character && character.Life)
            // Записываем в респаун текущую позицию персонажа
            character.RespawnPosition = transform.position + Vector3.up;
    }
}