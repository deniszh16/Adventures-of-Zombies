using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles.River
{
    public class River : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out ObjectInRiver objectInRiver))
            {
                if (col.transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer))
                    spriteRenderer.sortingOrder = 0;
                
                objectInRiver.ChangeMass();
                objectInRiver.PlaySplashEffect();

                if (col.TryGetComponent(out Character character))
                {
                    if (character.Life)
                    {
                        character.DamageToCharacter();
                        character.ShowSinkingAnimation();
                        character.GetComponent<CharacterControl>().ChangeDirectionVector();
                    }
                }
            }
        }
    }
}