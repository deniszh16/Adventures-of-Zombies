using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [Header("Кнопка действия")]
    [SerializeField] private GameObject action;

    // Вектор движения персонажа
    public Vector2 Vector { get; set; }

    // Ссылка на параметры
    private Parameters parameters;

    private void Start()
    {
        // Получаем компонент параметров
        parameters = Camera.main.GetComponent<Parameters>();
    }

    // Нажатие на стрелку движения
    public void ButtonArrowDown(float value)
    {
        // Если активен игровой режим, устанавливаем вектор движения
        if (parameters.Mode == "play") Vector = Vector2.right * value;
    }

    // Отжатие стрелки движения
    public void ButtonArrowUp()
    {
        // Сбрасываем вектор движения
        Vector = Vector2.zero;
    }

    // Нажатие на кнопку прыжка
    public void ButtonJump()
    {
        // Если активен игровой режим, запускаем прыжок персонажа
        if (parameters.Mode == "play") parameters.Character.Jump();
    }

    // Отображение/скрытие кнопки действия
    public void ButtonAction(bool state)
    {
        // Устанавливаем состояние кнопки
        action.SetActive(state);
    }

    //Управление на клавиатуре
    #if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        // Если персонаж жив
        if (parameters.Character.Life)
        {
            // При нажатии на стрелку влево, устанавливаем соответствующий вектор
            if (Input.GetKey("left")) Vector = Vector2.left;

            // При нажатии на стрелку вправо, устанавливаем соответствующий вектор
            if (Input.GetKey("right")) Vector = Vector2.right;

            // При нажатии на клавишу прыжка, выполняется прыжок
            if (Input.GetKey("space")) parameters.Character.Jump();
        }

        // Сбрасываем вектор, если кнопки движения не используются
        if (!Input.GetKey("left") && !Input.GetKey("right")) Vector = Vector2.zero;
    }
    #endif
}