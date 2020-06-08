using UnityEngine;

namespace Cubra.Controllers
{
    public abstract class BaseController : MonoBehaviour
    {
        // Активность контроллера
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Включение контроллера
        /// </summary>
        public virtual void Enable()
        {
            Enabled = true;
        }

        /// <summary>
        /// Отключение контроллера
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}