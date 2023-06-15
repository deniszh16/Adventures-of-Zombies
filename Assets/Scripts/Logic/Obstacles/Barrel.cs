using System.Collections;
using Logic.Camera;
using Logic.Characters;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Barrel : MonoBehaviour
    {
        [Header("Компоненты бочки")]
        [SerializeField] private Animator _animator;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        [Header("Эффект взрыва")]
        [SerializeField] private GameObject _destruction;

        [Header("Игровая камера")]
        [SerializeField] private GameCamera _gameCamera;
        
        private static readonly int ActiveAnimation = Animator.StringToHash("Active");
        
        private bool _active;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_active == false)
            {
                if (col.gameObject.TryGetComponent(out Character character))
                {
                    if (character.Life)
                    {
                        _active = true;
                        _ = StartCoroutine(DestroyBarrel());
                    }
                }
            }
        }

        private IEnumerator DestroyBarrel()
        {
            _animator.SetTrigger(ActiveAnimation);
            yield return new WaitForSeconds(1f);
            _boxCollider.enabled = false;
            _destruction.gameObject.SetActive(true);
            _gameCamera.ShakeCamera(0.6f, 2.1f, 2f);
        }
    }
}