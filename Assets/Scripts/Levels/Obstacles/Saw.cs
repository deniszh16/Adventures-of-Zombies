using UnityEngine;

public abstract class Saw : MonoBehaviour
{
    [Header("Скорость вращения")]
    [SerializeField] protected float rotate;

    protected abstract void Update();
}