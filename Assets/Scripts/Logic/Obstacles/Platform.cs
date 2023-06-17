using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Platform : MonoBehaviour
    {
        private Transform _previousContainer;
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    _previousContainer = character.transform.parent;
                    character.transform.parent = transform.parent;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Character character))
                character.transform.parent = _previousContainer;
        }
    }
}