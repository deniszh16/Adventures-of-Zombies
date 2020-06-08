using UnityEngine;

namespace Cubra
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            // Находим все объекты с фоновой музыкой
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

            // Если их больше одного, уничтожаем лишние
            if (objs.Length > 1) Destroy(gameObject);

            // Запрещяем уничтожение объекта
            DontDestroyOnLoad(gameObject);
        }
    }
}