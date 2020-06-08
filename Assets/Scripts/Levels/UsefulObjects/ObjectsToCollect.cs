using UnityEngine;

namespace Cubra
{
    public abstract class ObjectsToCollect : CollisionObjects
    {
        // Перечисление полезных объектов
        public enum UsefulObjects { Brain, Coin }
    }
}