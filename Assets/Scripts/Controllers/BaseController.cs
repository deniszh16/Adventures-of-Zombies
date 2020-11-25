using UnityEngine;

namespace Cubra.Controllers
{
    public abstract class BaseController : MonoBehaviour
    {
        public bool Enabled { get; set; } = true;

        public virtual void Enable()
        {
            Enabled = true;
        }
        
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}