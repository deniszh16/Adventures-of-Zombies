using UnityEngine;

namespace Cubra
{
    public class Barrel : CollisionObjects
    {
        // Активность бочки
        private bool _active;

        [Header("Эффект взрыва")]
        [SerializeField] private Animator _destruction;

        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        
        public override void ActionsOnEnter(Character character)
        {
            if (character.Life)
            {
                if (_active == false)
                {
                    _animator.enabled = true;
                    _active = true;
                }
            }
        }

        /// <summary>
        /// Уничтожение бочки
        /// </summary>
        public void DestroyBarrel()
        {
            _destruction.transform.position = Transform.position;

            if (_destruction.enabled == false) _destruction.enabled = true;
            _destruction.Rebind();

            PlayerPrefs.SetInt("barrel", PlayerPrefs.GetInt("barrel") + 1);
            InstanseObject.SetActive(false);
        }
    }
}