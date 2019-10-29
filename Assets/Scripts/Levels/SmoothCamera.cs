using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Ограничение камеры")]
    [SerializeField] private bool limit = false;

    // Свойство для настройки ограничения камеры
    public bool Limit { get { return limit; } set { limit = value; } }

    // Скорость движения камеры
    private float speed = 3f;

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