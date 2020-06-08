using UnityEngine;

namespace Cubra
{
    public abstract class BaseObjects : MonoBehaviour
    {
        /// <summary>
        /// Ссылка на текущий объект
        /// </summary>
        public GameObject InstanseObject { get; private set; }

        /// <summary>
        /// Ссылка на Transform объекта
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Ссылка на SpriteRenderer объекта
        /// </summary>
        public SpriteRenderer SpriteRenderer { get; private set; }

        protected virtual void Awake()
        {
            InstanseObject = gameObject;
            Transform = InstanseObject.transform;
            SpriteRenderer = InstanseObject.GetComponent<SpriteRenderer>();
        }
    }
}