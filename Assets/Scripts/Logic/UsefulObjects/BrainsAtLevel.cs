using System;
using UnityEngine;

namespace Logic.UsefulObjects
{
    public class BrainsAtLevel : MonoBehaviour
    {
        public int Brains { get; private set; }
        public const int InitialValue = 3;

        public event Action BrainsChanged;

        public void ResetBrainValue()
        {
            Brains = InitialValue;
            BrainsChanged?.Invoke();
        }

        public void SubtractBrains(int value)
        {
            Brains -= value;
            BrainsChanged?.Invoke();
        }
    }
}