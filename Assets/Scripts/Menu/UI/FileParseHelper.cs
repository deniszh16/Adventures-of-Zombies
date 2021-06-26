using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Cubra.Helpers
{
    public class FileParseHelper
    {
        // Ссылка на xml документ
        public static XDocument Languages;

        /// <summary>
        /// Парсинг xml документа
        /// </summary>
        /// <param name="fileName">имя файла</param>
        public static void ParseXml(string fileName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName + ".xml");

            // Получаем данные по указанному пути
            UnityWebRequest reader = UnityWebRequest.Get(path);
            // Выполняем обработку полученнных данных
            reader.SendWebRequest();
            // Ждем завершения обработки
            while (!reader.isDone) {}

            // Преобразуем полученную строку в объект
            Languages = XDocument.Parse(reader.downloadHandler.text);
        }
    }
}