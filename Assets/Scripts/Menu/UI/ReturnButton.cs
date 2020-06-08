using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class ReturnButton : MonoBehaviour
    {
        [Header("Сцена для перехода")]
        [SerializeField] private TransitionsController.Scenes _scene;

        // Ссылка на контроллер переходов
        private TransitionsController _transitionsController;

        private void Awake()
        {
            _transitionsController = gameObject.GetComponent<TransitionsController>();
        }

        private void Update()
        {
            // Если нажата кнопка возврата
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Переходим на указанную сцену
                _transitionsController.GoToScene((int)_scene);
            }
        }
    }
}