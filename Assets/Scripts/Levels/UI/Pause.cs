using UnityEngine;

namespace Cubra
{
    public class Pause : MonoBehaviour
    {
        public void TogglePause(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
        }
    }
}