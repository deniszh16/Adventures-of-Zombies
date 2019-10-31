using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [Header("Кнопка действия")]
    [SerializeField] private GameObject action;

    // Вектор движения персонажа
    public Vector2 Vector { get; private set; }

    // Ссылка на игровые параметры
    private Parameters parameters;

    private void Start()
    {
        parameters = Camera.main.GetComponent<Parameters>();
    }

    /// <summary>Нажатие на кнопку движения (значение вектора)</summary>
    public void ButtonArrowDown(float value)
    {
        // Если активен игровой режим
        if (parameters.Mode == "play")
            // Устанавливаем вектор движения
            Vector = Vector2.right * value;
    }

    /// <summary>Отжатие кнопки движения</summary>
    public void ButtonArrowUp()
    {
        // Сбрасываем вектор движения
        Vector = Vector2.zero;
    }

    /// <summary>Нажатие на кнопку прыжка</summary>
    public void ButtonJump()
    {
        // Если активен игровой режим
        if (parameters.Mode == "play")
            // Выполняем прыжок персонажа
            parameters.Character.Jump();
    }

    /// <summary>Видимость кнопки действия (состояние кнопки)</summary>
    public void ButtonAction(bool state)
    {
        action.SetActive(state);
    }

    //Управление на клавиатуре (для тестирования)
    #if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (parameters.Character.Life && parameters.Mode == "play")
        {
            // При нажатии на клавишу направления, устанавливаем вектор
            if (Input.GetKey("left")) Vector = Vector2.left;
            if (Input.GetKey("right")) Vector = Vector2.right;

            // При нажатии на клавишу прыжка, выполняем прыжок
            if (Input.GetKeyDown("space")) parameters.Character.Jump();
        }

        // Сбрасываем вектор, если кнопки движения не используются
        if (!Input.GetKey("left") && !Input.GetKey("right")) Vector = Vector2.zero;
    }
    #endif
}