using UnityEngine;

namespace Logic.Obstacles.River
{
    public class ObjectInRiver : MonoBehaviour
    {
        [Header("Физический компонент")]
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("Масса в воде")]
        [SerializeField] private float _massAfloat;
        
        [Header("Эффект брызг")]
        [SerializeField] private ParticleSystem _spray;
        [SerializeField] private float _offsetEffect;

        public void ChangeMass()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.mass = _massAfloat;
        }

        public void PlaySplashEffect()
        {
            var position = transform.position;
            _spray.transform.position = new Vector3(position.x, position.y - _offsetEffect, 0);
            _spray.Play();
        }
    }
}