using Logic.Characters;
using UnityEngine;

namespace Logic.UsefulObjects
{
    public class Bone : MonoBehaviour
    {
        [Header("Компонент костей")]
        [SerializeField] private BonesAtLevel _bonesAtLevel;
        
        //[Header("Компонент звука")]
        //[SerializeField] private BonesAtLevel _bonesAtLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<Character>())
            {
                
            }
        }
    }
}