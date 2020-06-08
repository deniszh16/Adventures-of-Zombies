using System;
using UnityEngine;

namespace Cubra.Helpers
{
    public class ButtonUpdateHelper : MonoBehaviour
    {
        // Событие по смене персонажа
        public event Action ZombieSelected;

        /// <summary>
        /// Обновляем кнопки выбора у подписанных персонажей
        /// </summary>
        public void UpdateButtons()
        {
            ZombieSelected?.Invoke();
        }

        private void OnDestroy()
        {
            ZombieSelected = null;
        }
    }
}