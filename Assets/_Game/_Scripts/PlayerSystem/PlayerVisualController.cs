using UnityEngine;
using UnityEngine.Serialization;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerVisualController : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private SpriteRenderer[] _spriteRendererArray;

        public void PlayDamageFlash()
        {
            StopAllCoroutines();
            StartCoroutine(DamageFlashCoroutine());
        }

        private System.Collections.IEnumerator DamageFlashCoroutine()
        {
            if (_spriteRendererArray == null)
                yield break;

            foreach (var spriteRenderer in _spriteRendererArray)
            {
                if (spriteRenderer)
                {
                    spriteRenderer.color = Color.black;
                }
            }

            yield return new WaitForSeconds(_duration);

            foreach (var spriteRenderer in _spriteRendererArray)
            {
                if (spriteRenderer)
                {
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }
}