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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _transitionsController.GoToScene((int)_scene);
            }
        }
    }
}