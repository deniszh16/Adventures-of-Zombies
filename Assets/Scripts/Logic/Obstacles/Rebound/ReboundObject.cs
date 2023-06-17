using Logic.Characters;
using Services.Sound;
using UnityEngine;

namespace Logic.Obstacles.Rebound
{
    public class ReboundObject : MonoBehaviour
    {
        [Header("Вектор отскока")]
        [SerializeField] protected Vector2 _direction;
        
        [Header("Сила отскока")]
        [SerializeField] protected float _force;
        
        [Header("Анимация")]
        [SerializeField] protected Animator _animator;
        
        [Header("Компонент звука")]
        [SerializeField] protected PlayingSound _playingSound;

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out Character character))
            {
                if (character.Life)
                {
                    Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                    rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);
                    
                    _playingSound.PlaySound();
                    _animator.enabled = true;
                    _animator.Rebind();
                }
            }
        }
    }
}