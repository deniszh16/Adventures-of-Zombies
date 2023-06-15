using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class SharpObstacles : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    character.DamageToCharacter();
                    character.ShowDeathAnimation();
                    character.PlayDeadSound();
                }
            }
        }
    }
}