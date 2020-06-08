using UnityEngine;

namespace Cubra
{
    public class ObjectInRiver : BaseObjects
    {
        [Header("Масса в воде")]
        [SerializeField] private float _massAfloat;

        public float MassAfloat => _massAfloat;

        // Ссылка на физический компонент
        public Rigidbody2D Rigidbody { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Rigidbody = InstanseObject.GetComponent<Rigidbody2D>();
        }
    }
}