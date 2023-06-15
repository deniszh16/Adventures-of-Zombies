using Logic.Characters;
using UnityEngine;

namespace Logic.Levels
{
    public class Respawn : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Character character))
                character.RespawnPosition = transform.position + Vector3.up;
        }
    }
}