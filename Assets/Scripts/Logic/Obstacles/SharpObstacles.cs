using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class SharpObstacles : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    character.DamageToCharacter();
                    character.ShowBloodEffect();
                    character.ShowDeathAnimation();
                    character.PlayDeadSound();
                }
            }
        }
    }
}