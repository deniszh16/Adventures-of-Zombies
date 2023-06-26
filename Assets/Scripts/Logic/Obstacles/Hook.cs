using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Hook : MonoBehaviour
    {
        private bool _busy;
        private Transform _parent;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    _parent = character.transform.parent;
                    CharacterControl characterControl = character.GetComponent<CharacterControl>();
                    characterControl.IsHanging = true;
                    characterControl.HangOnHookAnimation();
                    character.transform.parent = transform;
                    _busy = true;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_busy)
            {
                if (other.TryGetComponent(out Character character))
                {
                    if (character.Life)
                    {
                        if (character.transform.localPosition.x < 0.5f && character.transform.localPosition.y < -0.35f)
                        {
                            character.GetComponent<CharacterControl>().HangOnHook();
                            _busy = false;
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    character.transform.parent = _parent;
                    
                    CharacterControl characterControl = character.GetComponent<CharacterControl>();
                    characterControl.RestoreSpeed();
                    characterControl.RestoreGravity();
                    
                }
            }
        }
    }
}