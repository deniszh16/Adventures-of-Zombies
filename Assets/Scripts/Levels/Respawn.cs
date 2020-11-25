using UnityEngine;

namespace Cubra
{
    public class Respawn : CollisionObjects
    {
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
                GameManager.Instance.CharacterController.RespawnPosition = transform.position + Vector3.up;
        }
    }
}