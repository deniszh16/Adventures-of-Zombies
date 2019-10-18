using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [Header("Номер уровня")]
    [SerializeField] private int number;

    [Header("Карточка открытого уровня")]
    [SerializeField] private Sprite openLevel;

    [Header("Панель перехода")]
    [SerializeField] private Animator blackout;

    private Image image;
    private TransitionsInMenu transitions;

    private void Awake()
    {
        image = GetComponent<Image>();
        blackout = blackout.GetComponent<Animator>();
        transitions = Camera.main.GetComponent<TransitionsInMenu>();
    }

    private void Start()
	{
		// Если номер уровня меньше прогресса, отображаем открытую карточку
		if (number <= PlayerPrefs.GetInt("progress")) image.sprite = openLevel;
	}

    /// <summary>Переход на уровень</summary>
    public virtual void OpenLevel()
	{
        // Если номер уровня меньше прогресса
        if (number <= PlayerPrefs.GetInt("progress"))
        {
            // Отображаем анимацию затемнения
            blackout.enabled = true;
            // Через полторы секунды загружаем сцену
            Invoke("LoadScene", 1.5f);
        }
	}

    /// <summary>Загрузка выбранного уровня</summary>
    protected virtual void LoadScene()
    {
        transitions.GoToScene(number.ToString());
    }
}