using UnityEngine;
using UnityEngine.Serialization;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerVisualController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private PlayerVisualSettingsSO _settings;

        [Header("References")]
        [SerializeField] private SpriteRenderer[] _spriteRendererArray;

        private void Awake()
        {
            if (_settings == null)
            {
                Debug.LogError($"{nameof(PlayerVisualController)} on '{name}' has no PlayerVisualSettings assigned.");
                enabled = false;
                return;
            }
        }

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

            yield return new WaitForSeconds(_settings.DamageFlashDuration);

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