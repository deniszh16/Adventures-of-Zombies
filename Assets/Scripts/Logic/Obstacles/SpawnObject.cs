using System.Collections;
using UnityEngine;

namespace Logic.Obstacles
{
    public class SpawnObject : MonoBehaviour
    {
        [Header("Время до создания")]
        [SerializeField] private float _pause;
        
        [Header("Пул объектов")]
        [SerializeField] private GameObject _objectPool;

        private int _numberObjectFromPool;

        private void Start() =>
            _ = StartCoroutine(CreateObject());

        private IEnumerator CreateObject()
        {
            var seconds = new WaitForSeconds(_pause);
            
            while (true)
            {
                yield return seconds;
                if (_objectPool.transform.childCount > 0)
                {
                    Transform obj = _objectPool.transform.GetChild(_numberObjectFromPool);
                    obj.position = transform.position;
                    obj.rotation = transform.rotation;

                    obj.gameObject.SetActive(true);
                    _numberObjectFromPool++;

                    if (_numberObjectFromPool >= _objectPool.transform.childCount)
                        _numberObjectFromPool = 0;
                }
            }
        }
    }
}