using UnityEngine;

namespace _Game._Scripts
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;


        private void OnValidate()
        {
            if (!_spriteRenderer) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }


        private void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}