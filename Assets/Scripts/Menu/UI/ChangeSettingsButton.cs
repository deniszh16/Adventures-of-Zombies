using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public abstract class ChangeSettingsButton : MonoBehaviour
    {
        [Header("Спрайты кнопок")]
        [SerializeField] protected Sprite[] _sprites;
        
        protected Image _imageButton;

        protected virtual void Awake()
        {
            _imageButton = GetComponent<Image>();
        }

        /// <summary>
        /// Изменение спрайта кнопки настроек
        /// </summary>
        /// <param name="number">номер спрайта</param>
        protected virtual void ChangeSprite(int number)
        {
            _imageButton.sprite = _sprites[number];
        }
    }
}