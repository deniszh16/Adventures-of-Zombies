using UnityEngine;

namespace Logic.Obstacles
{
    public class FallingSaw : MonoBehaviour
    {
        [Header("Компоненты пилы")]
        [SerializeField] private Rigidbody2D _rigidbody;
        
        private void OnEnable() =>
            _rigidbody.velocity = Vector2.zero;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<River.River>())
                gameObject.SetActive(false);
        }
    }
}