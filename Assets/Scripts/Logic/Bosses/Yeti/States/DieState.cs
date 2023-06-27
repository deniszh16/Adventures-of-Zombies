using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Yeti.States
{
    public class DieState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Yeti _yeti;
        [SerializeField] private Animator _animator;
        [SerializeField] private CapsuleCollider2D _capsuleCollider;

        [Header("Объекты уровня")]
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField] private GameObject[] _objectsFinish;
        
        private static readonly int DieAnimation = Animator.StringToHash("Die");
        
        public override void Enter()
        {
            _animator.SetTrigger(DieAnimation);
            _capsuleCollider.enabled = false;

            for (int i = 0; i < _obstacles.Length; i++)
                _obstacles[i].SetActive(false);
            
            for (int i = 0; i < _objectsFinish.Length; i++)
                _objectsFinish[i].SetActive(true);
            
            _yeti.SnapCameraToCharacter();
        }

        public override void Exit()
        {
        }
    }
}