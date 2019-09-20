using UnityEngine;
using UnityEngine.UI;

public class ChoiceCharacter : MonoBehaviour
{
    // Список доступности персонажей
    public static bool[] characters = { true, false, false, false, false };

    [Header("Кнопки выбора")]
    [SerializeField] private Button[] buttons;

    // Выбор персонажа
    public void ChooseCharacter(int number)
    {
        // Сохраняем номер выбранного персонажа
        PlayerPrefs.SetInt("character", number);

        for (int i = 0; i < characters.Length; i++)
        {
            // Если персонаж открыт, получаем компонент персонажа и проверяем кнопку
            if (characters[i]) buttons[i].gameObject.GetComponentInParent<OpeningCharacter>().CheckSelectButton();
        }
    }
}