using StateMachine.States;
using UnityEngine;

namespace Logic.Bosses.Bull.States
{
    public class DieState : BaseStates
    {
        [Header("Компоненты босса")]
        [SerializeField] private Boss _boss;
        [SerializeField] private Animator _animator;
        [SerializeField] private CapsuleCollider2D _capsuleCollider;
        
        [Header("Объекты уровня")]
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField] private GameObject[] _objectsFinish;
        
        private static readonly int DeadAnimation = Animator.StringToHash("Dead");
        
        public override void Enter()
        {
            _animator.SetTrigger(DeadAnimation);
            _capsuleCollider.enabled = false;
            
            for (int i = 0; i < _obstacles.Length; i++)
                _obstacles[i].SetActive(false);
            
            for (int i = 0; i < _objectsFinish.Length; i++)
                _objectsFinish[i].SetActive(true);
            
            _boss.SnapCameraToCharacter();
        }

        public override void Exit()
        {
        }
    }
}