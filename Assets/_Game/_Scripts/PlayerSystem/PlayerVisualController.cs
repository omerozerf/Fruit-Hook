using System.Collections;
using _Game._Scripts.ScriptableObjects;
using UnityEngine;

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
            if (!_settings)
            {
                Debug.LogError($"{nameof(PlayerVisualController)} on '{name}' has no PlayerVisualSettings assigned.");
                enabled = false;
            }
        }


        private IEnumerator DamageFlashCoroutine()
        {
            if (_spriteRendererArray == null)
                yield break;

            foreach (var spriteRenderer in _spriteRendererArray)
                if (spriteRenderer)
                    spriteRenderer.color = Color.black;

            yield return new WaitForSeconds(_settings.DamageFlashDuration);

            foreach (var spriteRenderer in _spriteRendererArray)
                if (spriteRenderer)
                    spriteRenderer.color = Color.white;
        }


        public void PlayDamageFlash()
        {
            StopAllCoroutines();
            StartCoroutine(DamageFlashCoroutine());
        }
    }
}