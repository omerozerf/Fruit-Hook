using UnityEngine;

namespace _Game._Scripts
{
    public class SwordBubble : MonoBehaviour
    {
        [SerializeField] private Transform _swordTransform;
        [SerializeField] private float _rotationSpeed = 180f;
        
        private void Update()
        {
            RotateSword();
        }

        private void RotateSword()
        {
            if (_swordTransform == null)
                return;

            _swordTransform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
    }
}