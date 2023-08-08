using System.Collections;
using UnityEngine;

namespace Logic.Obstacles
{
    public class Portal : MonoBehaviour
    {
        [Header("Пауза для переходов")]
        [SerializeField] private float _pause;
        
        [Header("Точки переходов")]
        [SerializeField] private Vector3[] _points;

        private Transform _transform;
        private int _point = 0;

        private void Awake() =>
            _transform = transform;

        private void Start() =>
            _ = StartCoroutine(TeleportPlatform());

        private IEnumerator TeleportPlatform()
        {
            while (true)
            {
                yield return new WaitForSeconds(_pause);
                _point++;
                
                if (_point > _points.Length - 1)
                    _point = 0;
                
                _transform.position = _points[_point];
            }
        }
    }
}