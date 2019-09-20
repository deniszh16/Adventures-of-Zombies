using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameLocalization : MonoBehaviour
{
    private void Awake() { ParseTranslation.ParseXml(); }
}

// Локализация игры
public static class ParseTranslation {

    // Ссылка на документ
    public static XDocument languages;

    public static void ParseXml()
    {
        // Получаем путь до xml файла
        string path = Path.Combine(Application.streamingAssetsPath, "languages.xml");

        // Получаем данные по указанному пути
        UnityWebRequest reader = UnityWebRequest.Get(path);
        // Выполняем обработку полученнных данных
        reader.SendWebRequest();
        // Ждем завершения обработки
        while (!reader.isDone) {}

        // Преобразуем полученную строку в объект
        languages = XDocument.Parse(reader.downloadHandler.text);
    }
}