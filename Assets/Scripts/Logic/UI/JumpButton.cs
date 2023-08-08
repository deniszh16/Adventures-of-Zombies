using Logic.Characters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logic.UI
{
    public class JumpButton : MonoBehaviour
    {
        [Header("Кнопка прыжка")]
        [SerializeField] private EventTrigger _eventTrigger;
        
        [Header("Персонаж")]
        [SerializeField] private CharacterControl _characterControl;

        private void Awake()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            _eventTrigger.triggers.Add(entry);
        }

        private void OnPointerDownDelegate(PointerEventData arg0) =>
            _characterControl.Jump();
    }
}