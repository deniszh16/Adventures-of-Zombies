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
        
        private int _point = 0;

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
                
                transform.position = _points[_point];
            }
        }
    }
}