using UnityEngine;

namespace Cubra
{
    public class ActiveZombie : MonoBehaviour
    {
        [Header("Спрайты персонажей")]
        [SerializeField] private Sprite[] _zombies;

        // Ссылка на компонент
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // Устанавливаем изображение выбранного зомби
            _spriteRenderer.sprite = _zombies[PlayerPrefs.GetInt("character") - 1];
        }
    }
}