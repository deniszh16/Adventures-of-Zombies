using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Ограничение камеры")]
    public bool limit = false;

    // Скорость движения камеры
    private float speed = 3.5f;

    // Ссылка на персонажа
    private Character character;
    // Позиция персонажа
    private Vector3 position;

    private void Start()
    {
        // Находим персонажа на сцене и получаем его компонент
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<Character>();

        if (!limit)
            // Если не используется ограничение камеры, перемещаем ее к персонажу
            transform.position = new Vector3(character.transform.position.x, character.transform.position.y, -10);
    }

    private void FixedUpdate()
    {
        // Получаем текущую позицию персонажа и смещаем позицию по оси Z
        position = new Vector3(character.transform.position.x, character.transform.position.y, -10);

        // Запрещаем перемещение камеры ниже нуля по оси Y
        if (position.y <= 0) position.y = 0;

        // Если не используется ограничение и персонаж живой
        if (!limit && character.Life)
            // Плавно перемещаем камеру к персонажу
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.fixedDeltaTime);  
    }
}